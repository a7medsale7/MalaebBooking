using Hangfire; // 👈 إضافة Hangfire
using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Auth;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Authentication;
using MalaebBooking.Infrastructure.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace MalaebBooking.Application.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ILogger<AuthService> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;
    private readonly IJwtProvider jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> logger = logger;
    private readonly IEmailSender emailSender = emailSender;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    private readonly int tokenExpiryDayes = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(
     string email,
     string password,
     CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (!user.EmailConfirmed)
            return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiration) = jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(tokenExpiryDayes);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiry
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse
        {
            Id = user.Id,
            Name = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token,
            ExpiresIn = (int)(expiration - DateTime.UtcNow).TotalSeconds,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiry
        };

        return Result.Success(response);
    }


    public async Task<Result> RegisterAsync(
    RegisterRequest registerRequest,
    CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);

        if (existingUser is not null)
            return Result.Failure(UserErrors.EmailAlreadyExists);

        var user = new ApplicationUser
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
            return Result.Failure(
                new Error("User.CreationFailed",
                string.Join(", ", result.Errors.Select(e => e.Description))));

        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        emailToken = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(emailToken));

        logger.LogInformation(
            "Email confirmation token for user {Email}: {Token}",
            user.Email,
            emailToken);

        var origin = httpContextAccessor.HttpContext?.Request.Headers["Origin"].FirstOrDefault()
                     ?? "https://yourfrontend.com";

        var confirmationLink =
            $"{origin}/confirm-email?userId={user.Id}&code={emailToken}";

        var emailBody = EmailBodyBuilder.GenerateEmailBody(
            "EmailConfirmation",
            new Dictionary<string, string>
            {
            { "{{UserName}}", user.FirstName },
            { "{{ConfirmationLink}}", confirmationLink },
            { "{{Year}}", DateTime.UtcNow.Year.ToString() }
            });

        // 👈 استخدام Hangfire مباشرة هنا لرسال الإيميل في الخلفية
        BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(
            user.Email!,
            "Confirm your email",
            emailBody));

        return Result.Success();
    }


    public async Task<Result> ConfirmEmailAsync(
    ConfirmEmailReqest request,
    CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailAlreadyConfirmed);

        string decodedToken;

        try
        {
            decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(request.Code));
        }
        catch
        {
            return Result.Failure(UserErrors.InvalidTokenFormat);
        }

        var result = await userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
            return Result.Failure(UserErrors.EmailConfirmationFailed);

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(
    ResendConfirmationEmailReqest resendConfirmation,
    CancellationToken cancellationToken)
    {
        // 1️⃣ البحث عن المستخدم
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == resendConfirmation.Email, cancellationToken);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        // 2️⃣ تحقق إن الإيميل مش Confirmed بالفعل
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailAlreadyConfirmed);

        // 3️⃣ توليد التوكين
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // 4️⃣ تشفير التوكين
        var encodedToken = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(emailToken));

        // 5️⃣ تسجيل التوكين في الـ logs
        logger.LogInformation(
            "Email confirmation token for user {Email}: {Token}",
            user.Email,
            encodedToken);

        var origin = httpContextAccessor.HttpContext?
            .Request.Headers["Origin"]
            .FirstOrDefault() ?? "https://yourfrontend.com";

        // 6️⃣ إنشاء رابط التأكيد
        var confirmationLink =
            $"{origin}/confirm-email?userId={user.Id}&code={encodedToken}";

        // 7️⃣ تجهيز محتوى الإيميل
        var emailBody = EmailBodyBuilder.GenerateEmailBody(
            "EmailConfirmation",
            new Dictionary<string, string>
            {
            {"{{UserName}}", user.FirstName },
            {"{{ConfirmationLink}}", confirmationLink },
            {"{{Year}}", DateTime.UtcNow.Year.ToString() }
            });

        // 👈 استخدام Hangfire مباشرة هنا لرسال الإيميل في الخلفية
        BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(
            user.Email!,
            "Confirm your email",
            emailBody));

        return Result.Success();
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(
     string token,
     string refreshToken,
     CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var userRefreshToken = user.RefreshTokens
            .FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.RefreshTokenNotFound);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiration) = jwtProvider.GenerateToken(user);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(tokenExpiryDayes);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiry
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse
        {
            Id = user.Id,
            Name = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = newToken,
            ExpiresIn = (int)(expiration - DateTime.UtcNow).TotalSeconds,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiration = refreshTokenExpiry
        };

        return Result.Success(response);
    }
    public async Task<Result> RevokeRefreshTokenAsync(
     string token,
     string refreshToken,
     CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var userRefreshToken = user.RefreshTokens
            .FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.RefreshTokenNotFound);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}

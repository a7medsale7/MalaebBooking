using MalaebBooking.Application.Contracts.Auth;
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
    IJwtProvider jwtProvider ,
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

    private readonly int tokenExpiryDayes = 14; // ممكن تجيبه من الإعدادات بدل ما يكون ثابت



    public async Task<AuthResponse?> GetTokenAsync(
     string email,
     string password,
     CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        // 🚨 مهم جداً
        if (!user.EmailConfirmed)
            throw new InvalidOperationException("Email is not confirmed.");

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
            return null;

        var (token, expiration) = jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(tokenExpiryDayes); // مثلاً، ممكن تجيبه من الإعدادات

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiry
        });
        await _userManager.UpdateAsync(user);



        return new AuthResponse
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
    }

    public async Task RegisterAsync(
     RegisterRequest registerRequest,
     CancellationToken cancellationToken)
    {
        // 1️⃣ Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
        if (existingUser is not null)
            throw new InvalidOperationException("Email already exists.");

        // 2️⃣ Create user
        var user = new ApplicationUser
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));

        // 3️⃣ Generate Email Confirmation Token
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        emailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
        logger.LogInformation("Email confirmation token for user {Email}: {Token}", user.Email, emailToken);


        // هنا المفروض تبعت ايميل باللينك
        // var confirmationLink = $"{yourUrl}/confirm-email?userId={user.Id}&token={emailToken}";
        // await emailService.SendAsync(user.Email, confirmationLink);
        // خذ الـ Origin من الهيدر لو موجود، لو مش موجود استخدم fallback URL
        var origin = httpContextAccessor.HttpContext?.Request.Headers["Origin"].FirstOrDefault()
                     ?? "https://yourfrontend.com";

        // حضّر رابط التأكيد
        var confirmationLink = $"{origin}/confirm-email?userId={user.Id}&code={emailToken}";

        // حضّر محتوى الإيميل باستخدام الـ template والمتغيرات
        var emailBody = EmailBodyBuilder.GenerateEmailBody(
            "EmailConfirmation",
            new Dictionary<string, string>
            {
        { "{{UserName}}", user.FirstName },
        { "{{ConfirmationLink}}", confirmationLink },
        { "{{Year}}", DateTime.UtcNow.Year.ToString() }
            }
        );
        await emailSender.SendEmailAsync(user.Email, "Confirm your email", emailBody);

        // مفيش return 👌
    }



    public async Task ConfirmEmailAsync(ConfirmEmailReqest request , CancellationToken cancellationToken)
    {
        // 1️⃣ تحقق إن المستخدم موجود
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new InvalidOperationException("Invalid user ID.");

        // 2️⃣ تحقق إن الإيميل مش Confirmed قبل كده
        if (user.EmailConfirmed)
            throw new InvalidOperationException("Email already confirmed.");

        // 3️⃣ فك التوكين
        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        }
        catch (FormatException)
        {
            throw new InvalidOperationException("Invalid token format.");
        }

        // 4️⃣ Confirm Email
        var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            throw new InvalidOperationException(error != null
                ? $"{error.Code}: {error.Description}"
                : "Email confirmation failed.");
        }

        // ✅ لو كله تمام، مفيش حاجة تتعمل، الميثود انتهت
    }

    public async Task ResendConfirmationEmailAsync(ResendConfirmationEmailReqest resendConfirmation, CancellationToken cancellationToken)
    {
        // 1️⃣ البحث عن المستخدم
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == resendConfirmation.Email, cancellationToken);
        if (user is null)
            throw new InvalidOperationException("Email not found!");

        // 2️⃣ توليد التوكين
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // 3️⃣ تشفير التوكين لـ URL
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));

        // 4️⃣ تسجيل التوكين في الـ logs (ممكن تبعته في الإيميل بعد كده)
        logger.LogInformation("Email confirmation token for user {Email}: {Token}", user.Email, encodedToken);

        // 5️⃣ لو عندك Email Service:
        // var confirmationLink = $"{yourFrontendUrl}/confirm-email?userId={user.Id}&code={encodedToken}";
        // await emailService.SendAsync(user.Email, "Confirm your email", confirmationLink, cancellationToken);

        var origin = httpContextAccessor.HttpContext?.Request.Headers["Origin"].FirstOrDefault()
                    ?? "https://yourfrontend.com";

        // حضّر رابط التأكيد
        var confirmationLink = $"{origin}/confirm-email?userId={user.Id}&code={emailToken}";

        // حضّر محتوى الإيميل باستخدام الـ template والمتغيرات
        var emailBody = EmailBodyBuilder.GenerateEmailBody(
            "EmailConfirmation",
            new Dictionary<string, string>
            {
        {"{{UserName}}", user.FirstName },
        { "{{ConfirmationLink}}", confirmationLink },
        { "{{Year}}", DateTime.UtcNow.Year.ToString() }
            }
        );
        await emailSender.SendEmailAsync(user.Email, "Confirm your email", emailBody);

    }

    public async Task<AuthResponse?> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(token);
        if (userId is null)
            return null;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return null;

        var userRefreshToken = user.RefreshTokens
            .FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);

        if (userRefreshToken is null)
            return null;

        // revoke old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        // generate new jwt
        var (newToken, expiration) = jwtProvider.GenerateToken(user);

        // generate new refresh token
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(tokenExpiryDayes);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiry
        });

        await _userManager.UpdateAsync(user);

        return new AuthResponse
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
    }
    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(token);
        if (userId is null)
            return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        var userRefreshToken = user.RefreshTokens
            .FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return false;

        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return true;



    }


    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    
}
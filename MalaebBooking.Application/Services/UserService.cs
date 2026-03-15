using Hangfire;
using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Users;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    public UserService(
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }
    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);
        return Result.Success(new UserProfileResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber
        });
    }
    public async Task<Result<UserProfileResponse>> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        await _userManager.UpdateAsync(user);
        return Result.Success(new UserProfileResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber
        });
    }
    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            // ممكن ترجع أول إيرور بيحصل (زي Password too short وهكذا)
            return Result.Failure(new Error("User.InvalidPassword", result.Errors.First().Description));
        }
        return Result.Success();
    }
    public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request, string originUrl)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        // مش بنرجع Error لو الإيميل مش موجود عشان الـ Security (عشان محدش يعرف الإيميلات المسجلة)
        if (user is null)
            return Result.Success();
        // 1. توليد التوكن
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // 2. تشفير التوكن عشان يتبعت في الـ URL
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        // 3. بناء لينك الـ Reset (زي لينك الفرونت إند)
        var resetLink = $"{originUrl}/reset-password?email={user.Email}&token={encodedToken}";
        // 4. إرسال الإيميل في الخلفية بـ Hangfire
        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
            user.Email!,
            "استعادة كلمة المرور",
            $"أهلاً بك،<br>لاستعادة كلمة مرور حسابك، اضغط على الرابط التالي:<br><a href='{resetLink}'>اضغط هنا</a>"
        ));
        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);
        try
        {
            // فك تشفير التوكن اللي جاي من الـ URL
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            // الريسيت الحقيقي
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!result.Succeeded)
                return Result.Failure(UserErrors.InvalidToken);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(UserErrors.InvalidToken);
        }
    }
}
using Hangfire;
using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Roles;
using MalaebBooking.Application.Contracts.Users;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Consts;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IRoleService _roleService;
    public UserService(
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        ApplicationDbContext context,
        RoleManager<ApplicationRole> roleManager,
        IRoleService roleService)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _context = context;
        _roleManager = roleManager;
        _roleService = roleService;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
    await (from u in _context.Users
           join ur in _context.UserRoles
           on u.Id equals ur.UserId
           join r in _context.Roles
           on ur.RoleId equals r.Id into roles
           where !roles.Any(x => x.Name == DefaultRoles.Player)
           select new
           {
               u.Id,
               u.FirstName,
               u.LastName,
               u.Email,
               u.IsDisabled,
               Roles = roles.Select(x => x.Name!).ToList()
           }
            )
            .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })
           .Select(u => new UserResponse
           {
               Id = u.Key.Id,
               FirstName = u.Key.FirstName,
               LastName = u.Key.LastName,
               Email = u.Key.Email,
               IsDisabled = u.Key.IsDisabled,
               Roles = u.SelectMany(x => x.Roles)
           })
           .ToListAsync(cancellationToken);

    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var response = new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            IsDisabled = user.IsDisabled,
            Roles = roles
        };

        return Result.Success(response);
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

    public async Task<Result<UserResponse>> AddAsync(CreateUserReqeust request, CancellationToken cancellationToken = default)
    {
        // 1. التحقق من وجود البريد الإلكتروني مسبقًا (استخدمنا EmailAlreadyExists بدلاً من DisabledUser)
        var emailExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailExists)
            return Result.Failure<UserResponse>(UserErrors.EmailAlreadyExists);

        // 2. التحقق من أن الأدوار (Roles) المرسلة موجودة فعلاً في النظام
        // استخدمنا _roleManager هنا مباشرة للتحقق
        var systemRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(cancellationToken);

        if (request.Roles.Except(systemRoles).Any())
            return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

        // 3. تحويل DTO إلى ApplicationUser
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email, // الافتراضي أن اسم المستخدم هو إيميله
            EmailConfirmed = true   // نفترض أن المسؤول هو من أنشأه فيكون مفعلاً تلقائياً
        };

        // 4. إنشاء المستخدم مع كلمة المرور
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description));
        }

        // 5. إضافة المستخدم إلى الأدوار المطلوبة
        if (request.Roles.Any())
        {
            var rolesResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!rolesResult.Succeeded)
            {
                var error = rolesResult.Errors.First();
                return Result.Failure<UserResponse>(new Error(error.Code, error.Description));
            }
        }

        // 6. تجهيز الـ Response النهائي
        var response = new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            IsDisabled = user.IsDisabled,
            Roles = request.Roles
        };

        return Result.Success(response);
    }






    // --- ميثود التحديث (Update) ---
    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        // 1. التأكد من أن البريد الإلكتروني غير مستخدم من قبل شخص آخر
        var emailExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id, cancellationToken);
        if (emailExists)
            return Result.Failure(UserErrors.EmailAlreadyExists);

        // 2. جلب المستخدم والتأكد من وجوده
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        // 3. التحقق من أن الأدوار المرسلة صحيحة وموجودة في النظام
        var systemRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(cancellationToken);
        if (request.Roles.Except(systemRoles).Any())
            return Result.Failure(UserErrors.InvalidRoles);

        // 4. تحديث البيانات (يدوياً أو باستخدام Adapt)
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.UserName = request.Email;
        user.PhoneNumber = request.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // 5. تحديث الأدوار (حذف الأدوار القديمة وإضافة الجديدة)
            // الطريقة الرسمية لـ Identity (أكثر أماناً):
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, request.Roles);

            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description));
    }

    // --- ميثود تغيير الحالة (تفعيل/تعطيل) ---
    public async Task<Result> ToggleStatus(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        user.IsDisabled = !user.IsDisabled;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success(result)
            : Result.Failure(new Error(result.Errors.First().Code, result.Errors.First().Description));
    }

    // --- ميثود فك الحفل (Unlock) ---
    public async Task<Result> Unlock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        // تصفير تاريخ انتهاء الحظر لفك القفل فوراً
        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(new Error(result.Errors.First().Code, result.Errors.First().Description));
    }

}
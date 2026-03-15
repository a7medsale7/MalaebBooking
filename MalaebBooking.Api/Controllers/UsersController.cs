using MalaebBooking.Application.Contracts.Users;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // 1. عرض بيانات البروفايل (لازم يكون عامل لوجين)
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        var result = await _userService.GetProfileAsync(userId);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    // 2. تحديث بيانات البروفايل (لازم يكون عامل لوجين)
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        var result = await _userService.UpdateProfileAsync(userId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // 3. تغيير الباسورد القديم بواحد جديد (لازم يكون عامل لوجين)
    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        var result = await _userService.ChangePasswordAsync(userId, request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // 4. طلب استعادة الباسورد (بياخد الإيميل ويبعتله رابط الـ Reset في الخلفية)
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        // بنجيب أصل الرابط عشان نبعته لليوزر يدوس عليه (زي http://localhost:1234)
        var originUrl = $"{Request.Scheme}://{Request.Host.Value}";

        var result = await _userService.ForgotPasswordAsync(request, originUrl);

        // ديماً نرجع 200 OK عشان الهاكر ميعرفش مين مسجل في السيستم ومين مش مسجل
        return Ok(new { message = "إذا كان بريدك الإلكتروني مسجلاً لدينا، ستصلك رسالة تحتوي على رابط لاستعادة كلمة المرور." });
    }

    // 5. استعادة الباسورد باستخدام الـ Token اللي في الرابط
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _userService.ResetPasswordAsync(request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

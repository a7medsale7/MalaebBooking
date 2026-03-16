// 8. UsersController.cs
using MalaebBooking.Application.Contracts.Users;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("profile")]
    [Authorize(Policy = Permissions.Users_ViewProfile)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();
        var result = await _userService.GetProfileAsync(userId);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPut("profile")]
    [Authorize(Policy = Permissions.Users_UpdateProfile)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();
        var result = await _userService.UpdateProfileAsync(userId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("change-password")]
    [Authorize(Policy = Permissions.Users_ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();
        var result = await _userService.ChangePasswordAsync(userId, request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("all")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Users_ViewAll)]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok("Admin logic to get all users");
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var originUrl = $"{Request.Scheme}://{Request.Host.Value}";
        await _userService.ForgotPasswordAsync(request, originUrl);
        return Ok(new { message = "إذا كان بريدك الإلكتروني مسجلاً لدينا، ستصلك رسالة تحتوي على رابط لاستعادة كلمة المرور." });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _userService.ResetPasswordAsync(request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

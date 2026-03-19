// 8. UsersController.cs
using MalaebBooking.Application.Contracts.Users;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;

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
        var result = await _userService.GetAllAsync();
        return Ok(result);
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

    [HttpGet("GetById/{id}")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Users_ViewAll)]
    public async Task<IActionResult> GetUserById([FromRoute]string id)
    {
        var result = await _userService.GetAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }


    [HttpPost]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Users_ManageRoles)]
    public async Task<IActionResult> Add([FromBody] CreateUserReqeust request, CancellationToken cancellationToken)
    {
        var result = await _userService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetUserById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }


    // 1. أندبوينت تعديل بيانات المستخدم وأدواره
    [HttpPut("{id}")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Users_ManageRoles)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    // 2. أندبوينت تغيير حالة المستخدم (تفعيل/تعطيل)
    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Users_ManageRoles)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await _userService.ToggleStatus(id);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // 3. أندبوينت فك حظر المستخدم (Unlock)
    [HttpPatch("{id}/unlock")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Users_ManageRoles)]
    public async Task<IActionResult> Unlock([FromRoute] string id)
    {
        var result = await _userService.Unlock(id);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }


}

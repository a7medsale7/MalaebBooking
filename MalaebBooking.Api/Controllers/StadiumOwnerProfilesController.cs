using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.StadiumOwner;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StadiumOwnerProfilesController(IStadiumOwnerService ownerService) : ControllerBase
{
    private readonly IStadiumOwnerService _ownerService = ownerService;

    // 1. التقديم لطلب توثيق جديد (صاحب ملعب)
    [HttpPost("apply")]
    [Authorize(Policy = Permissions.OwnerProfiles_Apply)]
    [Consumes("multipart/form-data")] // ضروري عشان رفع الملفات
    public async Task<IActionResult> Apply([FromForm] ApplyForStadiumOwnerRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null) return Unauthorized();

        var result = await _ownerService.ApplyAsync(request, currentUserId);

        return result.IsFailure ? BadRequest(result.Error) : Ok(new { Message = "Application submitted successfully." });
    }

    // 2. عرض البروفايل الخاص بالمستخدم الحالي
    [HttpGet("my-profile")]
    [Authorize(Policy = Permissions.OwnerProfiles_View)]
    public async Task<IActionResult> GetMyProfile()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null) return Unauthorized();

        var result = await _ownerService.GetProfileByUserIdAsync(currentUserId);

        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    // 3. عرض كل الطلبات المعلقة (للأدمن فقط)
    [HttpGet("pending")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.OwnerProfiles_View)]
    public async Task<IActionResult> GetPending()
    {
        var result = await _ownerService.GetPendingProfilesAsync();
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    // 4. مراجعة الطلب: قبول أو رفض (للأدمن فقط)
    [HttpPost("{id:int}/review")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.OwnerProfiles_Review)]
    public async Task<IActionResult> Review(int id, [FromBody] ReviewStadiumOwnerRequest request)
    {
        var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (adminId is null) return Unauthorized();

        var result = await _ownerService.ReviewProfileAsync(id, adminId, request);

        return result.IsFailure ? BadRequest(result.Error) : Ok(new { Message = "Profile status updated successfully." });
    }
}

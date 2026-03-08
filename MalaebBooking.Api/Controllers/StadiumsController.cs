using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class StadiumsController : ControllerBase
{
    private readonly IStadiumService _stadiumService;

    public StadiumsController(IStadiumService stadiumService)
    {
        _stadiumService = stadiumService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _stadiumService.GetAllStadiumsAsync();

        if (result.IsFailure)
            return NotFound(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
    {
        var result = await _stadiumService.GetActiveStadiumsAsync();

        if (result.IsFailure)
            return NotFound(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _stadiumService.GetStadiumByIdAsync(id);

        if (result.IsFailure)
            return NotFound(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpGet("{id:int}/details")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDetails(int id)
    {
        var result = await _stadiumService.GetStadiumDetailsAsync(id);

        if (result.IsFailure)
            return NotFound(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpGet("sport/{sportTypeId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySportType(int sportTypeId)
    {
        var result = await _stadiumService.GetStadiumsBySportTypeAsync(sportTypeId);

        if (result.IsFailure)
            return NotFound(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStadiumRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // الأمان: التأكد من تسجيل الدخول قبل محاولة الحفظ 
        if (currentUserId is null)
        {
            return Unauthorized();
        }

        var result = await _stadiumService.CreateStadiumAsync(request, currentUserId);

        if (result.IsFailure)
            return BadRequest(new
            {
                result.Error.Code,
                result.Error.Description
            });

        // السيرفيس حاليا لا ترجع قيمة للـ Id لذلك نقوم فقط بإرجاع رسالة نجاح
        return Ok(new { Message = "Stadium created successfully." });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStadiumRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _stadiumService.UpdateStadiumAsync(id, request, currentUserId!);

        if (result.IsFailure)
            return BadRequest(new
            {
                result.Error.Code,
                result.Error.Description
            });

        var updated = await _stadiumService.GetStadiumByIdAsync(id);
        return Ok(updated.Value);
    }

    [HttpPatch("{id:int}/toggle-active")]
    
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _stadiumService.ToggleStadiumActiveAsync(id);

        if (result.IsFailure)
            return BadRequest(new
            {
                result.Error.Code,
                result.Error.Description
            });

        var updated = await _stadiumService.GetStadiumByIdAsync(id);
        return Ok(updated.Value);
    }
}

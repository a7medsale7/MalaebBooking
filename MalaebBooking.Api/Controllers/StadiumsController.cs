// 6. StadiumsController.cs
using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StadiumsController(IStadiumService stadiumService) : ControllerBase
{
    private readonly IStadiumService _stadiumService = stadiumService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _stadiumService.GetAllStadiumsAsync();
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
    {
        var result = await _stadiumService.GetActiveStadiumsAsync();
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _stadiumService.GetStadiumByIdAsync(id);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("{id:int}/details")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDetails(int id)
    {
        var result = await _stadiumService.GetStadiumDetailsAsync(id);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("sport/{sportTypeId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySportType(int sportTypeId)
    {
        var result = await _stadiumService.GetStadiumsBySportTypeAsync(sportTypeId);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.Stadiums_Create)]
    public async Task<IActionResult> Create([FromBody] CreateStadiumRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null) return Unauthorized();

        var result = await _stadiumService.CreateStadiumAsync(request, currentUserId);
        return result.IsFailure ? BadRequest(result.Error) : Ok(new { Message = "Stadium created successfully." });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.Stadiums_Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStadiumRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _stadiumService.UpdateStadiumAsync(id, request, currentUserId!);
        if (result.IsFailure) return BadRequest(result.Error);

        var updated = await _stadiumService.GetStadiumByIdAsync(id);
        return Ok(updated.Value);
    }

    [HttpPatch("{id:int}/toggle-active")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Stadiums_ToggleActive)]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _stadiumService.ToggleStadiumActiveAsync(id);
        if (result.IsFailure) return BadRequest(result.Error);

        var updated = await _stadiumService.GetStadiumByIdAsync(id);
        return Ok(updated.Value);
    }
}

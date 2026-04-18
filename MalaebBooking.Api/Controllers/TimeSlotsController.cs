// 7. TimeSlotsController.cs
using MalaebBooking.Application.Contracts.TimeSlots;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TimeSlotsController(ITimeSlotService timeSlotService) : ControllerBase
{
    private readonly ITimeSlotService _timeSlotService = timeSlotService;

    [HttpGet("stadium/{stadiumId}")]
    [Authorize(Policy = Permissions.TimeSlots_View)]
    public async Task<IActionResult> GetAllByStadiumId(int stadiumId)
    {
        var result = await _timeSlotService.GetAllByStadiumAsync(stadiumId);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet("stadium/{stadiumId}/available")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailableByStadiumAndDate(int stadiumId, [FromQuery] DateOnly date)
    {
        var result = await _timeSlotService.GetAvailableByStadiumAndDateAsync(stadiumId, date);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _timeSlotService.GetByIdAsync(id);
        if (result.IsFailure) return result.Error.Code == "TimeSlot.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.TimeSlots_Create)]
    public async Task<IActionResult> Add([FromBody] CreateTimeSlotRequest request)
    {
        var result = await _timeSlotService.CreateTimeSlotAsync(request);
        return result.IsFailure ? BadRequest(result.Error) : CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.TimeSlots_Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTimeSlotRequest request)
    {
        var result = await _timeSlotService.UpdateTimeSlotAsync(id, request);
        if (result.IsFailure) return result.Error.Code == "TimeSlot.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.TimeSlots_Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _timeSlotService.DeleteTimeSlotAsync(id);
        if (result.IsFailure) return result.Error.Code == "TimeSlot.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return NoContent();
    }

    // ==========================================
    // Endpoint 1: For the frontend to get preview before saving
    // ==========================================
    [HttpPost("preview")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.TimeSlots_Create)]
    public async Task<IActionResult> GeneratePreview([FromBody] PreviewTimeSlotsRequest request)
    {
        var result = await _timeSlotService.GeneratePreviewAsync(request);

        if (result.IsFailure)
        {
            return result.Error.Code == "TimeSlot.NotFound" || result.Error.Code == "TimeSlot.StadiumNotFound"
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    // ==========================================
    // Endpoint 2: For the frontend to submit selected slots and save them in bulk
    // ==========================================
    [HttpPost("bulk-create")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.TimeSlots_Create)]
    public async Task<IActionResult> BulkCreate([FromBody] BulkCreateTimeSlotsRequest request)
    {
        var result = await _timeSlotService.BulkCreateAsync(request);

        if (result.IsFailure)
        {
            return result.Error.Code == "TimeSlot.NotFound" || result.Error.Code == "TimeSlot.StadiumNotFound"
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        }

        // Returns 200 OK if saved successfully
        return Ok(new { message = "Time slots saved successfully!" });
    }
}

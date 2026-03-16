// 1. BookingsController.cs
using MalaebBooking.Application.Contracts.Bookings;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpPost]
    [Authorize(Policy = Permissions.Bookings_Create)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _bookingService.CreateBookingAsync(request, userId);
        return result.IsFailure ? BadRequest(result.Error) : CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Bookings_View)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bookingService.GetBookingByIdAsync(id);
        if (result.IsFailure) return result.Error.Code == "Booking.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("my-bookings")]
    [Authorize(Policy = Permissions.Bookings_View)]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _bookingService.GetBookingsByUserAsync(userId);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet("stadium/{stadiumId}")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.Bookings_View)]
    public async Task<IActionResult> GetBookingsByStadium(int stadiumId)
    {
        var result = await _bookingService.GetBookingsByStadiumAsync(stadiumId);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Bookings_View)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingService.GetAllBookingsAsync();
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.Bookings_UpdateStatus)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingRequest request)
    {
        var result = await _bookingService.UpdateBookingStatusAsync(id, request);
        if (result.IsFailure) return result.Error.Code == "Booking.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = Permissions.Bookings_Cancel)]
    public async Task<IActionResult> CancelBooking(int id, [FromBody] string cancellationReason)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _bookingService.CancelBookingAsync(id, cancellationReason, userId);
        if (result.IsFailure)
        {
            if (result.Error.Code == "Booking.NotFound") return NotFound(result.Error);
            if (result.Error.Code == "Booking.Unauthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { Message = "تم إلغاء الحجز بنجاح." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.Bookings_Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookingService.DeleteBookingAsync(id);
        if (result.IsFailure) return result.Error.Code == "Booking.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return NoContent();
    }
}

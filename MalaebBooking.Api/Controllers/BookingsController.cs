using MalaebBooking.Application.Contracts.Bookings;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // كل الحجوزات تتطلب تسجيل دخول
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // ==================== Create ====================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        // سحب الـ User ID من التوكن الحالي
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _bookingService.CreateBookingAsync(request, userId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    // ==================== Read ====================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bookingService.GetBookingByIdAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Booking.NotFound")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    // استعراض حجوزات المستخدم الحالي بناءً على التوكن
    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _bookingService.GetBookingsByUserAsync(userId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    // استعراض حجوزات لملعب معين (مهمة للمالك/الأدمن)
    [HttpGet("stadium/{stadiumId}")]
    // [Authorize(Roles = "StadiumOwner, Admin")] // ممكن تفعلها مستقبلاً
    public async Task<IActionResult> GetBookingsByStadium(int stadiumId)
    {
        var result = await _bookingService.GetBookingsByStadiumAsync(stadiumId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    // مفيدة للـ Dashboard
    [HttpGet]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingService.GetAllBookingsAsync();

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    // ==================== Update & Cancel ====================
    [HttpPut("{id}/status")]
    // [Authorize(Roles = "StadiumOwner, Admin")] // تغيير الحالة بيبقى للملعب أو الإدارة غالطاً
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingRequest request)
    {
        var result = await _bookingService.UpdateBookingStatusAsync(id, request);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Booking.NotFound")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id, [FromBody] string cancellationReason)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _bookingService.CancelBookingAsync(id, cancellationReason, userId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Booking.NotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Booking.Unauthorized")
            {
                // ده بيرجع 403 (Forbidden) وجواه رسالة الخطأ زي ما هي متسجلة
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            }


            return BadRequest(result.Error);
        }

        return Ok(new { Message = "تم إلغاء الحجز بنجاح." });
    }

    // ==================== Delete ====================
    [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin")] // يستحسن الحذف النهائي يكون من صلاحيات الأدمن بس
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookingService.DeleteBookingAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Booking.NotFound")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return NoContent();
    }
}

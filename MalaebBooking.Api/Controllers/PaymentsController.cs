using MalaebBooking.Application.Contracts.Payments;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // ==================== GET PAYMENT INFO ====================
    // GET: api/payments/booking/{bookingId}
    // اللاعب يشوف رقم InstaPay والمبلغ المطلوب
    [HttpGet("booking/{bookingId}")]
    public async Task<IActionResult> GetPaymentInfo(int bookingId)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var result = await _paymentService.GetPaymentInfoAsync(bookingId, playerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.BookingNotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Payment.NotAuthorized")
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    // ==================== SUBMIT PAYMENT PROOF ====================
    // POST: api/payments/booking/{bookingId}/submit
    // اللاعب يرفع السكرين شوت بعد ما يحول على InstaPay
    [HttpPost("booking/{bookingId}/submit")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SubmitPaymentProof(
      int bookingId,
      IFormFile? screenshot,            // ✅ منفصلة
      [FromForm] string? playerPhoneNumber)   // ✅ منفصلة
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var request = new SubmitPaymentRequest
        {
            Screenshot = screenshot,
            PlayerPhoneNumber = playerPhoneNumber
        };

        var result = await _paymentService.SubmitPaymentProofAsync(bookingId, request, playerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.BookingNotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Payment.NotAuthorized")
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    // ==================== APPROVE PAYMENT ====================
    // PUT: api/payments/{id}/approve
    // صاحب الملعب يوافق على الإيصال → الحجز يبقى Confirmed
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApprovePayment(int id)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized();

        var result = await _paymentService.ApprovePaymentAsync(id, ownerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.NotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Payment.NotAuthorized")
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);

            return BadRequest(result.Error);
        }

        return Ok(new { Message = "تم قبول الدفع وتأكيد الحجز بنجاح." });
    }

    // ==================== REJECT PAYMENT ====================
    // PUT: api/payments/{id}/reject
    // صاحب الملعب يرفض الإيصال ويكتب سبب الرفض
    [HttpPut("{id}/reject")]
    public async Task<IActionResult> RejectPayment(int id, [FromBody] RejectPaymentRequest request)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized();

        var result = await _paymentService.RejectPaymentAsync(id, request, ownerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.NotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Payment.NotAuthorized")
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);

            return BadRequest(result.Error);
        }

        return Ok(new { Message = "تم رفض الإيصال. يمكن للاعب رفع إيصال جديد." });
    }
}

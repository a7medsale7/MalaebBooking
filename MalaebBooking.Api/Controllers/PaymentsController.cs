// 2. PaymentsController.cs
using MalaebBooking.Application.Contracts.Payments;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentsController(IPaymentService paymentService) : ControllerBase
{
    private readonly IPaymentService _paymentService = paymentService;

    [HttpGet("booking/{bookingId}")]
    [Authorize(Policy = Permissions.Payments_View)]
    public async Task<IActionResult> GetPaymentInfo(int bookingId)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var result = await _paymentService.GetPaymentInfoAsync(bookingId, playerId);
        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.BookingNotFound") return NotFound(result.Error);
            if (result.Error.Code == "Payment.NotAuthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPost("booking/{bookingId}/submit")]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = Permissions.Payments_SubmitProof)]
    public async Task<IActionResult> SubmitPaymentProof(int bookingId, IFormFile? screenshot, [FromForm] string? playerPhoneNumber)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var request = new SubmitPaymentRequest { Screenshot = screenshot, PlayerPhoneNumber = playerPhoneNumber };
        var result = await _paymentService.SubmitPaymentProofAsync(bookingId, request, playerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.BookingNotFound") return NotFound(result.Error);
            if (result.Error.Code == "Payment.NotAuthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.Payments_Approve)]
    public async Task<IActionResult> ApprovePayment(int id)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId)) return Unauthorized();

        var result = await _paymentService.ApprovePaymentAsync(id, ownerId);
        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.NotFound") return NotFound(result.Error);
            if (result.Error.Code == "Payment.NotAuthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { Message = "تم قبول الدفع وتأكيد الحجز بنجاح." });
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.Payments_Reject)]
    public async Task<IActionResult> RejectPayment(int id, [FromBody] RejectPaymentRequest request)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId)) return Unauthorized();

        var result = await _paymentService.RejectPaymentAsync(id, request, ownerId);
        if (result.IsFailure)
        {
            if (result.Error.Code == "Payment.NotFound") return NotFound(result.Error);
            if (result.Error.Code == "Payment.NotAuthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { Message = "تم رفض الإيصال. يمكن للاعب رفع إيصال جديد." });
    }
}

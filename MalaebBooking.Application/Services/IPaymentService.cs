using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Payments;
using Microsoft.AspNetCore.Http;

namespace MalaebBooking.Application.Services;

public interface IPaymentService
{
    // ================== GET PAYMENT INFO (اللاعب يشوف رقم InstaPay والمبلغ) ==================
    Task<Result<PaymentInfoResponse>> GetPaymentInfoAsync(int bookingId, string playerId);

    // ================== SUBMIT PAYMENT PROOF (اللاعب يرفع السكرين شوت) ==================
    Task<Result<PaymentResponse>> SubmitPaymentProofAsync(int bookingId, SubmitPaymentRequest request, string playerId);

    // ================== APPROVE PAYMENT (صاحب الملعب يوافق) ==================
    Task<Result> ApprovePaymentAsync(int paymentId, string ownerId);

    // ================== REJECT PAYMENT (صاحب الملعب يرفض) ==================
    Task<Result> RejectPaymentAsync(int paymentId, RejectPaymentRequest request, string ownerId);
}

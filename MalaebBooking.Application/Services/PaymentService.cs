using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Payments;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MalaebBooking.Application.Services;

public class PaymentService(
    IPaymentRepository paymentRepository,
    IBookingRepository bookingRepository,
    IWebHostEnvironment env,
    IEmailSender emailSender) : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IWebHostEnvironment _env = env;
    private readonly IEmailSender _emailSender = emailSender;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    // =========================================================================================
    // ================== GET PAYMENT INFO ==================
    // =========================================================================================
    public async Task<Result<PaymentInfoResponse>> GetPaymentInfoAsync(int bookingId, string playerId)
    {
        // 1. نجيب الحجز
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking is null)
            return Result.Failure<PaymentInfoResponse>(PaymentErrors.BookingNotFound);

        // 2. نتأكد إن اللاعب ده هو صاحب الحجز
        if (booking.PlayerId != playerId)
            return Result.Failure<PaymentInfoResponse>(PaymentErrors.NotAuthorized);

        // 3. نجيب معلومات الملعب عن طريق التايم سلوت
        var stadium = booking.TimeSlot.Stadium;

        // 4. نجيب الدفع الخاص بالحجز (لو موجود)
        var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);

        var response = new PaymentInfoResponse
        {
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            StadiumName = stadium.Name,
            InstapayNumber = stadium.InstapayNumber ?? "غير متاح",
            PaymentStatus = payment != null ? payment.Status.ToString() : null,
            RejectionReason = payment?.RejectionReason,
            ExpiresAt = booking.ExpiresAt
        };

        return Result.Success(response);
    }

    // =========================================================================================
    // ================== SUBMIT PAYMENT PROOF ==================
    // =========================================================================================
    public async Task<Result<PaymentResponse>> SubmitPaymentProofAsync(
        int bookingId,
        SubmitPaymentRequest request,
        string playerId)
    {
        // 1. نتأكد إن الحجز موجود وتبع اللاعب ده
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.BookingNotFound);

        if (booking.PlayerId != playerId)
            return Result.Failure<PaymentResponse>(PaymentErrors.NotAuthorized);

        // 2. نتأكد إن الحجز لسه Pending (مش منتهي أو متأكد قبل كدا)
        if (booking.Status != BookingStatus.Pending)
            return Result.Failure<PaymentResponse>(PaymentErrors.BookingNotPending);

        // 3. نتأكد إن اللاعب مرفعش إيصال قبل كدا لنفس الحجز
        var existingPayment = await _paymentRepository.GetByBookingIdAsync(bookingId);
        if (existingPayment is not null && existingPayment.Status != PaymentStatus.Rejected)
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentAlreadySubmitted);

        // 4. نتحقق من الملف
        var file = request.Screenshot;
        if (file is null || file.Length == 0)
            return Result.Failure<PaymentResponse>(PaymentErrors.EmptyFile);

        if (file.Length > MaxFileSizeBytes)
            return Result.Failure<PaymentResponse>(PaymentErrors.FileTooLarge);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return Result.Failure<PaymentResponse>(PaymentErrors.InvalidFileType);

        // 5. نحفظ الصورة على الـ Disk
        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "payments");
        Directory.CreateDirectory(uploadsFolder);
        var filePath = Path.Combine(uploadsFolder, fileName);

        try
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch
        {
            return Result.Failure<PaymentResponse>(PaymentErrors.UploadFailed);
        }

        // 6. نحفظ في قاعدة البيانات (أو نعدل لو كان مرفوض قبل كدا)
        if (existingPayment is not null)
        {
            // إعادة رفع بعد الرفض
            existingPayment.PaymentScreenshotUrl = $"/uploads/payments/{fileName}";
            existingPayment.PlayerPhoneNumber = request.PlayerPhoneNumber;
            existingPayment.Status = PaymentStatus.Uploaded;
            existingPayment.RejectionReason = null;
            existingPayment.RejectedAt = null;
            await _paymentRepository.UpdateAsync(existingPayment);
        }
        else
        {
            // أول مرة يرفع إيصال
            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalPrice,
                PaymentScreenshotUrl = $"/uploads/payments/{fileName}",
                PlayerPhoneNumber = request.PlayerPhoneNumber ?? string.Empty, // ✅ مش null
                Status = PaymentStatus.Uploaded
            };

            await _paymentRepository.AddAsync(payment);
        }

        return Result.Success(new PaymentResponse
        {
            BookingId = bookingId,
            Amount = booking.TotalPrice,
            Status = nameof(PaymentStatus.Uploaded),
            Message = "تم رفع الإيصال بنجاح، في انتظار مراجعة صاحب الملعب."
        });
    }

    // =========================================================================================
    // ================== APPROVE PAYMENT ==================
    // =========================================================================================
    public async Task<Result> ApprovePaymentAsync(int paymentId, string ownerId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);

        if (payment is null)
            return Result.Failure(PaymentErrors.NotFound);

        // نتأكد إن ده صاحب الملعب بتاع الحجز  
        var stadium = payment.Booking.TimeSlot.Stadium;
        if (stadium.OwnerId != ownerId)
            return Result.Failure(PaymentErrors.NotAuthorized);

        // نتأكد إن في إيصال مرفوع فعلاً
        if (payment.Status != PaymentStatus.Uploaded)
            return Result.Failure(PaymentErrors.NoProofUploaded);

        // 1. نحدث حالة الدفع
        payment.Status = PaymentStatus.Approved;
        payment.ApprovedAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(payment);

        // 2. نحدث حالة الحجز لـ Confirmed
        var booking = payment.Booking;
        booking.Status = BookingStatus.Confirmed;
        booking.ConfirmedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking);

        // 3. إرسال إيميل تأكيد في الخلفية
        if (booking.Player?.Email != null)
        {
            var stadiumName = booking.TimeSlot.Stadium.Name;
            var playerEmail = booking.Player.Email;
            var date = booking.TimeSlot.Date.ToString("yyyy-MM-dd");
            var time = $"{booking.TimeSlot.StartTime} - {booking.TimeSlot.EndTime}";
            
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                playerEmail,
                "✅ تأكيد حجزك في ملاعب",
                $"أهلاً بك،<br>تم تأكيد حجزك في ملعب <b>{stadiumName}</b>.<br>الموعد: {date} من {time}.<br>نتمنى لك وقتاً ممتعاً!"
            ));
        }

        return Result.Success();
    }

    // =========================================================================================
    // ================== REJECT PAYMENT ==================
    // =========================================================================================
    public async Task<Result> RejectPaymentAsync(int paymentId, RejectPaymentRequest request, string ownerId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);

        if (payment is null)
            return Result.Failure(PaymentErrors.NotFound);

        // نتأكد إن ده صاحب الملعب
        var stadium = payment.Booking.TimeSlot.Stadium;
        if (stadium.OwnerId != ownerId)
            return Result.Failure(PaymentErrors.NotAuthorized);

        if (payment.Status != PaymentStatus.Uploaded)
            return Result.Failure(PaymentErrors.NoProofUploaded);

        // نحدث حالة الدفع
        payment.Status = PaymentStatus.Rejected;
        payment.RejectedAt = DateTime.UtcNow;
        payment.RejectionReason = request.RejectionReason;
        await _paymentRepository.UpdateAsync(payment);

        // إرسال إيميل رفض في الخلفية
        var booking = payment.Booking;
        if (booking.Player?.Email != null)
        {
            var stadiumName = booking.TimeSlot.Stadium.Name;
            var playerEmail = booking.Player.Email;
            
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                playerEmail,
                "❌ تم رفض إيصال الدفع",
                $"أهلاً بك،<br>تم رفض إيصال الدفع الخاص بحجزك في ملعب <b>{stadiumName}</b>.<br><b>السبب:</b> {request.RejectionReason}<br>يرجى إعادة رفع إيصال صحيح."
            ));
        }

        // الحجز يفضل Pending عشان اللاعب يقدر يرفع إيصال تاني
        return Result.Success();
    }
}

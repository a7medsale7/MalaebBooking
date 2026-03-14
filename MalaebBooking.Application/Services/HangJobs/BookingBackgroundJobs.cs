using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MalaebBooking.Application.Services.HangJobs;

public class BookingBackgroundJobs
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IEmailSender _emailSender;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<BookingBackgroundJobs> _logger;

    public BookingBackgroundJobs(
        IBookingRepository bookingRepository,
        ITimeSlotRepository timeSlotRepository,
        IPaymentRepository paymentRepository,
        IEmailSender emailSender,
        IWebHostEnvironment env,
        ILogger<BookingBackgroundJobs> logger)
    {
        _bookingRepository = bookingRepository;
        _timeSlotRepository = timeSlotRepository;
        _paymentRepository = paymentRepository;
        _emailSender = emailSender;
        _env = env;
        _logger = logger;
    }

    // 1. تحديث الحجوزات اللي خلصت (يعمل كل ساعة)
    public async Task AutoCompleteBookingsAsync()
    {
        var bookingsToComplete = await _bookingRepository.GetConfirmedBookingsInPastAsync();

        foreach (var booking in bookingsToComplete)
        {
            booking.Status = BookingStatus.Completed;
            await _bookingRepository.UpdateAsync(booking);
            _logger.LogInformation($"Auto-completed booking ID {booking.Id}");
        }
    }

    // 2. إلغاء الحجوزات المعلقة اللي متدفعش ثمنها (يعمل كل 15 دقيقة)
    public async Task AutoCancelExpiredPendingsAsync()
    {
        var expiredBookings = await _bookingRepository.GetExpiredPendingBookingsAsync();

        foreach (var booking in expiredBookings)
        {
            // نكنسل الحجز
            booking.Status = BookingStatus.Cancelled;
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancellationReason = "انتهاء مهلة الدفع ولم يتم رفع الإيصال";
            await _bookingRepository.UpdateAsync(booking);

            // نخلي ميعاد الملعب Available تاني
            if (booking.TimeSlot != null)
            {
                booking.TimeSlot.Status = SlotStatus.Available;
                await _timeSlotRepository.UpdateAsync(booking.TimeSlot);
            }

            _logger.LogInformation($"Auto-cancelled expired pending booking ID {booking.Id}");
        }
    }

    // 3. إرسال تذكير للاعبين بميعاد الحجز (يعمل كل نص ساعة)
    public async Task SendBookingRemindersAsync()
    {
        // بنجيب الحجوزات اللي هتبدأ كمان ساعتين بالظبط من دلوقتي
        var upcomingBookings = await _bookingRepository.GetUpcomingBookingsAsync(TimeSpan.FromHours(2));

        foreach (var booking in upcomingBookings)
        {
            if (booking.Player?.Email != null && booking.TimeSlot?.Stadium != null)
            {
                var stadiumName = booking.TimeSlot.Stadium.Name;
                var time = booking.TimeSlot.StartTime.ToString();

                await _emailSender.SendEmailAsync(
                    booking.Player.Email,
                    "⏰ تذكير: ميعاد الماتش اقترب!",
                    $"أهلاً بك،<br>نذكرك بأن حجزك في ملعب <b>{stadiumName}</b> سيبدأ بعد ساعتين (في تمام الساعة {time}).<br>نتمنى لك وقتاً ممتعاً!"
                );
            }
        }
    }

    // 4. مسح ملفات الإيصالات القديمة (يعمل كل شهر)
    public async Task CleanUpOldPaymentsStorageAsync()
    {
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var oldPayments = await _paymentRepository.GetPaymentsOlderThanAsync(sixMonthsAgo);

        foreach (var payment in oldPayments)
        {
            if (!string.IsNullOrEmpty(payment.PaymentScreenshotUrl))
            {
                // مسح الملف من الهاردديسك!
                var fileName = Path.GetFileName(payment.PaymentScreenshotUrl);
                var filePath = Path.Combine(_env.WebRootPath, "uploads", "payments", fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // تفضية الـ URL من الداتا بيس
                payment.PaymentScreenshotUrl = null;
                await _paymentRepository.UpdateAsync(payment);
                _logger.LogInformation($"Deleted old payment image for Payment ID {payment.Id}");
            }
        }
    }
}

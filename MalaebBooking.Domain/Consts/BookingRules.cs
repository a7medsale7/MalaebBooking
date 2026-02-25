using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Consts;
public static class BookingRules
{
    // أقصى مدة بالدقايق عشان اليوزر يدفع قبل ما الحجز يتلغي
    public const int PaymentTimeoutMinutes = 30;
    // قواعد خاصة بمدة حجز الملعب بالدقايق
    public const int MinSlotDurationMinutes = 30;
    public const int MaxSlotDurationMinutes = 120;
    public const int DefaultSlotDurationMinutes = 60;
    // أقصى عدد أيام يقدر اليوزر يحجز فيها قدام
    public const int MaxAdvanceBookingDays = 14;
    // قواعد التقييم
    public const int MinRating = 1;
    public const int MaxRating = 5;
}
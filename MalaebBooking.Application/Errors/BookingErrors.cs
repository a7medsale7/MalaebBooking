using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class BookingErrors
{
    public static readonly Error NotFound =
        new("Booking.NotFound", "عذراً، هذا الحجز غير موجود بالنظام.");

    public static readonly Error Unauthorized =
        new("Booking.Unauthorized", "غير مصرح لك بإلغاء أو التعديل على هذا الحجز.");

    public static readonly Error SlotUnavailable =
        new("Booking.SlotUnavailable", "عذراً، هذا الميعاد تم حجزه أو أصبح غير متاحًا الآن.");

    public static readonly Error StadiumNotFound =
        new("Booking.StadiumNotFound", "بيانات الملعب المرتبط بهذا الوقت غير موجودة.");

    public static readonly Error AlreadyCancelled =
        new("Booking.AlreadyCancelled", "هذا الحجز تم إلغاءه بالفعل مسبقاً.");

    public static readonly Error AlreadyConfirmed =
        new("Booking.AlreadyConfirmed", "لا يمكن تغيير حالة هذا الحجز لأنه تم تأكيده بالفعل.");

    // تقدر تضيف أي إيرور جديد هنا مستقبلاً
}

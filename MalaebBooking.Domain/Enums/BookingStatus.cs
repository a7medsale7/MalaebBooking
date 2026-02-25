using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Enums;
public enum BookingStatus
{
    [Description("في انتظار الدفع")]
    Pending = 0,      // الحجز اتخلق ومستني الدفع (خلال 30 دقيقة مثلا)
    [Description("مؤكد")]
    Confirmed = 1,    // صاحب الملعب راجع الإيصال وأكد الحجز
    [Description("مرفوض")]
    Rejected = 2,     // صاحب الملعب رفض الحجز (مثلا الإيصال مزور)
    [Description("ملغي")]
    Cancelled = 3,    // اللاعب لغى الحجز بنفسه
    [Description("منتهي الصلاحية")]
    Expired = 4,      // عدى 30 دقيقة واللاعب مادفعش، فالحجز اتلغى تلقائي
    [Description("مكتمل")]
    Completed = 5     // ميعاد الحجز جه وخلص واللاعب لعب
}
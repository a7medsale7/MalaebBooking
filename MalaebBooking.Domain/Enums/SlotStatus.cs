using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Enums;
public enum SlotStatus
{
    [Description("متاح")]
    Available = 0,    // الوقت ده فاضي وممكن يتحجز
    [Description("في انتظار التأكيد")]
    Pending = 1,      // في حد داس حجز بس لسه رافعش إيصال الدفع (محجوز مؤقتاً)
    [Description("محجوز")]
    Booked = 2,       // تم الدفع والتأكيد
    [Description("ملغي")]
    Cancelled = 3     // الملعب قفله لأي سبب
}
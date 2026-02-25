using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Enums;
public enum PaymentStatus
{
    [Description("في انتظار الرفع")]
    Pending = 0,      // منتظرين سكرين شوت إنستاباي
    [Description("تم رفع الإثبات")]
    Uploaded = 1,     // اللاعب رفع السكرين شوت
    [Description("مقبول")]
    Approved = 2,     // صاحب الملعب وافق
    [Description("مرفوض")]
    Rejected = 3      // صاحب الملعب رفض الإيصال
}
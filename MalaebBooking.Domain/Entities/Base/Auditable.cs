using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities.Base;
public abstract class Auditable
{
    // Id الشخص اللي أنشأ الريكورد
    public string CreatedById { get; set; } = string.Empty;

    // تاريخ الإنشاء (بياخد وقت السيرفر الحالي كافتراضي)
    public DateTime CreatedOn { get; set; }

    // Id الشخص اللي قام بآخر تعديل (ممكن يكون null لو لسه متعدلش)
    public string? UpdatedById { get; set; }

    // تاريخ آخر تعديل
    public DateTime? UpdatedOn { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Enums;
public enum StadiumOwnerStatus
{
    Pending,   // قيد المراجعة
    Approved,  // مقبول
    Rejected   // مرفوض
}
using MalaebBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class TimeSlot
{
    public int Id { get; set; }

    // تاريخ اليوم اللي فيه الحجز
    public DateOnly Date { get; set; }

    // بداية ونهاية الـ Slot
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    // حالة الوقت (متاح - محجوز - الخ)
    public SlotStatus Status { get; set; } = SlotStatus.Available;
    // Foreign Keys
    public int StadiumId { get; set; }
    // Navigation Properties
    public Stadium Stadium { get; set; } = default!;

    // ربط الـ Slot بالحجز الخاص بيه (لو موجود)
    public Booking? Booking { get; set; }
}
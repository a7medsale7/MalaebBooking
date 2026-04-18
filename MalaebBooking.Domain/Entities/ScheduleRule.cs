using MalaebBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class ScheduleRule : Auditable
{
    public int Id { get; set; }

    public int StadiumId { get; set; }
    public Stadium Stadium { get; set; } = default!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? ExcludedHours { get; set; }
    public bool IsActive { get; set; } = true;
    // Navigation Property عشان نجيب المواعيد اللي اتولدت منها
    public ICollection<TimeSlot> GeneratedTimeSlots { get; set; } = [];
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.TimeSlots;
public class PreviewTimeSlotResponse
{
 
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Price { get; set; }

    public bool IsAlreadyExists { get; set; }
}

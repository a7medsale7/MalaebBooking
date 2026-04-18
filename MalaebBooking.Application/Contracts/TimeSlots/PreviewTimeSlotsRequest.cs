using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.TimeSlots;
public class PreviewTimeSlotsRequest
{
    public int StadiumId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly RangeStart { get; set; } // مثلا 08:00
   public TimeOnly RangeEnd { get; set; }  // مثلا 20:00 (8 بليل)
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.TimeSlots;
public class BulkCreateTimeSlotsRequest
{

    public int StadiumId { get; set; }
  public DateOnly Date { get; set; }
    public List<SlotRange> SelectedSlots { get; set; }// هيبعتلك المواعيد الصافية بس
   public bool IsRecurring { get; set; }
}

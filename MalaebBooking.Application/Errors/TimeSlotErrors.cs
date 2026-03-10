using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class TimeSlotErrors
{
    public static readonly Error NotFound =
        new("TimeSlot.NotFound", "The time slot was not found.");

    public static readonly Error Overlapping =
        new("TimeSlot.Overlapping", "There is an overlapping time slot for this stadium on the selected date and time.");

    public static readonly Error StartTimeAfterEndTime =
        new("TimeSlot.StartTimeAfterEndTime", "Start time must be before end time.");
        
    public static readonly Error StatusUpdateFailed =
        new("TimeSlot.StatusUpdateFailed", "Cannot update time slot status.");
}

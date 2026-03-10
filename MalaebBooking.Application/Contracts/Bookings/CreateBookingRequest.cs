using System;

namespace MalaebBooking.Application.Contracts.Bookings;

public class CreateBookingRequest
{
    // بنحتاج بس نعرف التايم سلوت اللي هينحجز
    public int TimeSlotId { get; set; }
}

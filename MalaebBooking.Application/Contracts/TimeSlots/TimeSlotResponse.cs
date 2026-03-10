namespace MalaebBooking.Application.Contracts.TimeSlots;

public class TimeSlotResponse
{
    public int Id { get; set; }

    public int StadiumId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = string.Empty;
}
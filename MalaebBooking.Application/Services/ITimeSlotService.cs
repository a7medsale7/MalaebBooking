using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.TimeSlots;

namespace MalaebBooking.Application.Services;

public interface ITimeSlotService
{
    // ================== CREATE ==================
    Task<Result<TimeSlotResponse>> CreateTimeSlotAsync(CreateTimeSlotRequest request);

    // ================== UPDATE ==================
    Task<Result<TimeSlotResponse>> UpdateTimeSlotAsync(int id, UpdateTimeSlotRequest request);

    // ================== DELETE ==================
    Task<Result> DeleteTimeSlotAsync(int id);

    // ================== GET ==================
    Task<Result<TimeSlotResponse>> GetByIdAsync(int id);

    Task<Result<IEnumerable<TimeSlotResponse>>> GetAllByStadiumAsync(int stadiumId);

    Task<Result<IEnumerable<TimeSlotResponse>>> GetByStadiumAndDateAsync(int stadiumId, DateOnly date);

    Task<Result<IEnumerable<TimeSlotResponse>>> GetAvailableByStadiumAndDateAsync(int stadiumId, DateOnly date);

    // ================== BUSINESS CHECKS ==================
    Task<Result<bool>> HasOverlappingSlotsAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime);

    Task<Result<bool>> IsSlotAvailableAsync(int slotId);
}

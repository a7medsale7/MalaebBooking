using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface ITimeSlotRepository
{
    Task<TimeSlot?> GetByIdAsync(int id);

    Task<IEnumerable<TimeSlot>> GetAllByStadiumIdAsync(int stadiumId);

    Task<IEnumerable<TimeSlot>> GetByStadiumAndDateAsync(
        int stadiumId,
        DateOnly date);

    Task<IEnumerable<TimeSlot>> GetAvailableByStadiumAndDateAsync(
        int stadiumId,
        DateOnly date);

    Task<bool> HasOverlappingSlotsAsync(
        int stadiumId,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime);

    Task<bool> IsSlotAvailableAsync(int slotId);

    Task<bool> ExistsAsync(int slotId);

    Task AddAsync(TimeSlot timeSlot);

    Task UpdateAsync(TimeSlot timeSlot);

    Task DeleteAsync(TimeSlot timeSlot);
}

using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface IScheduleRuleRepository
{
    Task AddAsync(ScheduleRule scheduleRule);
    Task UpdateAsync(ScheduleRule scheduleRule);
    Task DeleteAsync(ScheduleRule scheduleRule);
    Task<ScheduleRule?> GetByIdAsync(int id);
    Task<ScheduleRule?> GetByStadiumIdAsync(int stadiumId);
}
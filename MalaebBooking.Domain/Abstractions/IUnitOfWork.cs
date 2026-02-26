using MalaebBooking.Domain.Abstractions.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    ISportTypeRepository SportTypes { get; }
    // لو عندك Repository تانية، تضيفها هنا
    // ISportScheduleRepository SportSchedules { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
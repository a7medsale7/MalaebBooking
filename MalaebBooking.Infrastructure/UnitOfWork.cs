using MalaebBooking.Domain.Abstractions;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Infrastructure.Persistence;
using MalaebBooking.Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        SportTypes = new SportTypeRepository(_context);
        // لو عندك Repository تانية:
        // SportSchedules = new SportScheduleRepository(_context);
    }

    public ISportTypeRepository SportTypes { get; }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
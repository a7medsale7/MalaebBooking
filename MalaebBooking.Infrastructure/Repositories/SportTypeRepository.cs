using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;
public class SportTypeRepository(ApplicationDbContext context) : ISportTypeRepository
{

    private readonly ApplicationDbContext _context = context;


public async Task<IEnumerable<SportType>> GetAllAsync(CancellationToken cancellationToken)
{
    return await _context.SportTypes
        .Where(x => x.IsActive)
        .ToListAsync(cancellationToken);
}

public async Task<SportType?> GetByIdAsync(int id, CancellationToken cancellationToken)
{
    return await _context.SportTypes
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}

public async Task AddAsync(SportType sportType, CancellationToken cancellationToken)
{
    await _context.SportTypes.AddAsync(sportType, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
}

public async Task UpdateAsync(SportType sportType, CancellationToken cancellationToken)
{
    _context.SportTypes.Update(sportType);
    await _context.SaveChangesAsync(cancellationToken);
}

public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken)
{
    var entity = await _context.SportTypes
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (entity is null)
        throw new Exception("SportType not found");

    entity.IsActive = false;

    await _context.SaveChangesAsync(cancellationToken);
}

}

using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;
public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    // ================== CREATE ==================
    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
    // ================== UPDATE ==================
    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
    // ================== GET BY ID ==================
    // Include Booking عشان نوصل للـ Stadium ومنه لـ InstapayNumber
    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.TimeSlot)
                    .ThenInclude(t => t.Stadium)
                        .ThenInclude(s => s.OwnerProfile)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Player)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    // ================== GET BY BOOKING ID ==================
    public async Task<Payment?> GetByBookingIdAsync(int bookingId)
    {
        return await _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.TimeSlot)
                    .ThenInclude(t => t.Stadium)
                        .ThenInclude(s => s.OwnerProfile)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Player)
            .FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }
    // ================== CHECK IF EXISTS ==================
    public async Task<bool> ExistsByBookingIdAsync(int bookingId)
    {
        return await _context.Payments
            .AnyAsync(p => p.BookingId == bookingId);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsOlderThanAsync(DateTime date)
    {
        // هنجيب المدفوعات اللي تمت الموافقة عليها أو اترفضت من زمان
        return await _context.Payments
            .Where(p => (p.Status == PaymentStatus.Approved && p.ApprovedAt <= date) ||
                        (p.Status == PaymentStatus.Rejected && p.RejectedAt <= date))
            .ToListAsync();
    }

    public async Task<(List<Payment> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchValue,
        string? sortColumn,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.Player)
            .Include(p => p.Booking)
                .ThenInclude(b => b.TimeSlot)
                    .ThenInclude(t => t.Stadium)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(p => p.Booking.Player.FirstName.Contains(searchValue) || 
                                     p.Booking.Player.LastName.Contains(searchValue) ||
                                     p.Booking.TimeSlot.Stadium.Name.Contains(searchValue));
        }

        query = sortColumn?.ToLower() switch
        {
            "date" => sortDirection == "desc" ? query.OrderByDescending(p => p.CreatedOn) : query.OrderBy(p => p.CreatedOn),
            "status" => sortDirection == "desc" ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
            _ => query.OrderByDescending(p => p.Id)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface IPaymentRepository
{
    // ================== CREATE ==================
    Task AddAsync(Payment payment);
    // ================== UPDATE ==================
    Task UpdateAsync(Payment payment);
    // ================== GET BY ID ==================
    Task<Payment?> GetByIdAsync(int id);
    // ================== GET BY BOOKING ID ==================
    Task<Payment?> GetByBookingIdAsync(int bookingId);
    // ================== CHECK IF EXISTS ==================
    Task<bool> ExistsByBookingIdAsync(int bookingId);

    Task<IEnumerable<Payment>> GetPaymentsOlderThanAsync(DateTime date);
}
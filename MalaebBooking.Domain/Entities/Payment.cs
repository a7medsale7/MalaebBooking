using MalaebBooking.Domain.Entities.Base;
using MalaebBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class Payment : Auditable
{
    public int Id { get; set; }
    public decimal Amount { get; set; }

    // رابط الصورة بتاعة إيصال إنستاباي
    public string PaymentScreenshotUrl { get; set; } = string.Empty;

    // رقم التليفون اللي حول منه الفلوس (عشان يسهل المراجعة لصاحب الملعب)
    public string PlayerPhoneNumber { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? RejectionReason { get; set; }
    // Foreign Keys
    public int BookingId { get; set; }

    // Navigation
    public Booking Booking { get; set; } = default!;
}
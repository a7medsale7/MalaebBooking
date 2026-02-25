using MalaebBooking.Domain.Entities.Base;
using MalaebBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Entities;
public class Booking : Auditable
{
    public int Id { get; set; }

    // حالة الحجز بتبدأ بـ منُتظر הדفع
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public decimal TotalPrice { get; set; }

    // تواريخ مهمة لتتبع عمر الحجز
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }

    // هيتلغي إمتى تلقائياً لو مادفعش (مثلاً بعد 30 دقيقة من الإنشاء)
    public DateTime ExpiresAt { get; set; }
    // Foreign Keys (اللاعب والوقت המחجوز)
    public string PlayerId { get; set; } = string.Empty;
    public int TimeSlotId { get; set; }
    // Navigation Properties (موجودة وبتشاور لليوزر عشان اخترنا Option A)
    public ApplicationUser Player { get; set; } = default!;
    public TimeSlot TimeSlot { get; set; } = default!;
    public Payment? Payment { get; set; }
}
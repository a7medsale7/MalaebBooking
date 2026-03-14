using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Payments;
public class PaymentInfoResponse
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }             // المبلغ المطلوب
    public string StadiumName { get; set; }         // اسم الملعب
    public string InstapayNumber { get; set; }      // رقم InstaPay للتحويل
    public string PaymentStatus { get; set; }       // Pending / Uploaded / Approved
    public DateTime ExpiresAt { get; set; }         // المهلة قبل ما الحجز ينتهي
    public string? RejectionReason { get; set; }    // سبب الرفض لو اترفض
}

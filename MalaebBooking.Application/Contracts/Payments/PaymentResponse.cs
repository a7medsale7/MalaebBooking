using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Payments;
public class PaymentResponse
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentScreenshotUrl { get; set; }
    public string PlayerPhoneNumber { get; set; }
    public string Status { get; set; }              // Pending / Uploaded / Approved / Rejected
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? RejectionReason { get; set; }
    public string? Message { get; set; }  // ✅ مضافة

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Payments;
public class RejectPaymentRequest
{
    public string? RejectionReason { get; set; }     // سبب الرفض
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Payments;
public class SubmitPaymentRequest
{
    public IFormFile? Screenshot { get; set; }       // صورة الإيصال
    public string? PlayerPhoneNumber { get; set; }   // الرقم اللي حول منه
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Users;
public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}
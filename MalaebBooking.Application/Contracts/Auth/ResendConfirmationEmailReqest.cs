using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Auth;
public class ResendConfirmationEmailReqest
{
    public string Email { get; set; } = null!;

}

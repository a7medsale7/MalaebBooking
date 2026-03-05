using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Authentication;
public interface IJwtProvider
{
    (string Token, DateTime Expiration) GenerateToken(ApplicationUser user);

    string? ValidateToken(string token);    


}

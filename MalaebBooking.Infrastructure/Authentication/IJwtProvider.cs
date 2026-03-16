using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Authentication;
public interface IJwtProvider
{
    (string Token, DateTime Expiration) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);

    string? ValidateToken(string token);    


}

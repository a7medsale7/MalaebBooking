using MalaebBooking.Application.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;
public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(string email, string password,CancellationToken cancellationToken);



}

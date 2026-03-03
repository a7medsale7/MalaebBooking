using MalaebBooking.Application.Contracts.Auth;
using MalaebBooking.Application.Services;
using MalaebBooking.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MalaebBooking.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var authRequest = await authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        if (authRequest is null)
            return Unauthorized();

        return Ok(authRequest);
    }
}
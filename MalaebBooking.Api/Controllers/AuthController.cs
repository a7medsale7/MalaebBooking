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

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResponse = await authService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        if (authResponse is null)
            return Unauthorized();
        return Ok(authResponse);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RevokeRefreshTokenAsync(
            request.Token,
            request.RefreshToken,
            cancellationToken);

        if (!result)
            return BadRequest(new
            {
                message = "Invalid token or refresh token."
            });

        return Ok(new
        {
            message = "Token revoked successfully."
        });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(
     RegisterRequest request,
     CancellationToken cancellationToken)
    {
        await authService.RegisterAsync(request, cancellationToken);

        return Ok(new
        {
            message = "Registration successful. Please check your email to confirm your account."
        });
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailReqest request, CancellationToken cancellationToken)
    {
        await authService.ConfirmEmailAsync(request, cancellationToken);
        return Ok(new
        {
            message = "Email confirmed successfully. You can now log in."
        });
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail(ResendConfirmationEmailReqest request, CancellationToken cancellationToken)
    {
        await authService.ResendConfirmationEmailAsync(request, cancellationToken);
        return Ok(new
        {
            message = "If an account with that email exists, a confirmation email has been resent."
        });
    }
}
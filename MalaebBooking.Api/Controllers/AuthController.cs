using MalaebBooking.Application.Contracts.Auth;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MalaebBooking.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.GetTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
            return Unauthorized(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(
            request.Token,
            request.RefreshToken,
            cancellationToken);

        if (result.IsFailure)
            return Unauthorized(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(result.Value);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RevokeRefreshTokenAsync(
            request.Token,
            request.RefreshToken,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(new
            {
                result.Error.Code,
                result.Error.Description
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
        var result = await authService.RegisterAsync(
            request,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(new
        {
            message = "Registration successful. Please check your email to confirm your account."
        });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] ConfirmEmailReqest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.ConfirmEmailAsync(
            request,
            cancellationToken);

        string title = result.IsSuccess ? "Email Confirmed!" : "Confirmation Failed";
        string message = result.IsSuccess 
            ? "Your email has been successfully verified. You can now close this window and log in to the app." 
            : $"Verification failed: {result.Error.Description}";
        string icon = result.IsSuccess ? "✅" : "❌";
        string color = result.IsSuccess ? "#22c55e" : "#ef4444";

        var html = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>{title}</title>
            <link href='https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap' rel='stylesheet'>
            <style>
                body {{ font-family: 'Inter', sans-serif; background-color: #f8fafc; display: flex; align-items: center; justify-content: center; height: 100vh; margin: 0; }}
                .card {{ background: white; padding: 2.5rem; border-radius: 1rem; box-shadow: 0 10px 25px rgba(0,0,0,0.05); text-align: center; max-width: 400px; width: 90%; }}
                .icon {{ font-size: 4rem; margin-bottom: 1rem; }}
                h1 {{ color: #1e293b; margin-bottom: 0.5rem; font-size: 1.5rem; }}
                p {{ color: #64748b; line-height: 1.6; margin-bottom: 2rem; }}
                .btn {{ background-color: {color}; color: white; padding: 0.75rem 1.5rem; border-radius: 0.5rem; text-decoration: none; font-weight: 600; transition: opacity 0.2s; display: inline-block; }}
                .btn:hover {{ opacity: 0.9; }}
            </style>
        </head>
        <body>
            <div class='card'>
                <div class='icon'>{icon}</div>
                <h1>{title}</h1>
                <p>{message}</p>
                <a href='https://malaeb-booking.runasp.net/' class='btn'>Go to Website</a>
            </div>
        </body>
        </html>";

        return Content(html, "text/html");
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail(
        ResendConfirmationEmailReqest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.ResendConfirmationEmailAsync(
            request,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(new
            {
                result.Error.Code,
                result.Error.Description
            });

        return Ok(new
        {
            message = "If an account with that email exists, a confirmation email has been resent."
        });
    }
}
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

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        ConfirmEmailReqest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.ConfirmEmailAsync(
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
            message = "Email confirmed successfully. You can now log in."
        });
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
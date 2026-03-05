using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Auth;

namespace MalaebBooking.Application.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken);

    Task<Result> RegisterAsync(
        RegisterRequest registerRequest,
        CancellationToken cancellationToken);

    Task<Result> ConfirmEmailAsync(
        ConfirmEmailReqest request,
        CancellationToken cancellationToken);

    Task<Result> ResendConfirmationEmailAsync(
        ResendConfirmationEmailReqest resendConfirmation,
        CancellationToken cancellationToken);

    Task<Result<AuthResponse>> RefreshTokenAsync(
        string token,
        string refreshToken,
        CancellationToken cancellationToken);

    Task<Result> RevokeRefreshTokenAsync(
        string token,
        string refreshToken,
        CancellationToken cancellationToken);
}
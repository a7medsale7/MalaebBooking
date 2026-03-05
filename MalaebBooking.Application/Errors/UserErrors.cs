using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email or password.");

    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email is not confirmed.");

    public static readonly Error EmailAlreadyExists =
        new("User.EmailAlreadyExists", "Email already exists.");

    public static readonly Error UserNotFound =
        new("User.NotFound", "User not found.");

    public static readonly Error EmailAlreadyConfirmed =
        new("User.EmailAlreadyConfirmed", "Email already confirmed.");

    public static readonly Error InvalidToken =
        new("User.InvalidToken", "Invalid token.");

    public static readonly Error InvalidTokenFormat =
        new("User.InvalidTokenFormat", "Invalid token format.");

    public static readonly Error RefreshTokenNotFound =
        new("User.RefreshTokenNotFound", "Refresh token is invalid or expired.");

    public static readonly Error EmailConfirmationFailed =
        new("User.EmailConfirmationFailed", "Email confirmation failed.");
}
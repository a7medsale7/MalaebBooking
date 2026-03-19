using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email or password.");

    public static readonly Error DisabledUser =
       new("User.Disabled", "Disabled  User contact your Admin ");

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


  

    public static readonly Error InvalidPassword =
        new("User.InvalidPassword", "كلمة المرور الحالية غير صحيحة");
    public static readonly Error PasswordChangeFailed =
        new("User.PasswordChangeFailed", "حدث خطأ أثناء تغيير كلمة المرور");


    public static readonly Error NotFound =
       new("Role.NotFound", "Role was not found.");

    public static readonly Error DuplicatedRole =
        new("Role.Duplicated", "Role already exists.");

    public static readonly Error InvalidPermissions =
        new("Role.InvalidPermissions", "One or more permissions are invalid.");

    public static readonly Error CreationFailed =
        new("Role.CreationFailed", "Failed to create role.");

    public static readonly Error UpdateFailed =
        new("Role.UpdateFailed", "Failed to update role.");

    public static readonly Error DeleteFailed =
        new("Role.DeleteFailed", "Failed to delete role.");
    public static readonly Error InvalidRoles =
    new("User.InvalidRoles", "One or more roles are invalid.");

   


}
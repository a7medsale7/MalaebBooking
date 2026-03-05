using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class SportTypeErrors
{
    public static readonly Error NotFound =
        new("SportType.NotFound", "Sport type was not found.");

    public static readonly Error AlreadyDeleted =
        new("SportType.AlreadyDeleted", "Sport type is already deleted.");

    public static readonly Error InvalidId =
        new("SportType.InvalidId", "Invalid sport type id.");

    public static readonly Error AlreadyExists =
        new("SportType.AlreadyExists", "Sport type already exists."); // ✅ الجديد
}
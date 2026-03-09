// MalaebBooking.Application/Errors/SportTypeErrors.cs

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
        new("SportType.AlreadyExists", "Sport type already exists.");

    // ✅ جديد للصور
    public static readonly Error InvalidFile =
        new("SportType.InvalidFile", "Invalid file. Only images (jpg, jpeg, png, webp) are allowed.");

    public static readonly Error FileTooLarge =
        new("SportType.FileTooLarge", "File size exceeds 5MB.");

    public static readonly Error EmptyFile =
        new("SportType.EmptyFile", "File cannot be empty.");

    public static readonly Error UploadFailed =
        new("SportType.UploadFailed", "Failed to save the icon file.");
}

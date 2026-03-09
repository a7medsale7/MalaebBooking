// MalaebBooking.Application/Errors/StadiumImageErrors.cs

using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class StadiumImageErrors
{
    public static readonly Error NotFound =
        new("StadiumImage.NotFound", "Image not found.");

    public static readonly Error InvalidFile =
        new("StadiumImage.InvalidFile", "Invalid file. Only images (jpg, jpeg, png, webp) are allowed.");

    public static readonly Error FileTooLarge =
        new("StadiumImage.FileTooLarge", "File size exceeds 5MB.");

    public static readonly Error EmptyFile =
        new("StadiumImage.EmptyFile", "File cannot be empty.");

    public static readonly Error StadiumNotFound =
        new("StadiumImage.StadiumNotFound", "Stadium not found.");

    public static readonly Error NotAuthorized =
        new("StadiumImage.NotAuthorized", "You are not authorized to manage this stadium's images.");

    public static readonly Error UploadFailed =
        new("StadiumImage.UploadFailed", "Failed to save the image file.");
}

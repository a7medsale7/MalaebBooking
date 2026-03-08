using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class StadiumErrors
{
    public static readonly Error NotFound =
        new("Stadium.NotFound", "No stadiums found.");

    public static readonly Error InvalidId =
        new("Stadium.InvalidId", "Invalid stadium id.");

    public static readonly Error InvalidData =
        new("Stadium.InvalidData", "Invalid stadium data.");

    // إضافات للفالديشن
    public static readonly Error NameRequired =
        new("Stadium.NameRequired", "Stadium name is required.");

    public static readonly Error NameTooLong =
        new("Stadium.NameTooLong", "Stadium name is too long.");

    public static readonly Error AddressRequired =
        new("Stadium.AddressRequired", "Stadium address is required.");

    public static readonly Error PriceInvalid =
        new("Stadium.PriceInvalid", "Price per hour must be greater than zero.");

    public static readonly Error TimeInvalid =
        new("Stadium.TimeInvalid", "Opening time must be before closing time.");

    public static readonly Error SlotDurationInvalid =
        new("Stadium.SlotDurationInvalid", "Slot duration must be greater than zero.");

    public static readonly Error SportTypeIdInvalid =
        new("Stadium.SportTypeIdInvalid", "Invalid sport type id.");

    public static readonly Error ImageUrlInvalid =
        new("Stadium.ImageUrlInvalid", "One or more image URLs are invalid.");

    public static readonly Error PhoneNumberInvalid =
        new("Stadium.PhoneNumberInvalid", "Phone number is invalid.");

    public static readonly Error InstaPayNumberInvalid =
        new("Stadium.InstaPayNumberInvalid", "Instapay number is invalid.");

    public static readonly Error GoogleMapsUrlInvalid =
        new("Stadium.GoogleMapsUrlInvalid", "Google Maps URL is invalid.");
    public static readonly Error NotAuthorized =
        new("SportType.NotAuthorized", "You are not authorized to perform this action.");
}
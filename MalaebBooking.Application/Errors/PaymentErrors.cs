using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class PaymentErrors
{
    public static readonly Error NotFound = new("Payment.NotFound", "Payment not found.");
    public static readonly Error BookingNotFound = new("Payment.BookingNotFound", "Booking not found.");
    public static readonly Error NotAuthorized = new("Payment.NotAuthorized", "You are not authorized.");
    public static readonly Error BookingNotPending = new("Payment.BookingNotPending", "Booking is not in pending status.");
    public static readonly Error PaymentAlreadySubmitted = new("Payment.AlreadySubmitted", "Payment proof already submitted.");
    public static readonly Error NoProofUploaded = new("Payment.NoProofUploaded", "No payment proof has been uploaded yet.");
    public static readonly Error EmptyFile = new("Payment.EmptyFile", "Screenshot file is empty.");
    public static readonly Error FileTooLarge = new("Payment.FileTooLarge", "File size exceeds 5MB.");
    public static readonly Error InvalidFileType = new("Payment.InvalidFileType", "Only JPG, PNG, WEBP files are allowed.");
    public static readonly Error UploadFailed = new("Payment.UploadFailed", "Failed to upload the screenshot.");
}

using MalaebBooking.Application.Abstractions.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Errors;
public static class StadiumOwnerProfileErrors
{
    public static readonly Error AlreadySubmitted =
        new("StadiumOwnerProfile.AlreadySubmitted", "You have already submitted a verification request which is under review.");

    public static readonly Error ProfileNotFound =
        new("StadiumOwnerProfile.NotFound", "Verification profile was not found.");

    public static readonly Error NotPending =
        new("StadiumOwnerProfile.NotPending", "This request has already been processed.");

    public static readonly Error NationalIdRequired =
        new("StadiumOwnerProfile.NationalIdRequired", "National ID is required.");

    public static readonly Error DocumentsRequired =
        new("StadiumOwnerProfile.DocumentsRequired", "You must upload both front and back images of the national ID.");
}
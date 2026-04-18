using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.StadiumOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;

public interface IStadiumOwnerService
{
    Task<Result> ApplyAsync(ApplyForStadiumOwnerRequest request, string userId);
    Task<Result<StadiumOwnerProfileResponse>> GetProfileByUserIdAsync(string userId);
    Task<Result<IEnumerable<StadiumOwnerProfileResponse>>> GetPendingProfilesAsync();
    Task<Result> ReviewProfileAsync(int profileId, string adminId, ReviewStadiumOwnerRequest request);
}
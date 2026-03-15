using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;
public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result<UserProfileResponse>> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request, string originUrl);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
}
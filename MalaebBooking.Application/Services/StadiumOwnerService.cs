using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.StadiumOwner;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Consts;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;
public class StadiumOwnerService(
    IStadiumOwnerProfileRepository ownerRepository,
    UserManager<ApplicationUser> userManager,
    IWebHostEnvironment env) : IStadiumOwnerService
{
    private readonly IStadiumOwnerProfileRepository _ownerRepository = ownerRepository;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IWebHostEnvironment _env = env;
    public async Task<Result> ApplyAsync(ApplyForStadiumOwnerRequest request, string userId)
    {
        // Check for existing profile
        var existingProfile = await _ownerRepository.GetByUserIdAsync(userId);
        if (existingProfile != null && existingProfile.Status != StadiumOwnerStatus.Rejected)
            return Result.Failure(StadiumOwnerProfileErrors.AlreadySubmitted);
        // Save Files
        var frontImagePath = await SaveFileAsync(request.NationalIdImageFront, "identity-docs");
        var backImagePath = await SaveFileAsync(request.NationalIdImageBack, "identity-docs");

        string? contractPath = null;
        if (request.OwnershipContractImage != null)
            contractPath = await SaveFileAsync(request.OwnershipContractImage, "ownership-docs");
        // Create Profile
        var profile = new StadiumOwnerProfile
        {
            UserId = userId,
            NationalId = request.NationalId,
            NationalIdImageFront = frontImagePath,
            NationalIdImageBack = backImagePath,
            OwnershipContractImage = contractPath,
            CommercialRegisterNumber = request.CommercialRegisterNumber,
            Status = StadiumOwnerStatus.Pending
        };
        await _ownerRepository.AddAsync(profile);
        return Result.Success();
    }
    public async Task<Result> ReviewProfileAsync(int profileId, string adminId, ReviewStadiumOwnerRequest request)
    {
        var profile = await _ownerRepository.GetByIdAsync(profileId);
        if (profile == null)
            return Result.Failure(StadiumOwnerProfileErrors.ProfileNotFound);

        if (profile.Status != StadiumOwnerStatus.Pending)
            return Result.Failure(StadiumOwnerProfileErrors.NotPending);
        if (request.Approve)
        {
            profile.Status = StadiumOwnerStatus.Approved;
            profile.VerifiedAt = DateTime.UtcNow;
            profile.ApprovedById = adminId;
            // Grant "Owner" Role
            var user = await _userManager.FindByIdAsync(profile.UserId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, DefaultRoles.Owner);
            }
        }
        else
        {
            profile.Status = StadiumOwnerStatus.Rejected;
            profile.AdminRemarks = request.Remarks;
        }
        await _ownerRepository.UpdateAsync(profile);
        return Result.Success();
    }
    public async Task<Result<StadiumOwnerProfileResponse>> GetProfileByUserIdAsync(string userId)
    {
        var profile = await _ownerRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return Result.Failure<StadiumOwnerProfileResponse>(StadiumOwnerProfileErrors.ProfileNotFound);
        var response = profile.Adapt<StadiumOwnerProfileResponse>();
        response.FullName = $"{profile.User.FirstName} {profile.User.LastName}";
        response.Status = profile.Status.ToString();
        return Result.Success(response);
    }
    public async Task<Result<IEnumerable<StadiumOwnerProfileResponse>>> GetPendingProfilesAsync()
    {
        var profiles = await _ownerRepository.GetPendingProfilesAsync();

        var response = profiles.Select(p => {
            var res = p.Adapt<StadiumOwnerProfileResponse>();
            res.FullName = $"{p.User.FirstName} {p.User.LastName}";
            res.Status = p.Status.ToString();
            return res;
        });
        return Result.Success(response);
    }
    private async Task<string> SaveFileAsync(IFormFile file, string folderName)
    {
        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", folderName);
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return $"/uploads/{folderName}/{fileName}";
    }
}
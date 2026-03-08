using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Reviews;
using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Contracts.TimeSlots;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace MalaebBooking.Application.Services;

public class StadiumService(IStadiumRepository stadiumRepository, UserManager<ApplicationUser> userManager) : IStadiumService
{
    private readonly IStadiumRepository stadiumRepository = stadiumRepository;
    private readonly UserManager<ApplicationUser> userManager = userManager;

    // ================== CREATE STADIUM ==================
    public async Task<Result> CreateStadiumAsync(
        CreateStadiumRequest request,
        string currentUserId, // جديد: نمرر ID صاحب الستاد
        CancellationToken cancellationToken = default)
    {
        if (request is null)
            return Result.Failure(StadiumErrors.InvalidData);

        // Basic Validations
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure(StadiumErrors.NameRequired);
        if (request.Name.Length > 200)
            return Result.Failure(StadiumErrors.NameTooLong);
        if (string.IsNullOrWhiteSpace(request.Address))
            return Result.Failure(StadiumErrors.AddressRequired);
        if (request.PricePerHour <= 0)
            return Result.Failure(StadiumErrors.PriceInvalid);
        if (request.SlotDurationMinutes <= 0)
            return Result.Failure(StadiumErrors.SlotDurationInvalid);
        if (request.ClosingTime <= request.OpeningTime)
            return Result.Failure(StadiumErrors.TimeInvalid);
        if (request.SportTypeId <= 0)
            return Result.Failure(StadiumErrors.SportTypeIdInvalid);

        // إنشاء الستاد وربطه بالمالك
        var stadium = request.Adapt<Stadium>();
        stadium.OwnerId = currentUserId;

        await stadiumRepository.AddAsync(stadium, cancellationToken);
        return Result.Success();
    }

    // ================== GET ACTIVE STADIUMS ==================
    public async Task<Result<List<StadiumResponse>>> GetActiveStadiumsAsync(
        CancellationToken cancellationToken = default)
    {
        var stadiums = await stadiumRepository.GetActiveAsync(cancellationToken);

        if (stadiums is null || !stadiums.Any())
            return Result.Failure<List<StadiumResponse>>(StadiumErrors.NotFound);

        var stadiumsResponse = stadiums.Adapt<List<StadiumResponse>>();
        foreach (var st in stadiumsResponse)
        {
            var owner = await userManager.FindByIdAsync(st.OwnerId);
            st.OwnerName = owner?.UserName ?? string.Empty;
        }

        return Result.Success(stadiumsResponse);
    }

    // ================== GET ALL STADIUMS ==================
    public async Task<Result<List<StadiumResponse>>> GetAllStadiumsAsync(
        CancellationToken cancellationToken = default)
    {
        var stadiums = await stadiumRepository.GetAllAsync(cancellationToken);

        if (stadiums is null || !stadiums.Any())
            return Result.Failure<List<StadiumResponse>>(StadiumErrors.NotFound);

        var stadiumsResponse = stadiums.Adapt<List<StadiumResponse>>();
        foreach (var st in stadiumsResponse)
        {
            var owner = await userManager.FindByIdAsync(st.OwnerId);
            st.OwnerName = owner?.UserName ?? string.Empty;
        }

        return Result.Success(stadiumsResponse);
    }

    // ================== GET STADIUM BY ID ==================
    public async Task<Result<StadiumResponse>> GetStadiumByIdAsync(
        int stadiumId,
        CancellationToken cancellationToken = default)
    {
        if (stadiumId <= 0)
            return Result.Failure<StadiumResponse>(StadiumErrors.InvalidId);

        var stadium = await stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium is null)
            return Result.Failure<StadiumResponse>(StadiumErrors.NotFound);

        var response = stadium.Adapt<StadiumResponse>();
        var owner = await userManager.FindByIdAsync(stadium.OwnerId);
        response.OwnerName = owner?.UserName ?? string.Empty;

        return Result.Success(response);
    }

    // ================== GET STADIUM DETAILS ==================
    public async Task<Result<StadiumDetailsResponse>> GetStadiumDetailsAsync(
     int stadiumId,
     CancellationToken cancellationToken = default)
    {
        if (stadiumId <= 0)
            return Result.Failure<StadiumDetailsResponse>(StadiumErrors.InvalidId);

        var stadium = await stadiumRepository.GetDetailsAsync(stadiumId, cancellationToken);
        if (stadium is null)
            return Result.Failure<StadiumDetailsResponse>(StadiumErrors.NotFound);

        // إعداد الـ Mapster لكل الحقول اللي محتاجة MapWith
        TypeAdapterConfig<Stadium, StadiumDetailsResponse>.NewConfig()
            .Map(dest => dest.TimeSlots, src => src.TimeSlots
                .Select(ts => ts.Adapt<TimeSlotResponse>())
                .ToList())
            .Map(dest => dest.Reviews, src => src.Reviews
                .Select(r => r.Adapt<ReviewResponse>())
                .ToList());

        // تحويل الـ Stadium إلى StadiumDetailsResponse
        var response = stadium.Adapt<StadiumDetailsResponse>();

        // إضافة اسم صاحب الملعب
        var owner = await userManager.FindByIdAsync(stadium.OwnerId);
        response.OwnerName = owner?.UserName ?? string.Empty;

        return Result.Success(response);
    }
    // ================== GET STADIUMS BY SPORT TYPE ==================
    public async Task<Result<List<StadiumResponse>>> GetStadiumsBySportTypeAsync(
        int sportTypeId,
        CancellationToken cancellationToken = default)
    {
        if (sportTypeId <= 0)
            return Result.Failure<List<StadiumResponse>>(StadiumErrors.SportTypeIdInvalid);

        var stadiums = await stadiumRepository.GetBySportTypeAsync(sportTypeId, cancellationToken);
        if (stadiums is null || !stadiums.Any())
            return Result.Failure<List<StadiumResponse>>(StadiumErrors.NotFound);

        var stadiumsResponse = stadiums.Adapt<List<StadiumResponse>>();
        foreach (var st in stadiumsResponse)
        {
            var owner = await userManager.FindByIdAsync(st.OwnerId);
            st.OwnerName = owner?.UserName ?? string.Empty;
        }

        return Result.Success(stadiumsResponse);
    }

    // ================== TOGGLE ACTIVE ==================
    public async Task<Result> ToggleStadiumActiveAsync(
        int stadiumId,
        CancellationToken cancellationToken = default)
    {
        if (stadiumId <= 0)
            return Result.Failure(StadiumErrors.InvalidId);

        var stadium = await stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium is null)
            return Result.Failure(StadiumErrors.NotFound);

        stadium.IsActive = !stadium.IsActive;
        await stadiumRepository.UpdateAsync(stadium, cancellationToken);

        return Result.Success();
    }

    // ================== UPDATE STADIUM ==================
    public async Task<Result> UpdateStadiumAsync(
        int stadiumId,
        UpdateStadiumRequest request,
        string currentUserId, // جديد: نتأكد إن اللي بيعدل هو المالك
        CancellationToken cancellationToken = default)
    {
        if (stadiumId <= 0)
            return Result.Failure(StadiumErrors.InvalidId);
        if (request is null)
            return Result.Failure(StadiumErrors.InvalidData);

        var stadium = await stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium is null)
            return Result.Failure(StadiumErrors.NotFound);

        // التأكد إن المالك هو اللي بيعدل
        if (stadium.OwnerId != currentUserId)
            return Result.Failure(StadiumErrors.NotAuthorized);

        // Optional Validations (مثل Create)
        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name.Length > 200)
            return Result.Failure(StadiumErrors.NameTooLong);
        if (request.ClosingTime <= request.OpeningTime)
            return Result.Failure(StadiumErrors.TimeInvalid);
        if (request.SlotDurationMinutes <= 0)
            return Result.Failure(StadiumErrors.SlotDurationInvalid);
        if (request.PricePerHour <= 0)
            return Result.Failure(StadiumErrors.PriceInvalid);

        request.Adapt(stadium);
        await stadiumRepository.UpdateAsync(stadium, cancellationToken);

        return Result.Success();
    }
}
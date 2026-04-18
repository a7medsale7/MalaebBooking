using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Reviews;
using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Contracts.TimeSlots;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums; // 👈 زودت دي
using MalaebBooking.Infrastructure.Persistence; // عشان الـ DbContext
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace MalaebBooking.Application.Services;

public class StadiumService(
    IStadiumRepository stadiumRepository, 
    ApplicationDbContext context) : IStadiumService // حقنا الـ DbContext للوصول للبروفايل
{
    private readonly IStadiumRepository _stadiumRepository = stadiumRepository;
    private readonly ApplicationDbContext _context = context;

    // ================== CREATE STADIUM ==================
    public async Task<Result> CreateStadiumAsync(
        CreateStadiumRequest request,
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        if (request is null) return Result.Failure(StadiumErrors.InvalidData);

        // 1. التأكد من وجود بروفايل مالك موثق (Approved) لليوزر الحالي
        var ownerProfile = await _context.StadiumOwnerProfiles
            .FirstOrDefaultAsync(p => p.UserId == currentUserId && p.Status == StadiumOwnerStatus.Approved, cancellationToken);

        if (ownerProfile == null)
            return Result.Failure(StadiumErrors.NotAuthorized); // اليوزر مش أونر موثق

        // 2. فالدبشن البيانات
        if (string.IsNullOrWhiteSpace(request.Name)) return Result.Failure(StadiumErrors.NameRequired);
        if (request.PricePerHourDay <= 0 || request.PricePerHourNight <= 0) return Result.Failure(StadiumErrors.PriceInvalid);
        if (string.IsNullOrWhiteSpace(request.InstapayNumber) && string.IsNullOrWhiteSpace(request.VodafoneCashNumber))
            return Result.Failure(StadiumErrors.PaymentMethodRequired);

        // 3. تخليق الملعب وربطه بالبروفايل
        var stadium = request.Adapt<Stadium>();
        stadium.OwnerProfileId = ownerProfile.Id; 

        await _stadiumRepository.AddAsync(stadium, cancellationToken);

        return Result.Success(stadium);
    }

    // ================== GET ACTIVE STADIUMS ==================
    public async Task<Result<List<StadiumResponse>>> GetActiveStadiumsAsync(CancellationToken cancellationToken = default)
    {
        var stadiums = await _stadiumRepository.GetActiveAsync(cancellationToken);
        if (stadiums == null || !stadiums.Any()) return Result.Failure<List<StadiumResponse>>(StadiumErrors.NotFound);

        var response = stadiums.Select(s => MapToStadiumResponse(s)).ToList();
        return Result.Success(response);
    }

    // ================== GET ALL STADIUMS ==================
    public async Task<Result<List<StadiumResponse>>> GetAllStadiumsAsync(CancellationToken cancellationToken = default)
    {
        var stadiums = await _stadiumRepository.GetAllAsync(cancellationToken);
        if (stadiums == null || !stadiums.Any()) return Result.Failure<List<StadiumResponse>>(StadiumErrors.NotFound);

        var response = stadiums.Select(s => MapToStadiumResponse(s)).ToList();
        return Result.Success(response);
    }

    // ================== GET STADIUM BY ID ==================
    public async Task<Result<StadiumResponse>> GetStadiumByIdAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        var stadium = await _stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium == null) return Result.Failure<StadiumResponse>(StadiumErrors.NotFound);

        return Result.Success(MapToStadiumResponse(stadium));
    }

    // ================== GET STADIUM DETAILS ==================
    public async Task<Result<StadiumDetailsResponse>> GetStadiumDetailsAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        var stadium = await _stadiumRepository.GetDetailsAsync(stadiumId, cancellationToken);
        if (stadium == null) return Result.Failure<StadiumDetailsResponse>(StadiumErrors.NotFound);

        var response = stadium.Adapt<StadiumDetailsResponse>();
        
        // جلب الصور والتقييمات والمواعيد
        response.Images = stadium.Images.Select(i => i.Adapt<StadiumImageResponse>()).ToList();
        response.Reviews = stadium.Reviews.Select(r => r.Adapt<ReviewResponse>()).ToList();
        response.TimeSlots = stadium.TimeSlots.Select(t => t.Adapt<TimeSlotResponse>()).ToList();
        
        // جلب اسم المالك من اليوزر المرتبط بالبروفايل
        response.OwnerName = $"{stadium.OwnerProfile?.User?.FirstName} {stadium.OwnerProfile?.User?.LastName}";
        response.SportTypeName = stadium.SportType?.Name ?? string.Empty;

        return Result.Success(response);
    }

    // ================== GET STADIUMS BY SPORT TYPE ==================
    public async Task<Result<List<StadiumResponse>>> GetStadiumsBySportTypeAsync(int sportTypeId, CancellationToken cancellationToken = default)
    {
        var stadiums = await _stadiumRepository.GetBySportTypeAsync(sportTypeId, cancellationToken);
        if (stadiums == null || !stadiums.Any()) return Result.Failure<List<StadiumResponse>>(StadiumErrors.NotFound);

        var response = stadiums.Select(s => MapToStadiumResponse(s)).ToList();
        return Result.Success(response);
    }

    // ================== UPDATE STADIUM ==================
    public async Task<Result> UpdateStadiumAsync(int stadiumId, UpdateStadiumRequest request, string currentUserId, CancellationToken cancellationToken = default)
    {
        var stadium = await _stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium == null) return Result.Failure(StadiumErrors.NotFound);

        // التأكد إن اللي بيعدل هو صاحب الملعب فعلاً
        if (stadium.OwnerProfile?.UserId != currentUserId)
            return Result.Failure(StadiumErrors.NotAuthorized);

        request.Adapt(stadium);
        await _stadiumRepository.UpdateAsync(stadium, cancellationToken);

        return Result.Success();
    }

    // ================== TOGGLE ACTIVE ==================
    public async Task<Result> ToggleStadiumActiveAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        var stadium = await _stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium == null) return Result.Failure(StadiumErrors.NotFound);

        stadium.IsActive = !stadium.IsActive;
        await _stadiumRepository.UpdateAsync(stadium, cancellationToken);

        return Result.Success();
    }

    // ================== GET MY STADIUMS ==================
    public async Task<Result<List<StadiumResponse>>> GetMyStadiumsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var stadiums = await _stadiumRepository.GetByOwnerUserIdAsync(userId, cancellationToken);
        
        var response = stadiums.Select(s => MapToStadiumResponse(s)).ToList();
        
        return Result.Success(response);
    }

    // --- Private Helper Method عشان نوحد شكل الـ Response ---
    private static StadiumResponse MapToStadiumResponse(Stadium stadium)
    {
        var response = stadium.Adapt<StadiumResponse>();
        response.Images = stadium.Images.Select(i => i.Adapt<StadiumImageResponse>()).ToList();
        
        // سحب اسم المالك والـ ID بتاعه مباشرة من البروفايل الملحق
        response.OwnerId = stadium.OwnerProfile?.UserId ?? string.Empty;
        response.OwnerName = $"{stadium.OwnerProfile?.User?.FirstName} {stadium.OwnerProfile?.User?.LastName}";
        response.SportTypeName = stadium.SportType?.Name ?? string.Empty;
        
        return response;
    }
}

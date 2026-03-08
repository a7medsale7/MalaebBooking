using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Stadiums;

namespace MalaebBooking.Application.Services;

public interface IStadiumService
{
    // ================== CREATE STADIUM ==================
    Task<Result> CreateStadiumAsync(
        CreateStadiumRequest request,
        string currentUserId, // جديد: ID صاحب الستاد
        CancellationToken cancellationToken = default);

    // ================== UPDATE STADIUM ==================
    Task<Result> UpdateStadiumAsync(
        int stadiumId,
        UpdateStadiumRequest request,
        string currentUserId, // جديد: ID صاحب الستاد للتأكد من الصلاحيات
        CancellationToken cancellationToken = default);

    // ================== GET ALL STADIUMS ==================
    Task<Result<List<StadiumResponse>>> GetAllStadiumsAsync(
        CancellationToken cancellationToken = default);

    // ================== GET ACTIVE STADIUMS ==================
    Task<Result<List<StadiumResponse>>> GetActiveStadiumsAsync(
        CancellationToken cancellationToken = default);

    // ================== GET STADIUM BY ID ==================
    Task<Result<StadiumResponse>> GetStadiumByIdAsync(
        int stadiumId,
        CancellationToken cancellationToken = default);

    // ================== GET STADIUM DETAILS ==================
    Task<Result<StadiumDetailsResponse>> GetStadiumDetailsAsync(
        int stadiumId,
        CancellationToken cancellationToken = default);

    // ================== GET STADIUMS BY SPORT TYPE ==================
    Task<Result<List<StadiumResponse>>> GetStadiumsBySportTypeAsync(
        int sportTypeId,
        CancellationToken cancellationToken = default);

    // ================== TOGGLE ACTIVE ==================
    Task<Result> ToggleStadiumActiveAsync(
        int stadiumId,
        CancellationToken cancellationToken = default);
}
// MalaebBooking.Domain/Abstractions/Repositories/IStadiumImageRepository.cs

using MalaebBooking.Domain.Entities;

namespace MalaebBooking.Domain.Abstractions.Repositories;

public interface IStadiumImageRepository
{
    Task AddAsync(StadiumImage image, CancellationToken cancellationToken = default);

    Task<StadiumImage?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<StadiumImage>> GetByStadiumIdAsync(int stadiumId, CancellationToken cancellationToken = default);

    Task UpdateAsync(StadiumImage image, CancellationToken cancellationToken = default);

    Task DeleteAsync(StadiumImage image, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}

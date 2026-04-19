using System.Collections.Generic;

namespace MalaebBooking.Application.Contracts.Common;

public class PaginatedResponse<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }

    public PaginatedResponse(IReadOnlyList<T> items, int count, int pageIndex, int totalPages, bool hasNextPage, bool hasPreviousPage)
    {
        Items = items;
        TotalCount = count;
        PageIndex = pageIndex;
        TotalPages = totalPages;
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
    }
}

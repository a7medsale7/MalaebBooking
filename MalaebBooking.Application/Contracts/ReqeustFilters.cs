using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts;
public class ReqeustFilters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchValue { get; init; } = string.Empty;
    public string SortColumn { get; init; } = string.Empty;
    public string SortDirection { get; init; } = "asc";

}

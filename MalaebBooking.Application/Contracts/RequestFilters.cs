using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts;
public class RequestFilters
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SearchValue { get; set; }

    public string? SortColumn { get; set; }

    public string? SortDirection { get; set; }
}

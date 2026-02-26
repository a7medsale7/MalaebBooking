using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.SportTypes;
public record UpdateSportTypeRequest(string Name,
    string? Description,
    string? IconUrl,
    bool IsActive);


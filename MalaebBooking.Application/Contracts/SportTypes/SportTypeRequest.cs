using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.SportTypes;
public class SportTypeRequest
{
    public string Name { get; set; } = string.Empty; // الأفضل تعطي default
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Roles;
public class RoleDetailResponse
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public IEnumerable<string> Permissions { get; set; } = default!;
}

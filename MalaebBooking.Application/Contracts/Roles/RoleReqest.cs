using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Contracts.Roles;
public class RoleReqest
{
    public string Name { get; set; }
    public IList<string> Permissions { get; set; } = default!;
}

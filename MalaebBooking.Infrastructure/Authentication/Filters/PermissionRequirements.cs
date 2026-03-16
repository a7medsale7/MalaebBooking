using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Authentication.Filters;
public class PermissionRequirements(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}

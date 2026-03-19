using MalaebBooking.Application.Abstractions.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Errors;
public class RoleErrors
{
    public static readonly Error NotFound =
            new("Role.NotFound", "Role was not found.");

    public static readonly Error DuplicatedRole =
       new ("DuplicatedRole", "This role already exists.");

    public static readonly Error InvalidPermissions =
        new ("InvalidPermissions", "One or more permissions are invalid.");

    

    public static readonly Error CreationFailed =
        new ("RoleCreationFailed", "Failed to create the role.");

    public static readonly Error UpdateFailed =
        new ("RoleUpdateFailed", "Failed to update the role.");

    public static readonly Error DeleteFailed =
        new ("RoleDeleteFailed", "Failed to delete the role.");
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Consts;
public static class DefaultRoles
{
    public const string Admin = "Admin";
    public const string AdminRoleId = "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297";
    public const string AdminRoleConcurrencyStamp = "d1c8e5b9-9f0a-4c3e-8a1b-2f5e6d7c8a9b";


    public const string Owner = "Owner";
    public const string OwnerRoleId = "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4";
    public const string OwnerRoleConcurrencyStamp = "e2f9c6a7-8b1d-4c3e-9a2b-3f6d7c8a9b0c";



    public const string Player = "Player";
    public const string PlayerRoleId = "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9";
    public const string PlayerRoleConcurrencyStamp = "f3a7b8c9-0d2e-4f5a-9b3c-4d6e7f8a9b1c";

}
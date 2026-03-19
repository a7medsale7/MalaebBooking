using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Consts;
public static class Permissions
{
    public const string Type = "Permissions";
    // --- Users Permissions ---
    // Users
    public const string Users_ViewProfile = "Permissions.Users.ViewProfile";
    public const string Users_UpdateProfile = "Permissions.Users.UpdateProfile";
    public const string Users_ChangePassword = "Permissions.Users.ChangePassword";
    public const string Users_ViewAll = "Permissions.Users.ViewAll";
    public const string Users_ManageRoles = "Permissions.Users.ManageRoles";

    // Stadiums
    public const string Stadiums_View = "Permissions.Stadiums.View";
    public const string Stadiums_Create = "Permissions.Stadiums.Create";
    public const string Stadiums_Update = "Permissions.Stadiums.Update";
    public const string Stadiums_ToggleActive = "Permissions.Stadiums.ToggleActive";
    public const string Stadiums_Delete = "Permissions.Stadiums.Delete";

    // Stadium Images
    public const string StadiumImages_View = "Permissions.StadiumImages.View";
    public const string StadiumImages_Upload = "Permissions.StadiumImages.Upload";
    public const string StadiumImages_Update = "Permissions.StadiumImages.Update";
    public const string StadiumImages_Delete = "Permissions.StadiumImages.Delete";

    // TimeSlots
    public const string TimeSlots_View = "Permissions.TimeSlots.View";
    public const string TimeSlots_Create = "Permissions.TimeSlots.Create";
    public const string TimeSlots_Update = "Permissions.TimeSlots.Update";
    public const string TimeSlots_Delete = "Permissions.TimeSlots.Delete";

    // SportTypes
    public const string SportTypes_View = "Permissions.SportTypes.View";
    public const string SportTypes_Create = "Permissions.SportTypes.Create";
    public const string SportTypes_Update = "Permissions.SportTypes.Update";
    public const string SportTypes_Delete = "Permissions.SportTypes.Delete";
    public const string SportTypes_UploadIcon = "Permissions.SportTypes.UploadIcon";

    // Bookings
    public const string Bookings_View = "Permissions.Bookings.View";
    public const string Bookings_Create = "Permissions.Bookings.Create";
    public const string Bookings_UpdateStatus = "Permissions.Bookings.UpdateStatus";
    public const string Bookings_Cancel = "Permissions.Bookings.Cancel";
    public const string Bookings_Delete = "Permissions.Bookings.Delete";

    // Payments
    public const string Payments_View = "Permissions.Payments.View";
    public const string Payments_SubmitProof = "Permissions.Payments.SubmitProof";
    public const string Payments_Approve = "Permissions.Payments.Approve";
    public const string Payments_Reject = "Permissions.Payments.Reject";

    // Reviews
    public const string Reviews_View = "Permissions.Reviews.View";
    public const string Reviews_Create = "Permissions.Reviews.Create";
    public const string Reviews_Update = "Permissions.Reviews.Update";
    public const string Reviews_Delete = "Permissions.Reviews.Delete";

    public const string Roles_View = "Permissions.Roles.View";
    public const string Roles_Create = "Permissions.Roles.Create";
    public const string Roles_Update = "Permissions.Roles.Update";
    public const string Roles_ToggleActive = "Permissions.Roles.ToggleActive";



    public static IList<string?> GetAllPermissions()=>
        typeof(Permissions).GetFields().Select(f => f.GetValue(f) as string).ToList();
}

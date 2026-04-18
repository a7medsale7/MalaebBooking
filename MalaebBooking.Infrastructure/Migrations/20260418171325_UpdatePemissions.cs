using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePemissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.OwnerProfiles.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.OwnerProfiles.Apply", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.OwnerProfiles.Review", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45,
                column: "ClaimValue",
                value: "Permissions.Users.ViewProfile");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46,
                column: "ClaimValue",
                value: "Permissions.Users.UpdateProfile");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47,
                column: "ClaimValue",
                value: "Permissions.Users.ChangePassword");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48,
                column: "ClaimValue",
                value: "Permissions.Stadiums.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49,
                column: "ClaimValue",
                value: "Permissions.Stadiums.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                column: "ClaimValue",
                value: "Permissions.Stadiums.Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                column: "ClaimValue",
                value: "Permissions.Stadiums.ToggleActive");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.Upload");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 55,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.Delete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 56,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 57,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 58,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 59,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.Delete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 60,
                column: "ClaimValue",
                value: "Permissions.Bookings.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 61,
                column: "ClaimValue",
                value: "Permissions.Bookings.UpdateStatus");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 62,
                column: "ClaimValue",
                value: "Permissions.Bookings.Cancel");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 63,
                column: "ClaimValue",
                value: "Permissions.Payments.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Payments.Approve", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Payments.Reject", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Reviews.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.OwnerProfiles.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 68,
                column: "ClaimValue",
                value: "Permissions.Users.ViewProfile");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 69,
                column: "ClaimValue",
                value: "Permissions.Users.UpdateProfile");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 70,
                column: "ClaimValue",
                value: "Permissions.Users.ChangePassword");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 71,
                column: "ClaimValue",
                value: "Permissions.Stadiums.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 72,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 73,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 74,
                column: "ClaimValue",
                value: "Permissions.SportTypes.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 75,
                column: "ClaimValue",
                value: "Permissions.Bookings.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 76,
                column: "ClaimValue",
                value: "Permissions.Bookings.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 77,
                column: "ClaimValue",
                value: "Permissions.Bookings.Cancel");

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 78, "Permissions", "Permissions.Payments.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 79, "Permissions", "Permissions.Payments.SubmitProof", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 80, "Permissions", "Permissions.Reviews.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 81, "Permissions", "Permissions.Reviews.Create", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 82, "Permissions", "Permissions.OwnerProfiles.Apply", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 83, "Permissions", "Permissions.OwnerProfiles.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Users.ViewProfile", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Users.UpdateProfile", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Users.ChangePassword", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45,
                column: "ClaimValue",
                value: "Permissions.Stadiums.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46,
                column: "ClaimValue",
                value: "Permissions.Stadiums.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47,
                column: "ClaimValue",
                value: "Permissions.Stadiums.Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48,
                column: "ClaimValue",
                value: "Permissions.Stadiums.ToggleActive");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.Upload");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.Delete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 55,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 56,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.Delete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 57,
                column: "ClaimValue",
                value: "Permissions.Bookings.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 58,
                column: "ClaimValue",
                value: "Permissions.Bookings.UpdateStatus");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 59,
                column: "ClaimValue",
                value: "Permissions.Bookings.Cancel");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 60,
                column: "ClaimValue",
                value: "Permissions.Payments.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 61,
                column: "ClaimValue",
                value: "Permissions.Payments.Approve");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 62,
                column: "ClaimValue",
                value: "Permissions.Payments.Reject");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 63,
                column: "ClaimValue",
                value: "Permissions.Reviews.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Users.ViewProfile", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Users.UpdateProfile", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Users.ChangePassword", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Stadiums.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 68,
                column: "ClaimValue",
                value: "Permissions.StadiumImages.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 69,
                column: "ClaimValue",
                value: "Permissions.TimeSlots.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 70,
                column: "ClaimValue",
                value: "Permissions.SportTypes.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 71,
                column: "ClaimValue",
                value: "Permissions.Bookings.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 72,
                column: "ClaimValue",
                value: "Permissions.Bookings.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 73,
                column: "ClaimValue",
                value: "Permissions.Bookings.Cancel");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 74,
                column: "ClaimValue",
                value: "Permissions.Payments.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 75,
                column: "ClaimValue",
                value: "Permissions.Payments.SubmitProof");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 76,
                column: "ClaimValue",
                value: "Permissions.Reviews.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 77,
                column: "ClaimValue",
                value: "Permissions.Reviews.Create");
        }
    }
}

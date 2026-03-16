using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297", "d1c8e5b9-9f0a-4c3e-8a1b-2f5e6d7c8a9b", false, false, "Admin", "ADMIN" },
                    { "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4", "e2f9c6a7-8b1d-4c3e-9a2b-3f6d7c8a9b0c", false, false, "Owner", "OWNER" },
                    { "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9", "f3a7b8c9-0d2e-4f5a-9b3c-4d6e7f8a9b1c", true, false, "Player", "PLAYER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "877a5585-4894-4f4b-8989-f45476063ce1", 0, "a25eba83-ab00-408b-8dfa-ce8a1cf37bea", "Admin@Malaeb-Booking.com", true, "Malaeb", false, "Admin", false, null, "ADMIN@MALAEB-BOOKING.COM", "ADMIN@MALAEB-BOOKING.COM", "AQAAAAIAAYagAAAAEBu6H7C9iF9Tq3twVRr0uJaFzrtfpWgkhEKBHa0jsAD+KuMRutaglQhMbnbICcDKyQ==", null, false, "CFFCC4EEB0EE4D608E7CEFFE61FFDBD2", false, "Admin@Malaeb-Booking.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "Permission", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 2, "Permission", "Permissions.Users.ViewProfile", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 3, "Permission", "Permissions.Users.UpdateProfile", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 4, "Permission", "Permissions.Users.ChangePassword", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 5, "Permission", "Permissions.Users.ViewAll", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 6, "Permission", "Permissions.Users.ManageRoles", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 7, "Permission", "Permissions.Stadiums.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 8, "Permission", "Permissions.Stadiums.Create", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 9, "Permission", "Permissions.Stadiums.Update", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 10, "Permission", "Permissions.Stadiums.ToggleActive", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 11, "Permission", "Permissions.Stadiums.Delete", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 12, "Permission", "Permissions.StadiumImages.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 13, "Permission", "Permissions.StadiumImages.Upload", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 14, "Permission", "Permissions.StadiumImages.Update", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 15, "Permission", "Permissions.StadiumImages.Delete", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 16, "Permission", "Permissions.TimeSlots.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 17, "Permission", "Permissions.TimeSlots.Create", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 18, "Permission", "Permissions.TimeSlots.Update", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 19, "Permission", "Permissions.TimeSlots.Delete", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 20, "Permission", "Permissions.SportTypes.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 21, "Permission", "Permissions.SportTypes.Create", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 22, "Permission", "Permissions.SportTypes.Update", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 23, "Permission", "Permissions.SportTypes.Delete", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 24, "Permission", "Permissions.SportTypes.UploadIcon", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 25, "Permission", "Permissions.Bookings.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 26, "Permission", "Permissions.Bookings.Create", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 27, "Permission", "Permissions.Bookings.UpdateStatus", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 28, "Permission", "Permissions.Bookings.Cancel", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 29, "Permission", "Permissions.Bookings.Delete", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 30, "Permission", "Permissions.Payments.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 31, "Permission", "Permissions.Payments.SubmitProof", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 32, "Permission", "Permissions.Payments.Approve", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 33, "Permission", "Permissions.Payments.Reject", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 34, "Permission", "Permissions.Reviews.View", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 35, "Permission", "Permissions.Reviews.Create", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 36, "Permission", "Permissions.Reviews.Update", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" },
                    { 37, "Permission", "Permissions.Reviews.Delete", "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297", "877a5585-4894-4f4b-8989-f45476063ce1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297", "877a5585-4894-4f4b-8989-f45476063ce1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a6ce7a1-2b66-48dd-ba28-3cf7080a3297");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "877a5585-4894-4f4b-8989-f45476063ce1");
        }
    }
}

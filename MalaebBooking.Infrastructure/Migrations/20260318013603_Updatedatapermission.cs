using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatedatapermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 38, "Permissions", "Permissions.Users.ViewProfile", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 39, "Permissions", "Permissions.Users.UpdateProfile", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 40, "Permissions", "Permissions.Users.ChangePassword", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 41, "Permissions", "Permissions.Stadiums.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 42, "Permissions", "Permissions.Stadiums.Create", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 43, "Permissions", "Permissions.Stadiums.Update", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 44, "Permissions", "Permissions.Stadiums.ToggleActive", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 45, "Permissions", "Permissions.StadiumImages.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 46, "Permissions", "Permissions.StadiumImages.Upload", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 47, "Permissions", "Permissions.StadiumImages.Update", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 48, "Permissions", "Permissions.StadiumImages.Delete", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 49, "Permissions", "Permissions.TimeSlots.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 50, "Permissions", "Permissions.TimeSlots.Create", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 51, "Permissions", "Permissions.TimeSlots.Update", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 52, "Permissions", "Permissions.TimeSlots.Delete", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 53, "Permissions", "Permissions.Bookings.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 54, "Permissions", "Permissions.Bookings.UpdateStatus", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 55, "Permissions", "Permissions.Bookings.Cancel", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 56, "Permissions", "Permissions.Payments.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 57, "Permissions", "Permissions.Payments.Approve", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 58, "Permissions", "Permissions.Payments.Reject", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 59, "Permissions", "Permissions.Reviews.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 59);
        }
    }
}

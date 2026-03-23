using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adplayerpermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42,
                column: "RoleId",
                value: "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43,
                column: "RoleId",
                value: "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44,
                column: "RoleId",
                value: "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45,
                column: "RoleId",
                value: "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.StadiumImages.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.TimeSlots.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.SportTypes.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Bookings.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Bookings.Create", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Bookings.Cancel", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Payments.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Payments.SubmitProof", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Reviews.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Reviews.Create", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42,
                column: "RoleId",
                value: "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43,
                column: "RoleId",
                value: "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44,
                column: "RoleId",
                value: "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45,
                column: "RoleId",
                value: "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Stadiums.Create", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Stadiums.Update", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.Stadiums.ToggleActive", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.StadiumImages.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.StadiumImages.Upload", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.StadiumImages.Update", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.StadiumImages.Delete", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.TimeSlots.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.TimeSlots.Create", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "ClaimValue", "RoleId" },
                values: new object[] { "Permissions.TimeSlots.Update", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 56, "Permissions", "Permissions.TimeSlots.Delete", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 57, "Permissions", "Permissions.Bookings.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 58, "Permissions", "Permissions.Bookings.UpdateStatus", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 59, "Permissions", "Permissions.Bookings.Cancel", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 60, "Permissions", "Permissions.Payments.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 61, "Permissions", "Permissions.Payments.Approve", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 62, "Permissions", "Permissions.Payments.Reject", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 63, "Permissions", "Permissions.Reviews.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" }
                });
        }
    }
}

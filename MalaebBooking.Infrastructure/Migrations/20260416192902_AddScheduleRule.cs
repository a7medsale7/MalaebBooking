using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleRuleId",
                table: "TimeSlots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StadiumId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ExcludedHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleRules_Stadiums_StadiumId",
                        column: x => x.StadiumId,
                        principalTable: "Stadiums",
                        principalColumn: "Id");
                });

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
                    { 63, "Permissions", "Permissions.Reviews.View", "5ab3a4d8-f2ee-48df-b897-a69fa5ef88d4" },
                    { 64, "Permissions", "Permissions.Users.ViewProfile", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 65, "Permissions", "Permissions.Users.UpdateProfile", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 66, "Permissions", "Permissions.Users.ChangePassword", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 67, "Permissions", "Permissions.Stadiums.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 68, "Permissions", "Permissions.StadiumImages.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 69, "Permissions", "Permissions.TimeSlots.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 70, "Permissions", "Permissions.SportTypes.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 71, "Permissions", "Permissions.Bookings.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 72, "Permissions", "Permissions.Bookings.Create", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 73, "Permissions", "Permissions.Bookings.Cancel", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 74, "Permissions", "Permissions.Payments.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 75, "Permissions", "Permissions.Payments.SubmitProof", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 76, "Permissions", "Permissions.Reviews.View", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" },
                    { 77, "Permissions", "Permissions.Reviews.Create", "b9a61ca4-01bb-4a4f-8ccc-ca5dd59b42f9" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_ScheduleRuleId",
                table: "TimeSlots",
                column: "ScheduleRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleRules_StadiumId",
                table: "ScheduleRules",
                column: "StadiumId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlots_ScheduleRules_ScheduleRuleId",
                table: "TimeSlots",
                column: "ScheduleRuleId",
                principalTable: "ScheduleRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_ScheduleRules_ScheduleRuleId",
                table: "TimeSlots");

            migrationBuilder.DropTable(
                name: "ScheduleRules");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_ScheduleRuleId",
                table: "TimeSlots");

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

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DropColumn(
                name: "ScheduleRuleId",
                table: "TimeSlots");

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
    }
}

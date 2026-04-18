using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStadiumDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerHour",
                table: "Stadiums",
                newName: "PricePerHourNight");

            migrationBuilder.AddColumn<string>(
                name: "CourtType",
                table: "Stadiums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "Stadiums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHourDay",
                table: "Stadiums",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "SummerNightStartTime",
                table: "Stadiums",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "VodafoneCashNumber",
                table: "Stadiums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WinterNightStartTime",
                table: "Stadiums",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourtType",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "PricePerHourDay",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "SummerNightStartTime",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "VodafoneCashNumber",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "WinterNightStartTime",
                table: "Stadiums");

            migrationBuilder.RenameColumn(
                name: "PricePerHourNight",
                table: "Stadiums",
                newName: "PricePerHour");
        }
    }
}

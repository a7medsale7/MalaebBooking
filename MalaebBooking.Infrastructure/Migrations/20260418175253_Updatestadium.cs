using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatestadium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Stadiums",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "Stadiums",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "Stadiums");
        }
    }
}

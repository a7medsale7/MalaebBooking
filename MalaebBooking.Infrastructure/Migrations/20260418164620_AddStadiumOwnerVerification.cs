using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MalaebBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStadiumOwnerVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stadiums_AspNetUsers_OwnerId",
                table: "Stadiums");

            migrationBuilder.DropIndex(
                name: "IX_Stadiums_OwnerId",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Stadiums");

            migrationBuilder.AddColumn<int>(
                name: "OwnerProfileId",
                table: "Stadiums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StadiumOwnerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdImageFront = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdImageBack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnershipContractImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialRegisterNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialRegisterImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeLicenseImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumOwnerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StadiumOwnerProfiles_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StadiumOwnerProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stadiums_OwnerProfileId",
                table: "Stadiums",
                column: "OwnerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_StadiumOwnerProfiles_ApprovedById",
                table: "StadiumOwnerProfiles",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_StadiumOwnerProfiles_UserId",
                table: "StadiumOwnerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stadiums_StadiumOwnerProfiles_OwnerProfileId",
                table: "Stadiums",
                column: "OwnerProfileId",
                principalTable: "StadiumOwnerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stadiums_StadiumOwnerProfiles_OwnerProfileId",
                table: "Stadiums");

            migrationBuilder.DropTable(
                name: "StadiumOwnerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Stadiums_OwnerProfileId",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "OwnerProfileId",
                table: "Stadiums");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Stadiums",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Stadiums_OwnerId",
                table: "Stadiums",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stadiums_AspNetUsers_OwnerId",
                table: "Stadiums",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventify.Infrastructure.Migrations.Events
{
    /// <inheritdoc />
    public partial class ManyToManyArtistProfileTimeTableSlot3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistProfileTimeTableSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TimeTableSlotArtistProfiles_TimeTableSlotId",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles",
                columns: new[] { "TimeTableSlotId", "ArtistProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableSlotArtistProfiles_ArtistProfileId",
                table: "TimeTableSlotArtistProfiles",
                column: "ArtistProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TimeTableSlotArtistProfiles_ArtistProfileId",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles",
                columns: new[] { "ArtistProfileId", "TimeTableSlotId" });

            migrationBuilder.CreateTable(
                name: "ArtistProfileTimeTableSlot",
                columns: table => new
                {
                    ArtistProfilesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeTableSlotsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistProfileTimeTableSlot", x => new { x.ArtistProfilesId, x.TimeTableSlotsId });
                    table.ForeignKey(
                        name: "FK_ArtistProfileTimeTableSlot_ArtistProfiles_ArtistProfilesId",
                        column: x => x.ArtistProfilesId,
                        principalTable: "ArtistProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistProfileTimeTableSlot_TimeTableSlots_TimeTableSlotsId",
                        column: x => x.TimeTableSlotsId,
                        principalTable: "TimeTableSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableSlotArtistProfiles_TimeTableSlotId",
                table: "TimeTableSlotArtistProfiles",
                column: "TimeTableSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfileTimeTableSlot_TimeTableSlotsId",
                table: "ArtistProfileTimeTableSlot",
                column: "TimeTableSlotsId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventify.Infrastructure.Migrations.Events
{
    /// <inheritdoc />
    public partial class ManyToManyArtistProfileTimeTableSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeTableSlotArtistProfiles",
                columns: table => new
                {
                    TimeTableSlotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTableSlotArtistProfiles", x => new { x.TimeTableSlotId, x.ArtistProfileId });
                    table.ForeignKey(
                        name: "FK_TimeTableSlotArtistProfiles_ArtistProfiles_ArtistProfileId",
                        column: x => x.ArtistProfileId,
                        principalTable: "ArtistProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeTableSlotArtistProfiles_TimeTableSlots_TimeTableSlotId",
                        column: x => x.TimeTableSlotId,
                        principalTable: "TimeTableSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableSlotArtistProfiles_ArtistProfileId",
                table: "TimeTableSlotArtistProfiles",
                column: "ArtistProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeTableSlotArtistProfiles");
        }
    }
}

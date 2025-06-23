using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventify.Infrastructure.Migrations.Events
{
    /// <inheritdoc />
    public partial class ManyToManyArtistProfileTimeTableSlot5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTableSlots_TimeTables_TimeTableId",
                table: "TimeTableSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TimeTableSlotArtistProfiles_ArtistProfileId",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.RenameColumn(
                name: "TimeTableId",
                table: "TimeTableSlots",
                newName: "TimetableId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeTableSlots_TimeTableId",
                table: "TimeTableSlots",
                newName: "IX_TimeTableSlots_TimetableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles",
                columns: new[] { "ArtistProfileId", "TimeTableSlotId" });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableSlotArtistProfiles_TimeTableSlotId",
                table: "TimeTableSlotArtistProfiles",
                column: "TimeTableSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTableSlots_TimeTables_TimetableId",
                table: "TimeTableSlots",
                column: "TimetableId",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTableSlots_TimeTables_TimetableId",
                table: "TimeTableSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TimeTableSlotArtistProfiles_TimeTableSlotId",
                table: "TimeTableSlotArtistProfiles");

            migrationBuilder.RenameColumn(
                name: "TimetableId",
                table: "TimeTableSlots",
                newName: "TimeTableId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeTableSlots_TimetableId",
                table: "TimeTableSlots",
                newName: "IX_TimeTableSlots_TimeTableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeTableSlotArtistProfiles",
                table: "TimeTableSlotArtistProfiles",
                columns: new[] { "TimeTableSlotId", "ArtistProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableSlotArtistProfiles_ArtistProfileId",
                table: "TimeTableSlotArtistProfiles",
                column: "ArtistProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTableSlots_TimeTables_TimeTableId",
                table: "TimeTableSlots",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

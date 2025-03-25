using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplyOptimizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Members_MemberId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MemberId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "MemberEvent",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Attended = table.Column<bool>(type: "bit", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberEvent", x => new { x.MemberId, x.EventId });
                    table.ForeignKey(
                        name: "FK_MemberEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberEvent_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberEvent_EventId",
                table: "MemberEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEvent_MemberId",
                table: "MemberEvent",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberEvent");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_MemberId",
                table: "Events",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Members_MemberId",
                table: "Events",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}

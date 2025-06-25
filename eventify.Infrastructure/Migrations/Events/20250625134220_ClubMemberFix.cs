using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventify.Infrastructure.Migrations.Events
{
    /// <inheritdoc />
    public partial class ClubMemberFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubOwners");

            migrationBuilder.DropColumn(
                name: "_ownerMemberIds",
                table: "Clubs");

            migrationBuilder.CreateTable(
                name: "MemberClubs",
                columns: table => new
                {
                    ClubId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberClubs", x => new { x.ClubId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_MemberClubs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberClubs_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberClubs_MemberId",
                table: "MemberClubs",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberClubs");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "_ownerMemberIds",
                table: "Clubs",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "ClubOwners",
                columns: table => new
                {
                    ClubId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubOwners", x => new { x.ClubId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ClubOwners_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubOwners_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubOwners_MemberId",
                table: "ClubOwners",
                column: "MemberId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventify.Infrastructure.Migrations.Events
{
    /// <inheritdoc />
    public partial class TimeToDateTimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add temporary columns for the new DateTime values
            migrationBuilder.AddColumn<DateTime>(
                name: "TempStartTime",
                table: "TimeTableSlots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            
            migrationBuilder.AddColumn<DateTime>(
                name: "TempEndTime",
                table: "TimeTableSlots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            // Convert TimeSpan to DateTime (using current date as default)
            migrationBuilder.Sql(@"
                UPDATE ""TimeTableSlots""
                SET ""TempStartTime"" = (date '1970-01-01' + ""StartTime"") AT TIME ZONE 'UTC',
                    ""TempEndTime"" = (date '1970-01-01' + ""EndTime"") AT TIME ZONE 'UTC'
            ");

            // Drop the old TimeSpan columns
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TimeTableSlots");
            
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeTableSlots");

            // Rename the temporary columns to the original names
            migrationBuilder.RenameColumn(
                name: "TempStartTime",
                table: "TimeTableSlots",
                newName: "StartTime");
            
            migrationBuilder.RenameColumn(
                name: "TempEndTime",
                table: "TimeTableSlots",
                newName: "EndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add temporary columns for the old TimeSpan values
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TempStartTime",
                table: "TimeTableSlots",
                type: "interval",
                nullable: false,
                defaultValue: TimeSpan.Zero);
            
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TempEndTime",
                table: "TimeTableSlots",
                type: "interval",
                nullable: false,
                defaultValue: TimeSpan.Zero);

            // Convert DateTime back to TimeSpan (extract time portion)
            migrationBuilder.Sql(@"
                UPDATE ""TimeTableSlots""
                SET ""TempStartTime"" = (""StartTime"" AT TIME ZONE 'UTC')::time,
                    ""TempEndTime"" = (""EndTime"" AT TIME ZONE 'UTC')::time
            ");

            // Drop the DateTime columns
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TimeTableSlots");
            
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeTableSlots");

            // Rename the temporary columns to the original names
            migrationBuilder.RenameColumn(
                name: "TempStartTime",
                table: "TimeTableSlots",
                newName: "StartTime");
            
            migrationBuilder.RenameColumn(
                name: "TempEndTime",
                table: "TimeTableSlots",
                newName: "EndTime");
        }
    }
}
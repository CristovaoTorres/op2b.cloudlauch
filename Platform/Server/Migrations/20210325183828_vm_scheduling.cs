using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Server.Migrations
{
    public partial class vm_scheduling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserVMSchedulings",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionTypeId = table.Column<int>(type: "int", nullable: false),
                    WeekDaySunday = table.Column<bool>(type: "bit", nullable: false),
                    WeekDayMonday = table.Column<bool>(type: "bit", nullable: false),
                    WeekDayTuesday = table.Column<bool>(type: "bit", nullable: false),
                    WeekDayWednesday = table.Column<bool>(type: "bit", nullable: false),
                    WeekDayThursday = table.Column<bool>(type: "bit", nullable: false),
                    WeekDayFriday = table.Column<bool>(type: "bit", nullable: false),
                    WeekDaySaturday = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVMSchedulings", x => new { x.UserId, x.ActionTypeId });
                    table.ForeignKey(
                        name: "FK_UserVMSchedulings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57f0d489-4f14-417d-8646-5a665d8bbfeb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43afe25f-3e99-443e-9347-f8b690ebb162", "AQAAAAEAACcQAAAAEJngPyCPy5mSUWLFWDkjb5zdkCaJS4lDf03yzXC2FCLKwcZdZ+Z03JQmXEKLhiXt/g==", "6f688a38-7451-4417-a2a5-78935e75a955" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserVMSchedulings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57f0d489-4f14-417d-8646-5a665d8bbfeb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ef18faf-0827-475e-8442-ab6c14f379ca", "AQAAAAEAACcQAAAAEPBZ2JMmyGzYKQ7EHwxB0Gnry1zppkVAugvrTivEJ9vUW9Y1glJimThh5t29SboLaQ==", "956f4754-eb39-40f9-9b9d-eb6fae1410a9" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Server.Migrations
{
    public partial class vm_scheduling1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "UserVMSchedulings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "UserVMSchedulings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57f0d489-4f14-417d-8646-5a665d8bbfeb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46b76a1d-4eb8-431a-9c3a-fcf0c64cc44f", "AQAAAAEAACcQAAAAECU//IJDssW4I6X60y+eS1gqUIb8J40JkyBsHGtnCR3XuQg3jZYbllwAfO+zc1+2og==", "e1e07354-111e-425c-b9d5-99887f366f7a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "UserVMSchedulings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "UserVMSchedulings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57f0d489-4f14-417d-8646-5a665d8bbfeb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43afe25f-3e99-443e-9347-f8b690ebb162", "AQAAAAEAACcQAAAAEJngPyCPy5mSUWLFWDkjb5zdkCaJS4lDf03yzXC2FCLKwcZdZ+Z03JQmXEKLhiXt/g==", "6f688a38-7451-4417-a2a5-78935e75a955" });
        }
    }
}

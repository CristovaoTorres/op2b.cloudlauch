using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Server.Migrations
{
    public partial class vm_monitoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VirtualMachineMonitoring",
                table: "VirtualMachines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57f0d489-4f14-417d-8646-5a665d8bbfeb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5bb15b30-f5f9-4fe7-8c31-857e88ad49d8", "AQAAAAEAACcQAAAAEN2PTNINVKPovUHETlrrqf0oSXR0LjokLGp7VdmztW0si0m42fbbHlKA4LyqrMDO4g==", "5882bc12-d6c9-40d8-b08f-b886a4ef3337" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VirtualMachineMonitoring",
                table: "VirtualMachines");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57f0d489-4f14-417d-8646-5a665d8bbfeb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46b76a1d-4eb8-431a-9c3a-fcf0c64cc44f", "AQAAAAEAACcQAAAAECU//IJDssW4I6X60y+eS1gqUIb8J40JkyBsHGtnCR3XuQg3jZYbllwAfO+zc1+2og==", "e1e07354-111e-425c-b9d5-99887f366f7a" });
        }
    }
}

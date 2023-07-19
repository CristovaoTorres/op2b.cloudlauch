using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Server.Migrations
{
    public partial class addColumnCloudOnVirtualMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cloud",
                table: "VirtualMachines",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "AZURE");
            
            migrationBuilder.Sql("UPDATE VirtualMachines SET Cloud = 'AZURE'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cloud",
                table: "VirtualMachines");
        }
    }
}

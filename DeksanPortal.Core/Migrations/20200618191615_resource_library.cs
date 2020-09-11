using Microsoft.EntityFrameworkCore.Migrations;

namespace DeksanPortal.Core.Migrations
{
    public partial class resource_library : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceUrl",
                table: "Libraries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceUrl",
                table: "Libraries");
        }
    }
}

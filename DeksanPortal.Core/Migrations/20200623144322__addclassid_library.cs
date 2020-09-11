using Microsoft.EntityFrameworkCore.Migrations;

namespace DeksanPortal.Core.Migrations
{
    public partial class _addclassid_library : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "Libraries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Libraries");
        }
    }
}

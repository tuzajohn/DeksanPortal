using Microsoft.EntityFrameworkCore.Migrations;

namespace DeksanPortal.Core.Migrations
{
    public partial class _addclassid_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace DeksanPortal.Core.Migrations
{
    public partial class library_education_level : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationLevel",
                table: "Libraries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationLevel",
                table: "Libraries");
        }
    }
}

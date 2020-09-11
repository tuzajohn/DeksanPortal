using Microsoft.EntityFrameworkCore.Migrations;

namespace DeksanPortal.Core.Migrations
{
    public partial class thumbnail_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Libraries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Libraries");
        }
    }
}

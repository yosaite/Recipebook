using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipebook.Migrations
{
    public partial class UpdatedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Categories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Categories");

        }
    }
}

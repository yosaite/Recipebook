using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipebook.Migrations
{
    public partial class imageColumnRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Images",
                newName: "File");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "File",
                table: "Images",
                newName: "Path");
        }
    }
}

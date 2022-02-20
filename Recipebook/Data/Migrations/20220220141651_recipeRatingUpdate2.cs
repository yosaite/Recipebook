using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipebook.Migrations
{
    public partial class recipeRatingUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRatingValue",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "TotalUserRating",
                table: "Recipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "TotalRatingValue",
                table: "Recipes",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "TotalUserRating",
                table: "Recipes",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);
        }
    }
}

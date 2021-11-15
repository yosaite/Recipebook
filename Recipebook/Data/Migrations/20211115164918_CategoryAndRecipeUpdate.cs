using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipebook.Migrations
{
    public partial class CategoryAndRecipeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Categories");

            migrationBuilder.AddColumn<uint>(
                name: "CookingTime",
                table: "Recipes",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "PreparationTime",
                table: "Recipes",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Yields",
                table: "Recipes",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<ulong>(
                name: "ImageId",
                table: "Categories",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ImageId",
                table: "Categories",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ImageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CookingTime",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Yields",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Categories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

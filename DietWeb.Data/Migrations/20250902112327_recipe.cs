using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class recipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "FavoriteRecipe");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "FavoriteRecipe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRecipe_RecipeId",
                table: "FavoriteRecipe",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteRecipe_Recipes_RecipeId",
                table: "FavoriteRecipe",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteRecipe_Recipes_RecipeId",
                table: "FavoriteRecipe");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteRecipe_RecipeId",
                table: "FavoriteRecipe");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "FavoriteRecipe");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FavoriteRecipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

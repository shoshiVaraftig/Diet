using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriteRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "Recipes",
                newName: "Likes");

            migrationBuilder.AddColumn<bool>(
                name: "DairyFree",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GlutenFree",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReadyInMinutes",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Servings",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vegan",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Vegetarian",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FavoriteRecipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteRecipe_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRecipe_UserId",
                table: "FavoriteRecipe",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteRecipe");

            migrationBuilder.DropColumn(
                name: "DairyFree",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "GlutenFree",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ReadyInMinutes",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Servings",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Vegan",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Vegetarian",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Recipes",
                newName: "Calories");
        }
    }
}

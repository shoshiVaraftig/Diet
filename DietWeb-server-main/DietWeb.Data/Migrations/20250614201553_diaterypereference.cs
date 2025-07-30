using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class diaterypereference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DietaryPreference_UserId",
                table: "DietaryPreference");

            migrationBuilder.AlterColumn<string>(
                name: "Like",
                table: "DietaryPreference",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_DietaryPreference_UserId",
                table: "DietaryPreference",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DietaryPreference_UserId",
                table: "DietaryPreference");

            migrationBuilder.AlterColumn<int>(
                name: "Like",
                table: "DietaryPreference",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_DietaryPreference_UserId",
                table: "DietaryPreference",
                column: "UserId",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class assheight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Users");
        }
    }
}

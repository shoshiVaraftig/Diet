using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteTRackingWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "currentWeight",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currentWeight",
                table: "Users");
        }
    }
}

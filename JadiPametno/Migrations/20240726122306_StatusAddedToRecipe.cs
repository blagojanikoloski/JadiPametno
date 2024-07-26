using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JadiPametno.Migrations
{
    /// <inheritdoc />
    public partial class StatusAddedToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Recipe",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Recipe");
        }
    }
}

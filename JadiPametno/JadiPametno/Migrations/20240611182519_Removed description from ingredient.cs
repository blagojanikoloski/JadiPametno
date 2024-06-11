using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JadiPametno.Migrations
{
    /// <inheritdoc />
    public partial class Removeddescriptionfromingredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientDescription",
                table: "Ingredient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IngredientDescription",
                table: "Ingredient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

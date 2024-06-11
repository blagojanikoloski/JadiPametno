using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JadiPametno.Migrations
{
    /// <inheritdoc />
    public partial class Addidtorecipeehasingredientpair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RecipeHasIngredient",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeHasIngredient",
                table: "RecipeHasIngredient",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeHasIngredient",
                table: "RecipeHasIngredient");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecipeHasIngredient");
        }
    }
}

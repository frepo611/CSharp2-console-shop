using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsolesShoppen.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "ShoppingCartItems",
                newName: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ShoppingCartItems",
                newName: "ProductVariantId");
        }
    }
}

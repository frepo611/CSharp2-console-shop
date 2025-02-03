using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleShoppen.Migrations
{
    /// <inheritdoc />
    public partial class NewSChema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ShippingMethods",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProductVariantId");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ShippingMethods",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

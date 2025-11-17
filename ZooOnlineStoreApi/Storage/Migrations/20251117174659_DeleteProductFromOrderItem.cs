using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooOnlineStoreApi.Storage.Migrations
{
    /// <inheritdoc />
    public partial class DeleteProductFromOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Удаляем внешний ключ
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            // Удаляем индекс (если существует)
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems");

            // Удаляем столбец ProductId
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
           name: "ProductId",
           table: "OrderItems",
           type: "int",
           nullable: false,
           defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

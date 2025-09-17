using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditProductCustomProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCustomProperties_ProductId_Key",
                table: "ProductCustomProperties");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "ProductCustomProperties");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "productHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productHeaderId",
                table: "ProductCustomProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_productHeaders_ProductId",
                table: "productHeaders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomProperties_productHeaderId",
                table: "ProductCustomProperties",
                column: "productHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomProperties_ProductId_productHeaderId",
                table: "ProductCustomProperties",
                columns: new[] { "ProductId", "productHeaderId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCustomProperties_productHeaders_productHeaderId",
                table: "ProductCustomProperties",
                column: "productHeaderId",
                principalTable: "productHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productHeaders_Products_ProductId",
                table: "productHeaders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCustomProperties_productHeaders_productHeaderId",
                table: "ProductCustomProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_productHeaders_Products_ProductId",
                table: "productHeaders");

            migrationBuilder.DropIndex(
                name: "IX_productHeaders_ProductId",
                table: "productHeaders");

            migrationBuilder.DropIndex(
                name: "IX_ProductCustomProperties_productHeaderId",
                table: "ProductCustomProperties");

            migrationBuilder.DropIndex(
                name: "IX_ProductCustomProperties_ProductId_productHeaderId",
                table: "ProductCustomProperties");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "productHeaders");

            migrationBuilder.DropColumn(
                name: "productHeaderId",
                table: "ProductCustomProperties");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "ProductCustomProperties",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomProperties_ProductId_Key",
                table: "ProductCustomProperties",
                columns: new[] { "ProductId", "Key" },
                unique: true);
        }
    }
}

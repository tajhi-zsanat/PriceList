using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveModelfromProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Model_BrandId_ProductTypeId_ProductGroupId_CategoryId_SupplierId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId_ProductTypeId_ProductGroupId_CategoryId_SupplierId",
                table: "Products",
                columns: new[] { "BrandId", "ProductTypeId", "ProductGroupId", "CategoryId", "SupplierId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId_ProductTypeId_ProductGroupId_CategoryId_SupplierId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Model_BrandId_ProductTypeId_ProductGroupId_CategoryId_SupplierId",
                table: "Products",
                columns: new[] { "Model", "BrandId", "ProductTypeId", "ProductGroupId", "CategoryId", "SupplierId" },
                unique: true);
        }
    }
}

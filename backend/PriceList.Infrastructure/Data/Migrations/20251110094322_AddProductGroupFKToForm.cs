using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductGroupFKToForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId",
                table: "Forms");

            migrationBuilder.AddColumn<int>(
                name: "ProductGroupId",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId", "ProductGroupId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ProductGroupId",
                table: "Forms");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId" },
                unique: true);
        }
    }
}

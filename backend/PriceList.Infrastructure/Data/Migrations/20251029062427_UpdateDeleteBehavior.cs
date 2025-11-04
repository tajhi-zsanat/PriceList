using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Brands_BrandId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Categories_CategoryId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_CategoryId",
                table: "Forms");

            migrationBuilder.AlterColumn<int>(
                name: "MaxCols",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 15,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 8);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Brands_BrandId",
                table: "Forms",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Categories_BrandId",
                table: "Forms",
                column: "BrandId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Brands_BrandId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Categories_BrandId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms");

            migrationBuilder.AlterColumn<int>(
                name: "MaxCols",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 8,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 15);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CategoryId",
                table: "Forms",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Brands_BrandId",
                table: "Forms",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Categories_CategoryId",
                table: "Forms",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

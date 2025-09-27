using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierIdToTypeFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypeFeatures",
                table: "ProductTypeFeatures");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "ProductTypeFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypeFeatures",
                table: "ProductTypeFeatures",
                columns: new[] { "ProductTypeId", "FeatureId", "SupplierId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeFeatures_SupplierId",
                table: "ProductTypeFeatures",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypeFeatures_Suppliers_SupplierId",
                table: "ProductTypeFeatures",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypeFeatures_Suppliers_SupplierId",
                table: "ProductTypeFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypeFeatures",
                table: "ProductTypeFeatures");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypeFeatures_SupplierId",
                table: "ProductTypeFeatures");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ProductTypeFeatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypeFeatures",
                table: "ProductTypeFeatures",
                columns: new[] { "ProductTypeId", "FeatureId" });
        }
    }
}

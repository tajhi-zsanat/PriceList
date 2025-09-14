using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductTypeFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatureProductType_ProductFeatures_ProductFeaturesId",
                table: "ProductFeatureProductType");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatureProductType_ProductTypes_ProductTypesId",
                table: "ProductFeatureProductType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeatureProductType",
                table: "ProductFeatureProductType");

            migrationBuilder.RenameTable(
                name: "ProductFeatureProductType",
                newName: "ProductTypeFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeatureProductType_ProductTypesId",
                table: "ProductTypeFeatures",
                newName: "IX_ProductTypeFeatures_ProductTypesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypeFeatures",
                table: "ProductTypeFeatures",
                columns: new[] { "ProductFeaturesId", "ProductTypesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypeFeatures_ProductFeatures_ProductFeaturesId",
                table: "ProductTypeFeatures",
                column: "ProductFeaturesId",
                principalTable: "ProductFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypeFeatures_ProductTypes_ProductTypesId",
                table: "ProductTypeFeatures",
                column: "ProductTypesId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypeFeatures_ProductFeatures_ProductFeaturesId",
                table: "ProductTypeFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypeFeatures_ProductTypes_ProductTypesId",
                table: "ProductTypeFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypeFeatures",
                table: "ProductTypeFeatures");

            migrationBuilder.RenameTable(
                name: "ProductTypeFeatures",
                newName: "ProductFeatureProductType");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTypeFeatures_ProductTypesId",
                table: "ProductFeatureProductType",
                newName: "IX_ProductFeatureProductType_ProductTypesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeatureProductType",
                table: "ProductFeatureProductType",
                columns: new[] { "ProductFeaturesId", "ProductTypesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatureProductType_ProductFeatures_ProductFeaturesId",
                table: "ProductFeatureProductType",
                column: "ProductFeaturesId",
                principalTable: "ProductFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatureProductType_ProductTypes_ProductTypesId",
                table: "ProductFeatureProductType",
                column: "ProductTypesId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

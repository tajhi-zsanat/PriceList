using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdProductTypeFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypeFeature",
                table: "ProductTypeFeature");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypeFeature_ProductTypeId",
                table: "ProductTypeFeature");

            migrationBuilder.AddColumn<string>(
                name: "CreateDate",
                table: "ProductTypeFeature",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateAndTime",
                table: "ProductTypeFeature",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateTime",
                table: "ProductTypeFeature",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateDate",
                table: "ProductTypeFeature",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateAndTime",
                table: "ProductTypeFeature",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateTime",
                table: "ProductTypeFeature",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypeFeature",
                table: "ProductTypeFeature",
                columns: new[] { "ProductTypeId", "ProductFeatureId" });

            migrationBuilder.CreateTable(
                name: "ProductFeatureProductType",
                columns: table => new
                {
                    ProductFeaturesId = table.Column<int>(type: "int", nullable: false),
                    ProductTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureProductType", x => new { x.ProductFeaturesId, x.ProductTypesId });
                    table.ForeignKey(
                        name: "FK_ProductFeatureProductType_ProductFeatures_ProductFeaturesId",
                        column: x => x.ProductFeaturesId,
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFeatureProductType_ProductTypes_ProductTypesId",
                        column: x => x.ProductTypesId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeFeature_ProductFeatureId",
                table: "ProductTypeFeature",
                column: "ProductFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureProductType_ProductTypesId",
                table: "ProductFeatureProductType",
                column: "ProductTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductFeatureProductType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypeFeature",
                table: "ProductTypeFeature");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypeFeature_ProductFeatureId",
                table: "ProductTypeFeature");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductTypeFeature");

            migrationBuilder.DropColumn(
                name: "CreateDateAndTime",
                table: "ProductTypeFeature");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "ProductTypeFeature");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ProductTypeFeature");

            migrationBuilder.DropColumn(
                name: "UpdateDateAndTime",
                table: "ProductTypeFeature");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "ProductTypeFeature");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypeFeature",
                table: "ProductTypeFeature",
                columns: new[] { "ProductFeatureId", "ProductTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeFeature_ProductTypeId",
                table: "ProductTypeFeature",
                column: "ProductTypeId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTypeFormEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId_ProductTypeId",
                table: "Forms");

            migrationBuilder.CreateTable(
                name: "FormProductTypes",
                columns: table => new
                {
                    FormId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormProductTypes", x => new { x.FormId, x.ProductTypeId });
                    table.ForeignKey(
                        name: "FK_FormProductTypes_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormProductTypes_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId", "ProductGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormProductTypes_ProductTypeId",
                table: "FormProductTypes",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropTable(
                name: "FormProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId_ProductTypeId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId", "ProductGroupId", "ProductTypeId" },
                unique: true,
                filter: "[ProductTypeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

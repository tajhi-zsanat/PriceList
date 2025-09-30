using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFromEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductGroupId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    RowCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ColumnCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_BrandId",
                table: "Forms",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CategoryId_ProductGroupId_ProductTypeId_BrandId_SupplierId",
                table: "Forms",
                columns: new[] { "CategoryId", "ProductGroupId", "ProductTypeId", "BrandId", "SupplierId" });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId",
                table: "Forms",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forms");
        }
    }
}

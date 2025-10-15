using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormCol : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Forms_FormId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Forms_CategoryId_ProductGroupId_ProductTypeId_BrandId_SupplierId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ColumnCount",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "RowCount",
                table: "Forms");

            migrationBuilder.AlterColumn<string>(
                name: "FormTitle",
                table: "Forms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Forms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxCols",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 8);

            migrationBuilder.AddColumn<int>(
                name: "MinCols",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<int>(
                name: "Rows",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FormCells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    RowIndex = table.Column<int>(type: "int", nullable: false),
                    ColIndex = table.Column<int>(type: "int", nullable: false),
                    ItemTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormCells_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormColumnDefs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    WidthPx = table.Column<int>(type: "int", nullable: true),
                    FeatureId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormColumnDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormColumnDefs_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormCellFeatureValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CellId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    TextValue = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BoolValue = table.Column<bool>(type: "bit", nullable: true),
                    OptionKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ColorHex = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormCellFeatureValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormCellFeatureValues_FormCells_CellId",
                        column: x => x.CellId,
                        principalTable: "FormCells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CategoryId",
                table: "Forms",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_UpdateDateAndTime",
                table: "Forms",
                column: "UpdateDateAndTime");

            migrationBuilder.CreateIndex(
                name: "IX_FormCellFeatureValues_CellId_FeatureId",
                table: "FormCellFeatureValues",
                columns: new[] { "CellId", "FeatureId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_FormId_RowIndex_ColIndex",
                table: "FormCells",
                columns: new[] { "FormId", "RowIndex", "ColIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormColumnDefs_FormId_Index",
                table: "FormColumnDefs",
                columns: new[] { "FormId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormColumnDefs_FormId_Key",
                table: "FormColumnDefs",
                columns: new[] { "FormId", "Key" },
                unique: true);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Forms_FormId",
                table: "Products",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Forms_FormId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "FormCellFeatureValues");

            migrationBuilder.DropTable(
                name: "FormColumnDefs");

            migrationBuilder.DropTable(
                name: "FormCells");

            migrationBuilder.DropIndex(
                name: "IX_Forms_CategoryId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_UpdateDateAndTime",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "MaxCols",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "MinCols",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Rows",
                table: "Forms");

            migrationBuilder.AlterColumn<string>(
                name: "FormTitle",
                table: "Forms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ColumnCount",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowCount",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CategoryId_ProductGroupId_ProductTypeId_BrandId_SupplierId",
                table: "Forms",
                columns: new[] { "CategoryId", "ProductGroupId", "ProductTypeId", "BrandId", "SupplierId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Brands_BrandId",
                table: "Forms",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Categories_CategoryId",
                table: "Forms",
                column: "CategoryId",
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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Forms_FormId",
                table: "Products",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

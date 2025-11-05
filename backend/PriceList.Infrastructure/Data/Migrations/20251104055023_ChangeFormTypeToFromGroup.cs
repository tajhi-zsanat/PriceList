using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFormTypeToFromGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_ProductTypes_ProductTypeId",
                table: "FormProductTypes");

            migrationBuilder.DropTable(
                name: "FormRowProductTypes");

            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                table: "FormProductTypes",
                newName: "ProductGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_FormProductTypes_ProductTypeId",
                table: "FormProductTypes",
                newName: "IX_FormProductTypes_ProductGroupId");

            migrationBuilder.CreateTable(
                name: "FormRowProductGroups",
                columns: table => new
                {
                    FormRowId = table.Column<int>(type: "int", nullable: false),
                    ProductGroupId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormRowProductGroups", x => new { x.FormRowId, x.ProductGroupId });
                    table.ForeignKey(
                        name: "FK_FormRowProductGroups_FormRows_FormRowId",
                        column: x => x.FormRowId,
                        principalTable: "FormRows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormRowProductGroups_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormRowProductGroups_ProductGroupId",
                table: "FormRowProductGroups",
                column: "ProductGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_ProductGroups_ProductGroupId",
                table: "FormProductTypes",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_ProductGroups_ProductGroupId",
                table: "FormProductTypes");

            migrationBuilder.DropTable(
                name: "FormRowProductGroups");

            migrationBuilder.RenameColumn(
                name: "ProductGroupId",
                table: "FormProductTypes",
                newName: "ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FormProductTypes_ProductGroupId",
                table: "FormProductTypes",
                newName: "IX_FormProductTypes_ProductTypeId");

            migrationBuilder.CreateTable(
                name: "FormRowProductTypes",
                columns: table => new
                {
                    FormRowId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormRowProductTypes", x => new { x.FormRowId, x.ProductTypeId });
                    table.ForeignKey(
                        name: "FK_FormRowProductTypes_FormRows_FormRowId",
                        column: x => x.FormRowId,
                        principalTable: "FormRows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormRowProductTypes_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormRowProductTypes_ProductTypeId",
                table: "FormRowProductTypes",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_ProductTypes_ProductTypeId",
                table: "FormProductTypes",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

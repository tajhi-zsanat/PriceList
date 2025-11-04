using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FormRowProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_FormRows_FormRowId",
                table: "FormProductTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_FormProductTypes_FormRowId",
                table: "FormProductTypes");

            migrationBuilder.DropColumn(
                name: "FormRowId",
                table: "FormProductTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes",
                columns: new[] { "FormId", "ProductTypeId" });

            migrationBuilder.CreateTable(
                name: "FormRowProductTypes",
                columns: table => new
                {
                    FormRowId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
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
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes");

            migrationBuilder.DropTable(
                name: "FormRowProductTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes");

            migrationBuilder.AddColumn<int>(
                name: "FormRowId",
                table: "FormProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes",
                columns: new[] { "FormId", "ProductTypeId", "FormRowId" });

            migrationBuilder.CreateIndex(
                name: "IX_FormProductTypes_FormRowId",
                table: "FormProductTypes",
                column: "FormRowId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_FormRows_FormRowId",
                table: "FormProductTypes",
                column: "FormRowId",
                principalTable: "FormRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

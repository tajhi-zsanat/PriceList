using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorFormProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_FormRows_FormId",
                table: "FormProductTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes",
                columns: new[] { "FormId", "ProductTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_FormRows_FormId",
                table: "FormProductTypes",
                column: "FormId",
                principalTable: "FormRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

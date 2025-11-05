using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFormTypename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_ProductGroups_ProductGroupId",
                table: "FormProductTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes");

            migrationBuilder.RenameTable(
                name: "FormProductTypes",
                newName: "FormProductGroups");

            migrationBuilder.RenameIndex(
                name: "IX_FormProductTypes_ProductGroupId",
                table: "FormProductGroups",
                newName: "IX_FormProductGroups_ProductGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormProductGroups",
                table: "FormProductGroups",
                columns: new[] { "FormId", "ProductGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductGroups_Forms_FormId",
                table: "FormProductGroups",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductGroups_ProductGroups_ProductGroupId",
                table: "FormProductGroups",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductGroups_Forms_FormId",
                table: "FormProductGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_FormProductGroups_ProductGroups_ProductGroupId",
                table: "FormProductGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormProductGroups",
                table: "FormProductGroups");

            migrationBuilder.RenameTable(
                name: "FormProductGroups",
                newName: "FormProductTypes");

            migrationBuilder.RenameIndex(
                name: "IX_FormProductGroups_ProductGroupId",
                table: "FormProductTypes",
                newName: "IX_FormProductTypes_ProductGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormProductTypes",
                table: "FormProductTypes",
                columns: new[] { "FormId", "ProductGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_Forms_FormId",
                table: "FormProductTypes",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_ProductGroups_ProductGroupId",
                table: "FormProductTypes",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

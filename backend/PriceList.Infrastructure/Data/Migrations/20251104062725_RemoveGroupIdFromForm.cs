using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGroupIdFromForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRowProductGroups_FormRows_FormRowId",
                table: "FormRowProductGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_FormRowProductGroups_ProductGroups_ProductGroupId",
                table: "FormRowProductGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormRowProductGroups",
                table: "FormRowProductGroups");

            migrationBuilder.RenameTable(
                name: "FormRowProductGroups",
                newName: "FormRowGroups");

            migrationBuilder.RenameIndex(
                name: "IX_FormRowProductGroups_ProductGroupId",
                table: "FormRowGroups",
                newName: "IX_FormRowGroups_ProductGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormRowGroups",
                table: "FormRowGroups",
                columns: new[] { "FormRowId", "ProductGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FormRowGroups_FormRows_FormRowId",
                table: "FormRowGroups",
                column: "FormRowId",
                principalTable: "FormRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormRowGroups_ProductGroups_ProductGroupId",
                table: "FormRowGroups",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRowGroups_FormRows_FormRowId",
                table: "FormRowGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_FormRowGroups_ProductGroups_ProductGroupId",
                table: "FormRowGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormRowGroups",
                table: "FormRowGroups");

            migrationBuilder.RenameTable(
                name: "FormRowGroups",
                newName: "FormRowProductGroups");

            migrationBuilder.RenameIndex(
                name: "IX_FormRowGroups_ProductGroupId",
                table: "FormRowProductGroups",
                newName: "IX_FormRowProductGroups_ProductGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormRowProductGroups",
                table: "FormRowProductGroups",
                columns: new[] { "FormRowId", "ProductGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FormRowProductGroups_FormRows_FormRowId",
                table: "FormRowProductGroups",
                column: "FormRowId",
                principalTable: "FormRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormRowProductGroups_ProductGroups_ProductGroupId",
                table: "FormRowProductGroups",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

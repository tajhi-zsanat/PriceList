using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGroupFromForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGroupId",
                table: "Forms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId",
                table: "Forms");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGroupId",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId", "ProductGroupId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

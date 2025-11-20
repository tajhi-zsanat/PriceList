using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsereIdToForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId",
                table: "Forms");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "Forms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Forms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId",
                table: "Forms",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_UserId_BrandId_CategoryId_ProductGroupId",
                table: "Forms",
                columns: new[] { "UserId", "BrandId", "CategoryId", "ProductGroupId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_AspNetUsers_UserId",
                table: "Forms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_AspNetUsers_UserId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_UserId_BrandId_CategoryId_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Forms");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
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
                name: "FK_Forms_Suppliers_SupplierId",
                table: "Forms",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

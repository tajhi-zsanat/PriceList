using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_ProductGroups_ProductGroupId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_CategoryId",
                table: "ProductGroups");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "ProductGroups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "UX_ProductGroups_CategoryId_Name",
                table: "ProductGroups",
                columns: new[] { "CategoryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_ProductGroups_ProductGroupId",
                table: "ProductTypes",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_ProductGroups_ProductGroupId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "UX_ProductGroups_CategoryId_Name",
                table: "ProductGroups");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentPath",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "ProductGroups",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_CategoryId",
                table: "ProductGroups",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_ProductGroups_ProductGroupId",
                table: "ProductTypes",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

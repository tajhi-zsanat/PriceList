using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTypeIdFromForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "Forms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                table: "Forms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductTypes_ProductTypeId",
                table: "Forms",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");
        }
    }
}

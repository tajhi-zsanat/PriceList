using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormIdtoProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FormId",
                table: "Products",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Forms_FormId",
                table: "Products",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Forms_FormId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FormId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Products");
        }
    }
}

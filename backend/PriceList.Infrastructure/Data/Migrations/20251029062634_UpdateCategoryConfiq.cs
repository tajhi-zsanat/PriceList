using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryConfiq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Categories_BrandId",
                table: "Forms");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CategoryId",
                table: "Forms",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Categories_CategoryId",
                table: "Forms",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Categories_CategoryId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_CategoryId",
                table: "Forms");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Categories_BrandId",
                table: "Forms",
                column: "BrandId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

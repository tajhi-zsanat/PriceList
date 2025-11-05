using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGroupIdFromForm2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_ProductGroupId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ProductGroupId",
                table: "Forms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductGroupId",
                table: "Forms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_ProductGroups_ProductGroupId",
                table: "Forms",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id");
        }
    }
}

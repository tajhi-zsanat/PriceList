using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdtoProductHeaderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_productHeaders",
                table: "productHeaders");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "productHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productHeaders",
                table: "productHeaders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_productHeaders_BrandId_ProductTypeId",
                table: "productHeaders",
                columns: new[] { "BrandId", "ProductTypeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_productHeaders",
                table: "productHeaders");

            migrationBuilder.DropIndex(
                name: "IX_productHeaders_BrandId_ProductTypeId",
                table: "productHeaders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "productHeaders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productHeaders",
                table: "productHeaders",
                columns: new[] { "BrandId", "ProductTypeId" });
        }
    }
}

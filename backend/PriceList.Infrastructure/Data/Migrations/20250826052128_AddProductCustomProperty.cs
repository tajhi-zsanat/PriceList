using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCustomProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCustomProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCustomProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCustomProperties_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomProperties_ProductId_Key",
                table: "ProductCustomProperties",
                columns: new[] { "ProductId", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCustomProperties");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqeForNameInFeatureForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_DisplayOrder",
                table: "FormFeatures");

            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_DisplayOrder_Name",
                table: "FormFeatures",
                columns: new[] { "FormId", "DisplayOrder", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_DisplayOrder_Name",
                table: "FormFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_DisplayOrder",
                table: "FormFeatures",
                columns: new[] { "FormId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures",
                columns: new[] { "FormId", "Name" });
        }
    }
}

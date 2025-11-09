using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqeForNameInFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures",
                columns: new[] { "FormId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures",
                columns: new[] { "FormId", "Name" },
                unique: true);
        }
    }
}

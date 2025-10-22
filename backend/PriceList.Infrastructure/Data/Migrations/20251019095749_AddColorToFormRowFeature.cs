using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToFormRowFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_UpdateDateAndTime",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_FormCells_RowId",
                table: "FormCells");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "FormRowFeatures",
                newName: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId_ProductTypeId",
                table: "Forms",
                columns: new[] { "SupplierId", "BrandId", "CategoryId", "ProductGroupId", "ProductTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_RowId_ColIndex",
                table: "FormCells",
                columns: new[] { "RowId", "ColIndex" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forms_SupplierId_BrandId_CategoryId_ProductGroupId_ProductTypeId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_FormCells_RowId_ColIndex",
                table: "FormCells");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "FormRowFeatures",
                newName: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_SupplierId",
                table: "Forms",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_UpdateDateAndTime",
                table: "Forms",
                column: "UpdateDateAndTime");

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_RowId",
                table: "FormCells",
                column: "RowId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToRowIdFormCellInculudeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormCells_RowId_ColIndex",
                table: "FormCells");

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_RowId_ColIndex",
                table: "FormCells",
                columns: new[] { "RowId", "ColIndex" },
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Value" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormCells_RowId_ColIndex",
                table: "FormCells");

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_RowId_ColIndex",
                table: "FormCells",
                columns: new[] { "RowId", "ColIndex" },
                unique: true);
        }
    }
}

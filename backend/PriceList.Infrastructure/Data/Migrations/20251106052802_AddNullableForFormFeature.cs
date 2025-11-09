using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableForFormFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormRows_FormId_RowIndex_FormFeatureId",
                table: "FormRows");

            migrationBuilder.AlterColumn<int>(
                name: "FormFeatureId",
                table: "FormRows",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_FormRows_FormId_RowIndex_FormFeatureId",
                table: "FormRows",
                columns: new[] { "FormId", "RowIndex", "FormFeatureId" },
                unique: true,
                filter: "[FormFeatureId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormRows_FormId_RowIndex_FormFeatureId",
                table: "FormRows");

            migrationBuilder.AlterColumn<int>(
                name: "FormFeatureId",
                table: "FormRows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormRows_FormId_RowIndex_FormFeatureId",
                table: "FormRows",
                columns: new[] { "FormId", "RowIndex", "FormFeatureId" },
                unique: true);
        }
    }
}

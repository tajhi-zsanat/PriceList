using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormToFeatureManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId",
                table: "FormFeatures");

            migrationBuilder.AddColumn<int>(
                name: "FormId1",
                table: "FormFeatures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId1",
                table: "FormFeatures",
                column: "FormId1",
                unique: true,
                filter: "[FormId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFeatures_Forms_FormId1",
                table: "FormFeatures",
                column: "FormId1",
                principalTable: "Forms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFeatures_Forms_FormId1",
                table: "FormFeatures");

            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId1",
                table: "FormFeatures");

            migrationBuilder.DropColumn(
                name: "FormId1",
                table: "FormFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId",
                table: "FormFeatures",
                column: "FormId",
                unique: true);
        }
    }
}

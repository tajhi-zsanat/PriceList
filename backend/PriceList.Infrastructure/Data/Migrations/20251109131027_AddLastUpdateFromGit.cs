using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLastUpdateFromGit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFeatures_Forms_FormId",
                table: "FormFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFeatures_Forms_FormId",
                table: "FormFeatures",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows",
                column: "FormFeatureId",
                principalTable: "FormFeatures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFeatures_Forms_FormId",
                table: "FormFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFeatures_Forms_FormId",
                table: "FormFeatures",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows",
                column: "FormFeatureId",
                principalTable: "FormFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

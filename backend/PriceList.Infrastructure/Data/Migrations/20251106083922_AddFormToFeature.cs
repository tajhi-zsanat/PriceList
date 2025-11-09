using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormToFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FormFeatures",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "FormFeatures",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "FormFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId",
                table: "FormFeatures",
                column: "FormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_DisplayOrder",
                table: "FormFeatures",
                columns: new[] { "FormId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures",
                columns: new[] { "FormId", "Name" },
                unique: true);

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

            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId",
                table: "FormFeatures");

            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_DisplayOrder",
                table: "FormFeatures");

            migrationBuilder.DropIndex(
                name: "IX_FormFeatures_FormId_Name",
                table: "FormFeatures");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "FormFeatures");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FormFeatures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "FormFeatures",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows",
                column: "FormFeatureId",
                principalTable: "FormFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

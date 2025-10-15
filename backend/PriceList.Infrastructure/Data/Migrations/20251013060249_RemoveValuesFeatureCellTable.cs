using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveValuesFeatureCellTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoolValue",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "OptionKey",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "TextValue",
                table: "FormCellFeatureValues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BoolValue",
                table: "FormCellFeatureValues",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "FormCellFeatureValues",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "FormCellFeatureValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OptionKey",
                table: "FormCellFeatureValues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextValue",
                table: "FormCellFeatureValues",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}

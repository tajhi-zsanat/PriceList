using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddValueToCellTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "FormCells");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "FormCells");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "FormCells");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "FormCells");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FormCells",
                newName: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "FormCells",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "FormCells",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "FormCells",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "FormCells",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "FormCells",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}

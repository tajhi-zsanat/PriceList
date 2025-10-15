using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddnametoFromCells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "FormCells");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FormCells");

            migrationBuilder.RenameColumn(
                name: "ItemTitle",
                table: "FormCells",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FormCells",
                newName: "ItemTitle");

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "FormCells",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FormCells",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}

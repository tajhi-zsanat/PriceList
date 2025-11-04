using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormIdInFormProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormRowId",
                table: "FormProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_FormProductTypes_FormRows_FormId",
                table: "FormProductTypes",
                column: "FormId",
                principalTable: "FormRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormProductTypes_FormRows_FormId",
                table: "FormProductTypes");

            migrationBuilder.DropColumn(
                name: "FormRowId",
                table: "FormProductTypes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormRows_FormId_RowIndex",
                table: "FormRows");

            migrationBuilder.AddColumn<int>(
                name: "FormFeatureId",
                table: "FormRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FormFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFeatures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormRows_FormFeatureId",
                table: "FormRows",
                column: "FormFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRows_FormId_RowIndex_FormFeatureId",
                table: "FormRows",
                columns: new[] { "FormId", "RowIndex", "FormFeatureId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows",
                column: "FormFeatureId",
                principalTable: "FormFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRows_FormFeatures_FormFeatureId",
                table: "FormRows");

            migrationBuilder.DropTable(
                name: "FormFeatures");

            migrationBuilder.DropIndex(
                name: "IX_FormRows_FormFeatureId",
                table: "FormRows");

            migrationBuilder.DropIndex(
                name: "IX_FormRows_FormId_RowIndex_FormFeatureId",
                table: "FormRows");

            migrationBuilder.DropColumn(
                name: "FormFeatureId",
                table: "FormRows");

            migrationBuilder.CreateIndex(
                name: "IX_FormRows_FormId_RowIndex",
                table: "FormRows",
                columns: new[] { "FormId", "RowIndex" },
                unique: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMediatorFormRowTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormCells_Forms_FormId",
                table: "FormCells");

            migrationBuilder.DropIndex(
                name: "IX_FormCells_FormId_RowIndex_ColIndex",
                table: "FormCells");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "FormCells");

            migrationBuilder.RenameColumn(
                name: "RowIndex",
                table: "FormCells",
                newName: "RowId");

            migrationBuilder.CreateTable(
                name: "FormRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    RowIndex = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormRows_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormRowFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    UpdateDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    UpdateTime = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormRowFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormRowFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormRowFeatures_FormRows_RowId",
                        column: x => x.RowId,
                        principalTable: "FormRows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_RowId",
                table: "FormCells",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRowFeatures_FeatureId",
                table: "FormRowFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRowFeatures_RowId_FeatureId",
                table: "FormRowFeatures",
                columns: new[] { "RowId", "FeatureId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormRows_FormId_RowIndex",
                table: "FormRows",
                columns: new[] { "FormId", "RowIndex" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormCells_FormRows_RowId",
                table: "FormCells",
                column: "RowId",
                principalTable: "FormRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormCells_FormRows_RowId",
                table: "FormCells");

            migrationBuilder.DropTable(
                name: "FormRowFeatures");

            migrationBuilder.DropTable(
                name: "FormRows");

            migrationBuilder.DropIndex(
                name: "IX_FormCells_RowId",
                table: "FormCells");

            migrationBuilder.RenameColumn(
                name: "RowId",
                table: "FormCells",
                newName: "RowIndex");

            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "FormCells",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FormCells_FormId_RowIndex_ColIndex",
                table: "FormCells",
                columns: new[] { "FormId", "RowIndex", "ColIndex" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormCells_Forms_FormId",
                table: "FormCells",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

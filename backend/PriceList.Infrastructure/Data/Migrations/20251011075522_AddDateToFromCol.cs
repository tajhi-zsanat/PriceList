using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDateToFromCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateDate",
                table: "FormCellFeatureValues",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateAndTime",
                table: "FormCellFeatureValues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateTime",
                table: "FormCellFeatureValues",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateDate",
                table: "FormCellFeatureValues",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateAndTime",
                table: "FormCellFeatureValues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateTime",
                table: "FormCellFeatureValues",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "CreateDateAndTime",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "UpdateDateAndTime",
                table: "FormCellFeatureValues");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "FormCellFeatureValues");
        }
    }
}

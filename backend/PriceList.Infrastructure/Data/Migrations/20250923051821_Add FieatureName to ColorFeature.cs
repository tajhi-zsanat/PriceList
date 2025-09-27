using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieatureNametoColorFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeatureName",
                table: "ColorFeatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeatureName",
                table: "ColorFeatures");
        }
    }
}

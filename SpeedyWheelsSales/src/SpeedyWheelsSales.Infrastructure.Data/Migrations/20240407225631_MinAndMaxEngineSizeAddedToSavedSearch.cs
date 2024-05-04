using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedyWheelsSales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MinAndMaxEngineSizeAddedToSavedSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxEngineSize",
                table: "SavedSearches",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinEngineSize",
                table: "SavedSearches",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxEngineSize",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "MinEngineSize",
                table: "SavedSearches");
        }
    }
}

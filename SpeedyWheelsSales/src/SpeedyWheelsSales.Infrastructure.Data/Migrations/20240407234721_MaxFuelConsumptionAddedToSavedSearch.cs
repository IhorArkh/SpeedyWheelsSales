using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedyWheelsSales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MaxFuelConsumptionAddedToSavedSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxFuelConsumption",
                table: "SavedSearches",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFuelConsumption",
                table: "SavedSearches");
        }
    }
}

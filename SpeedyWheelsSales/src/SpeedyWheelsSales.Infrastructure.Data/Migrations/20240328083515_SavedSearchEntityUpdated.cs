using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedyWheelsSales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SavedSearchEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Filters",
                table: "SavedSearches",
                newName: "QueryString");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "SavedSearches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "SavedSearches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "SavedSearches",
                type: "int",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EngineType",
                table: "SavedSearches",
                type: "int",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxManufactureYear",
                table: "SavedSearches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxMileage",
                table: "SavedSearches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPrice",
                table: "SavedSearches",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinManufactureYear",
                table: "SavedSearches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinPrice",
                table: "SavedSearches",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "SavedSearches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Transmission",
                table: "SavedSearches",
                type: "int",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeOfDrive",
                table: "SavedSearches",
                type: "int",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "City",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "EngineType",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "MaxManufactureYear",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "MaxMileage",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "MinManufactureYear",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "SavedSearches");

            migrationBuilder.DropColumn(
                name: "TypeOfDrive",
                table: "SavedSearches");

            migrationBuilder.RenameColumn(
                name: "QueryString",
                table: "SavedSearches",
                newName: "Filters");
        }
    }
}

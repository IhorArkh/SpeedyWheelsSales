using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedyWheelsSales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelSplitedToBrandAndModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Cars",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Cars");
        }
    }
}

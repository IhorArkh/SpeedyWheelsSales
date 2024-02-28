using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedyWheelsSales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FavoutiteAdAndAppUserUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "EngineSize",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteAds_AppUserId",
                table: "FavouriteAds",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteAds_AppUsers_AppUserId",
                table: "FavouriteAds",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteAds_AppUsers_AppUserId",
                table: "FavouriteAds");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteAds_AppUserId",
                table: "FavouriteAds");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Cars",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "EngineSize",
                table: "Cars",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}

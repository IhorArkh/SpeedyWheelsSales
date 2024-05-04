using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedyWheelsSales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhotoPublicIdAddedToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPublicId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPublicId",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaHub.Data.Migrations
{
    public partial class MovieScreening_ScreeningType_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "MovieScreenings");

            migrationBuilder.AlterColumn<decimal>(
                name: "IsPremiere",
                table: "MovieScreenings",
                type: "decimal(19,5)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MovieScreenings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MovieScreenings");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPremiere",
                table: "MovieScreenings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,5)");

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "MovieScreenings",
                type: "decimal(19,5)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

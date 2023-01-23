using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaHub.Data.Migrations
{
    public partial class Movie_Screening_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieScreenings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    MovieId = table.Column<Guid>(nullable: true),
                    TheaterId = table.Column<Guid>(nullable: true),
                    TicketPrice = table.Column<decimal>(type: "decimal(19,5)", nullable: false),
                    IsPremiere = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieScreenings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieScreenings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieScreenings_MovieTheaters_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "MovieTheaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieScreenings_MovieId",
                table: "MovieScreenings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieScreenings_TheaterId",
                table: "MovieScreenings",
                column: "TheaterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieScreenings");
        }
    }
}

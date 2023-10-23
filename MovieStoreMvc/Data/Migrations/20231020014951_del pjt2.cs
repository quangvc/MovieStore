using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class delpjt2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieCountry");

            migrationBuilder.DropTable(
                name: "MovieFormat");

            migrationBuilder.CreateTable(
                name: "CountryMovie",
                columns: table => new
                {
                    countriesId = table.Column<int>(type: "int", nullable: false),
                    moviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryMovie", x => new { x.countriesId, x.moviesId });
                    table.ForeignKey(
                        name: "FK_CountryMovie_Country_countriesId",
                        column: x => x.countriesId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryMovie_Movie_moviesId",
                        column: x => x.moviesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormatMovie",
                columns: table => new
                {
                    formatsId = table.Column<int>(type: "int", nullable: false),
                    moviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatMovie", x => new { x.formatsId, x.moviesId });
                    table.ForeignKey(
                        name: "FK_FormatMovie_Format_formatsId",
                        column: x => x.formatsId,
                        principalTable: "Format",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormatMovie_Movie_moviesId",
                        column: x => x.moviesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryMovie_moviesId",
                table: "CountryMovie",
                column: "moviesId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatMovie_moviesId",
                table: "FormatMovie",
                column: "moviesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryMovie");

            migrationBuilder.DropTable(
                name: "FormatMovie");

            migrationBuilder.CreateTable(
                name: "MovieCountry",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCountry", x => new { x.MovieId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_MovieCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCountry_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieFormat",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    FormatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieFormat", x => new { x.MovieId, x.FormatId });
                    table.ForeignKey(
                        name: "FK_MovieFormat_Format_FormatId",
                        column: x => x.FormatId,
                        principalTable: "Format",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieFormat_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieCountry_CountryId",
                table: "MovieCountry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieFormat_FormatId",
                table: "MovieFormat",
                column: "FormatId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class addunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rating_Name",
                table: "Rating",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturer_Name",
                table: "Manufacturer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Name",
                table: "Genre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Format_Name",
                table: "Format",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "Country",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rating_Name",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturer_Name",
                table: "Manufacturer");

            migrationBuilder.DropIndex(
                name: "IX_Genre_Name",
                table: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_Format_Name",
                table: "Format");

            migrationBuilder.DropIndex(
                name: "IX_Country_Name",
                table: "Country");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class updateticket2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Showtimes_showtimesId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_showtimesId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ShowTimeId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "showtimesId",
                table: "Ticket");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShowTimeId",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "showtimesId",
                table: "Ticket",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_showtimesId",
                table: "Ticket",
                column: "showtimesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Showtimes_showtimesId",
                table: "Ticket",
                column: "showtimesId",
                principalTable: "Showtimes",
                principalColumn: "Id");
        }
    }
}

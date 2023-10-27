using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class updateticket4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ShowtimesId",
                table: "Ticket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ShowtimesId",
                table: "Ticket",
                column: "ShowtimesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Showtimes_ShowtimesId",
                table: "Ticket",
                column: "ShowtimesId",
                principalTable: "Showtimes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Showtimes_ShowtimesId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_ShowtimesId",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "ShowtimesId",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

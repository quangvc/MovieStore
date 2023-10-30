using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class editsurcharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surcharge_Seat_SeatId",
                table: "Surcharge");

            migrationBuilder.RenameColumn(
                name: "SeatId",
                table: "Surcharge",
                newName: "SeatTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Surcharge_SeatId",
                table: "Surcharge",
                newName: "IX_Surcharge_SeatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surcharge_SeatType_SeatTypeId",
                table: "Surcharge",
                column: "SeatTypeId",
                principalTable: "SeatType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surcharge_SeatType_SeatTypeId",
                table: "Surcharge");

            migrationBuilder.RenameColumn(
                name: "SeatTypeId",
                table: "Surcharge",
                newName: "SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_Surcharge_SeatTypeId",
                table: "Surcharge",
                newName: "IX_Surcharge_SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surcharge_Seat_SeatId",
                table: "Surcharge",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id");
        }
    }
}

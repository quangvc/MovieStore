using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class update1tomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Cinema_CinemaId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatType_SeatTypeId",
                table: "Seat");

            migrationBuilder.RenameColumn(
                name: "SeatTypeId",
                table: "Seat",
                newName: "seatTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_SeatTypeId",
                table: "Seat",
                newName: "IX_Seat_seatTypeId");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "Room",
                newName: "roomTypeId");

            migrationBuilder.RenameColumn(
                name: "CinemaId",
                table: "Room",
                newName: "cinemaId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_RoomTypeId",
                table: "Room",
                newName: "IX_Room_roomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_CinemaId",
                table: "Room",
                newName: "IX_Room_cinemaId");

            migrationBuilder.AlterColumn<int>(
                name: "seatTypeId",
                table: "Seat",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "roomTypeId",
                table: "Room",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "cinemaId",
                table: "Room",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Cinema_cinemaId",
                table: "Room",
                column: "cinemaId",
                principalTable: "Cinema",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_RoomType_roomTypeId",
                table: "Room",
                column: "roomTypeId",
                principalTable: "RoomType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatType_seatTypeId",
                table: "Seat",
                column: "seatTypeId",
                principalTable: "SeatType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Cinema_cinemaId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomType_roomTypeId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatType_seatTypeId",
                table: "Seat");

            migrationBuilder.RenameColumn(
                name: "seatTypeId",
                table: "Seat",
                newName: "SeatTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_seatTypeId",
                table: "Seat",
                newName: "IX_Seat_SeatTypeId");

            migrationBuilder.RenameColumn(
                name: "roomTypeId",
                table: "Room",
                newName: "RoomTypeId");

            migrationBuilder.RenameColumn(
                name: "cinemaId",
                table: "Room",
                newName: "CinemaId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_roomTypeId",
                table: "Room",
                newName: "IX_Room_RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_cinemaId",
                table: "Room",
                newName: "IX_Room_CinemaId");

            migrationBuilder.AlterColumn<int>(
                name: "SeatTypeId",
                table: "Seat",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CinemaId",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Cinema_CinemaId",
                table: "Room",
                column: "CinemaId",
                principalTable: "Cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room",
                column: "RoomTypeId",
                principalTable: "RoomType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatType_SeatTypeId",
                table: "Seat",
                column: "SeatTypeId",
                principalTable: "SeatType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

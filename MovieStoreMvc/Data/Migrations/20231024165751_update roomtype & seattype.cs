using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreMvc.Data.Migrations
{
    public partial class updateroomtypeseattype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatType");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "RoomType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "RoomType");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "SeatType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

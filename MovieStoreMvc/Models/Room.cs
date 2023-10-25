namespace MovieStoreMvc.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CinemaId { get; set; }
        public int RoomTypeId { get; set; }
        public int? TotalSeat { get; set; } = 0;
        public virtual Cinema cinema { get; set; }
        public virtual RoomType roomType { get; set; }
        public virtual List<Seat> Seats { get; set; }

    }
}

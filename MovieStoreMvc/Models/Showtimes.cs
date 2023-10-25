namespace MovieStoreMvc.Models
{
    public class Showtimes
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public int FormatId { get; set; }
        public DateTime StartTime { get; set; }
        public int Price { get; set; }
        public int TicketSold { get; set; } = 0;
        public int TotalPrice { get; set; } = 0;
        public virtual Movie movie { get; set; }
        public virtual Room room { get; set; }
        public virtual Format format { get; set; }
    }
}

namespace MovieStoreMvc.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public int ShowTimeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public string Status { get; set;}
        public DateTime CreatedDate { get; set;}
        public DateTime UpdatedDate { get; set;} = DateTime.Now;
        public virtual Showtimes showtimes { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStoreMvc.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public int ShowtimesId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public DateTime CreatedDate { get; set;}
        public DateTime UpdatedDate { get; set;} = DateTime.Now;
        public virtual Seat seat { get; set; }
        public virtual Showtimes showtimes { get; set; }

    }
}

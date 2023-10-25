using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStoreMvc.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public int RoomId { get; set; } 
        public int SeatTypeId { get; set; }
        public virtual Room room { get; set; }
        public virtual SeatType seatType { get; set; }

    }
}

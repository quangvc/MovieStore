namespace MovieStoreMvc.Models
{
    public class Surcharge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int? FormatId { get; set; }
        public int? RoomId { get; set; }
        public int? SeatTypeId { get; set; }
        public virtual Room? room { get; set; }
        public virtual SeatType? seat { get; set; }
        public virtual Format format { get; set; }


    }
}

using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [RegularExpression("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$")]
        public string Phone { get; set; }
        public int? TotalRoom { get; set; } = 0;
        public int? TotalSeat { get; set; } = 0;

    }
}

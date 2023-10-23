using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }
        [StringLength(60)]
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

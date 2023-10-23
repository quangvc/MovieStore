using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models
{
    public class Rating
    {
        public int Id { get; set; }
        [StringLength(255)]
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

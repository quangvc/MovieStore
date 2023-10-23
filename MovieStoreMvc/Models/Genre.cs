using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(60)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Movie>? movies { get; set; }
    }
}

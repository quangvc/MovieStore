using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Movie> movies { get; set; }
    }
}

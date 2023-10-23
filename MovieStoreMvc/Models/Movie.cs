using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStoreMvc.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile Poster { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        [StringLength(255)]
        public string Director { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Cast { get; set; } = string.Empty;
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Running Time")]
        public int RunningTime { get; set; }
        [StringLength(255)]
        public string Language { get; set; } = string.Empty;
        [RegularExpression(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$", ErrorMessage = "Vui lòng nhập link youtube!")]
        public string Trailer { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Manufacturer")]
        public int ManuId { get; set; }
        [Display(Name = "Rating")]
        public int RatingId { get; set; }
        [DataType(DataType.Currency)]
        public decimal? TotalRevenue { get; set; } = decimal.Zero;
        public virtual Rating? rating { get; set; }
        public virtual Manufacturer? manufacturer { get; set; }
        public virtual ICollection<Country> countries { get; set; }
        public virtual ICollection<Format> formats { get; set; }
        public virtual ICollection<Genre> genres { get; set; }
    }
}

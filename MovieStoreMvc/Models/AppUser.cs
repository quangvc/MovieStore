using Microsoft.AspNetCore.Identity;
using MovieStoreMvc.Constants;

namespace MovieStoreMvc.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

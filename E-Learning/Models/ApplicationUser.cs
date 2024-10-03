using Microsoft.AspNetCore.Identity;

namespace E_Learning.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public DateTime DateJoined { get; set; }
    }
}

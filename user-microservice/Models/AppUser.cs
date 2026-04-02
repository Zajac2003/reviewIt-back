using Microsoft.AspNetCore.Identity;

namespace user_microservice.Models
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}

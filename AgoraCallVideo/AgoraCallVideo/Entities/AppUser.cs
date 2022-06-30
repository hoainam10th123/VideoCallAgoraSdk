using Microsoft.AspNetCore.Identity;

namespace AgoraCallVideo.Entities
{
    public class AppUser: IdentityUser<int>
    {
        public string? DisplayName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}

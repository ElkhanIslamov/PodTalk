using Microsoft.AspNetCore.Identity;

namespace PodTalk.DataContext.Entities
{
    public class AppUser : IdentityUser
    {
        public required string FullName { get; set; }
    }
}

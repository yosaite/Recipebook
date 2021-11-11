using Microsoft.AspNetCore.Identity;

namespace Recipebook.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string AvatarPath { get; set; }
    }
}

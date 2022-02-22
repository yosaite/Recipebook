using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Recipebook.Models
{
    public class User: IdentityUser
    {
        public Image? Image { get; set;}
        [ForeignKey("Image")]
        public ulong? ImageId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class Comment
    {
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        
        public virtual Recipe Recipe { get; set; }
        [ForeignKey("Recipe")]
        public ulong RecipeId { get; set; }
        
        public string Content { get; set; }
    }
}
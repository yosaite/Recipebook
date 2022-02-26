using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class Favorite
    {
        [Key]
        public ulong Id { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public virtual Recipe Recipe { get; set; }
        [ForeignKey("Recipe")]
        public ulong RecipeId { get; set; }
    }
}
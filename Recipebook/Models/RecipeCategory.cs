using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class RecipeCategory
    {
        [ForeignKey("Recipe")]
        public ulong RecipeId { get; set; }
        public virtual Recipe? Recipe { get; set; }
        [ForeignKey("Category")]
        public ulong CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
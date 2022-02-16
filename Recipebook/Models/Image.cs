using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class Image
    {
        [Key]
        public ulong Id { get; set; }
        public string Path { get; set; }
        [ForeignKey("Recipe")]
        public ulong? RecipeId { get; set; }
    }
}

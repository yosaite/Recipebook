using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Recipebook.Models
{
    public class Image
    {
        [Key]
        public ulong Id { get; set; }
        public string File { get; set; }
        [ForeignKey("Recipe")]
        public ulong? RecipeId { get; set; }
        
        [NotMapped]
        public string WebPath => $"/{Setup.ImagesFolder}/{File}";
    }
}

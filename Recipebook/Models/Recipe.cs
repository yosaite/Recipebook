using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class Recipe
    {
        [Key]
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Directions { get; set; }
        public virtual Category Category { get; set; }
        [ForeignKey("Category")]
        public ulong CategoryId { get; set; }
        public List<Image> Images { get; set; }

    }
}

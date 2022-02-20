using System;
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
        public List<string> Ingredients { get; set; }
        public string Directions { get; set; }
        public string Description { get; set; }
        public uint PreparationTime { get; set; }
        public uint Yields { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public List<Image> Images { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("AspNetUsers")]
        public string ApplicationUserId { get; set; }
        public virtual IList<RecipeUserRate> Rates { get; set; }
        public Recipe()
        {
            Created = DateTime.Now;
        }
    }
}

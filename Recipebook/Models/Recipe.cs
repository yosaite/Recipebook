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
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public List<Image> Images { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public Recipe()
        {
            Created = DateTime.Now;
        }
    }
}

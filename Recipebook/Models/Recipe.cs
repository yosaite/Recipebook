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
        public virtual Category Category { get; set; }
        [ForeignKey("Category")]
        public ulong CategoryId { get; set; }
        public List<Image> Images { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("AspNetUsers")]
        public string ApplicationUserId { get; set; }
        public ulong TotalRatingValue { get; set; }
        public ulong TotalUserRating { get; set; }
        [NotMapped]
        public float Rating { get => TotalRatingValue / TotalUserRating; }
        public Recipe()
        {
            Created = DateTime.Now;
        }

    }
}

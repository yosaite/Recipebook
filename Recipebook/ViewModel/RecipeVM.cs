using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Recipebook.Models;

namespace Recipebook.ViewModel
{
    public class RecipeVM
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Ingredients { get; set; }
        public string Directions { get; set; }
        public string Description { get; set; }
        public uint PreparationTime { get; set; }
        public uint Yields { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Image> Images { get; set; }
        public DateTime Created { get; set; }
        public User User { get; set; }
        public double UserRate { get; set; } = 0;
        public double Rate { get; set; }
        public bool Favorite { get; set; }
    }
}
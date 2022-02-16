using Recipebook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Recipebook.ViewModel
{
    public class RecipeVM
    {
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Nazwa musi mieć pomiędzy {2} - {1} znaków.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = "Sposób przygotowania musi mieć pomiędzy {2} - {1} znaków.")]
        [DataType(DataType.Text)]
        public string Directions { get; set; }
        [Required]
        [StringLength(800, MinimumLength = 20, ErrorMessage = "Opis musi mieć pomiędzy {2} - {1} znaków.")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required]
        public uint PreparationTime { get; set; }
        [Required]
        public uint CookingTime { get; set; }
        [Required]
        public uint Yields { get; set; }
        [Required]
        public ulong CategoryId { get; set; }
        public List<Image> Images { get; set; }
        
        public List<IFormFile> Files { get; set; }
        public DateTime Created { get; set; }
        public string ApplicationUserId { get; set; }
        public RecipeVM()
        {
            Created = DateTime.Now;
        }
    }
}

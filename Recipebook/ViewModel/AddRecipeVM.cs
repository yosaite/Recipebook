using Recipebook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Recipebook.ViewModel
{
    public class AddRecipeVM
    {
        public ulong Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "Nazwa musi mieć pomiędzy {2} - {1} znaków.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        public List<string> Ingredients { get; set; }
        [Required]
        [StringLength(8000, MinimumLength = 20, ErrorMessage = "Sposób przygotowania musi mieć pomiędzy {2} - {1} znaków.")]
        [DataType(DataType.Text)]
        public string Directions { get; set; }
        [Required]
        [StringLength(8000, MinimumLength = 20, ErrorMessage = "Opis musi mieć pomiędzy {2} - {1} znaków.")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required]
        public uint PreparationTime { get; set; }
        [Required]
        public uint Yields { get; set; }
        [Required(ErrorMessage = "Wymagana przynajmniej 1 kategoria")]
        public List<ulong> SelectedCategoriesIds { get; set; } 
        public IEnumerable<SelectListItem> CategoriesList { get; set; }
        public List<Image> Images { get; set; }
        public List<IFormFile> Files { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public AddRecipeVM()
        {
            Created = DateTime.Now;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Recipebook.Models;

namespace Recipebook.ViewModel
{
    public class AddCategoryVM
    {
        public ulong Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nazwa musi mieć pomiędzy {2} - {1} znaków.")]
        public string Name { get; set;}
        
        public IFormFile File { get; set; }
        [ValidateNever]
        
        public Image Image { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Recipebook.Models;

namespace Recipebook.ViewModel
{
    public class AddCategoryVM
    {
        public ulong Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set;}
        public IFormFile File { get; set; }
        
        public Image? Image { get; set; }
    }
}
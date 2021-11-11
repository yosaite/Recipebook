using System.ComponentModel.DataAnnotations;

namespace Recipebook.Models
{
    public class Image
    {
        [Key]
        public ulong Id { get; set; }  
        public string Path { get; set; }
    }
}

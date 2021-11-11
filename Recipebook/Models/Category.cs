using System.ComponentModel.DataAnnotations;

namespace Recipebook.Models
{
    public class Category
    {
        [Key]
        public ulong Id { get; set;}
        public string Name { get; set;}
    }
}

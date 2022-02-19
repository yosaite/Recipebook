using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class Category
    {
        [Key]
        public ulong Id { get; set;}
        public string Name { get; set;}
        public Image? Image { get; set;}
        [ForeignKey("Image")]
        public ulong? ImageId { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}

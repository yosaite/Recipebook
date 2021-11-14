﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipebook.Models
{
    public class Category
    {
        [Key]
        public ulong Id { get; set;}
        public string Name { get; set;}
        public string ImagePath { get; set; }
    }
}

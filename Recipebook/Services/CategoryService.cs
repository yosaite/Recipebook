using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Models;
using System.Collections.Generic;
using System.Linq;
using Recipebook.Interfaces;

namespace Recipebook.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Category> GetCategories()
        {
            return dbContext.Categories.Include(m => m.Image).OrderBy(c => c.Name).ToList();
        }
    }
}

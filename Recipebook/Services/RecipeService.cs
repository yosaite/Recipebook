using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Models;
using System.Collections.Generic;
using System.Linq;

namespace Recipebook.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly ApplicationDbContext dbContext;

        public RecipeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Recipe> GetRecipes()
        {
            var list = dbContext.Recipes.Include(m => m.Images).ToList();
            return list;
        }

        public Recipe GetRecipe(ulong id)
        {
           return dbContext.Recipes.Where(r => r.Id == id).Include(m => m.Images).Include(m => m.ApplicationUser).FirstOrDefault();
        }

        public List<Recipe> GetRecipes(ulong categoryId)
        {
            if(categoryId == 0)
            {
                return GetRecipes();
            }
            return dbContext.Recipes.Include(m => m.Images).Where(c => c.CategoryId == categoryId).ToList();
        }

    }
}

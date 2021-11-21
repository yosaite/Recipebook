using Recipebook.Models;
using Recipebook.ViewModel;
using System.Collections.Generic;

namespace Recipebook.Services
{
    public interface IRecipeService
    {
        public List<Recipe> GetRecipes();
        public Recipe GetRecipe(ulong id);
        public List<Recipe> GetRecipes(ulong categoryId);
        public List<Recipe> GetRecipes(string userId);
        public void AddRecipe(RecipeVM recipeVM);
    }
}

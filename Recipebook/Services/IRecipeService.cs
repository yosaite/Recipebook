using Recipebook.Models;
using Recipebook.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipebook.Services
{
    public interface IRecipeService
    {
        Task<List<Recipe>> GetRecipes();
        Task<Recipe> GetRecipe(ulong id);
        Task<List<Recipe>> GetRecipes(ulong categoryId);
        Task<List<Recipe>> GetRecipes(string userId);
        Task<RecipeVM> AddRecipe(RecipeVM recipeVM);
        Task DeleteRecipe(ulong id);
    }
}

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
        Task<Recipe> AddRecipe(RecipeVM recipeVm);
        Task DeleteRecipe(ulong id);
        Task<Recipe> EditRecipe(RecipeVM recipeVm);
        Task<bool> Rate(string userId, ulong recipeId, int rate);
    }
}

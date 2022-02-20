using Recipebook.Models;
using Recipebook.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipebook.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeVM>> GetRecipesVM();
        Task<RecipeVM> GetRecipeVM(ulong id);
        Task<List<RecipeVM>> GetRecipesVM(ulong categoryId);
        Task<List<RecipeVM>> GetRecipesVM(string userId);
        Task<Recipe> AddRecipe(AddRecipeVM addRecipeVM);
        Task DeleteRecipe(ulong id);
        Task<Recipe> EditRecipe(AddRecipeVM addRecipeVM);
        Task<bool> Rate(string userId, ulong recipeId, int rate);
        Task<double> GetUserRate(string userId, ulong recipeId);
        Task<Recipe> GetRecipe(ulong id);
        Task<double> GetRecipeRate(ulong recipeId);
    }
}

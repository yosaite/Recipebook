using System.Collections.Generic;
using System.Threading.Tasks;
using Recipebook.Models;
using Recipebook.ViewModel;

namespace Recipebook.Interfaces
{
    public interface IRecipeService
    {
        Task<List<RecipeVM>> GetRecipesVM(int page = 0, RecipeSort sort = RecipeSort.Newest);
        Task<int> GetRecipesVMCount();
        Task<RecipeVM> GetRecipeVM(ulong id);
        Task<List<RecipeVM>> GetRecipesVM(ulong categoryId,int page = 0, RecipeSort sort = RecipeSort.Newest);
        Task<int> GetRecipesVMCount(ulong categoryId);
        Task<List<RecipeVM>> GetRecipesVM(string userId, int page = 1, RecipeSort sort = RecipeSort.Newest);
        Task<int> GetRecipesVMCount(string userId);
        Task<Recipe> AddRecipe(AddRecipeVM addRecipeVM);
        Task DeleteRecipe(ulong id);
        Task<Recipe> EditRecipe(AddRecipeVM addRecipeVM);
        Task<bool> Rate(string userId, ulong recipeId, int rate);
        Task<double> GetUserRate(string userId, ulong recipeId);
        Task<Recipe> GetRecipe(ulong id);
        Task<double> GetRecipeRate(ulong recipeId);
    }
}

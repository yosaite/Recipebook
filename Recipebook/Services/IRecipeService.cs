using Recipebook.Models;
using System.Collections.Generic;

namespace Recipebook.Services
{
    public interface IRecipeService
    {
        List<Recipe> GetRecipe();
    }
}

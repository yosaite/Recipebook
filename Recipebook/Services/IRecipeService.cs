using Recipebook.Models;
using System.Collections.Generic;

namespace Recipebook.Services
{
    public interface IRecipeService
    {
        public List<Recipe> GetRecipe();
        public Recipe GetRecipe(ulong id);
    }
}

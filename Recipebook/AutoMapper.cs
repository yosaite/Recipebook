using AutoMapper;
using Recipebook.Models;
using Recipebook.ViewModel;

namespace Recipebook
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<RecipeVM, Recipe>();
            CreateMap<Recipe, RecipeVM>();
        }
    }
}

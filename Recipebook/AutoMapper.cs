using AutoMapper;
using Recipebook.Models;
using Recipebook.ViewModel;

namespace Recipebook
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<AddRecipeVM, Recipe>();
            CreateMap<Recipe, AddRecipeVM>();
            CreateMap<Recipe, RecipeVM>();
            CreateMap<RecipeVM, Recipe>();
            CreateMap<Comment, CommentVM>().ForMember(x => x.UserName, y => y.MapFrom(z => z.User.UserName));
        }
    }
}

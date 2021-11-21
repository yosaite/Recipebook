using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Models;
using Recipebook.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Recipebook.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public RecipeService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public List<Recipe> GetRecipes()
        {
            var list = dbContext.Recipes.Include(m => m.Images).OrderByDescending(m => m.TotalRatingValue/m.TotalUserRating).ToList();
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

        public List<Recipe> GetRecipes(string userId)
        { 
            if(userId is null){
                return GetRecipes();
            }
            return dbContext.Recipes.Include(m => m.Images).Include(m => m.ApplicationUser).Where(c => c.ApplicationUserId == userId).ToList();

        }

        public void AddRecipe(RecipeVM recipeVM)
        {
            var recipe = mapper.Map<Recipe>(recipeVM);
            dbContext.Recipes.Add(recipe);
            dbContext.SaveChanges();
        }

    }
}

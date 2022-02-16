using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Models;
using Recipebook.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Recipebook.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public RecipeService(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _environment = environment;
        }
        public async Task<List<Recipe>> GetRecipes()
        {
            var list = await _dbContext.Recipes.Include(m => m.Images).OrderByDescending(m => m.TotalRatingValue/m.TotalUserRating).ToListAsync();
            return list;
        }

        public async Task<Recipe> GetRecipe(ulong id)
        {
           return await _dbContext.Recipes.Where(r => r.Id == id).Include(m => m.Images).Include(m => m.ApplicationUser).FirstOrDefaultAsync();
        }

        public async Task<List<Recipe>> GetRecipes(ulong categoryId)
        {
            if(categoryId == 0)
            {
                return await GetRecipes();
            }

            return await _dbContext.RecipesCategories
                .Include(m => m.Recipe)
                .ThenInclude(i => i .Images)
                .Where(c => c.CategoryId == categoryId)
                .Select(p => p.Recipe).ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipes(string userId)
        { 
            if(userId is null){
                return await GetRecipes();
            }
            return await _dbContext.Recipes.Include(m => m.Images).Include(m => m.ApplicationUser).Where(c => c.ApplicationUserId == userId).ToListAsync();

        }

        public async Task<RecipeVM> AddRecipe(RecipeVM recipeVM)
        {
            var recipe = _mapper.Map<Recipe>(recipeVM);
            if (recipeVM.Files != null)
            {
                var path = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach (var file in recipeVM.Files.Where(img=>img.Length > 0))
                {
                    var img = new Image(){Path = $"{Guid.NewGuid()}.{file.FileName.Split('.').Last()}"};
                    await using (var stream = File.Create(Path.Combine(path,img.Path)))
                    {
                        await file.CopyToAsync(stream);
                    }
                    recipe.Images.Add(img);
                }
            }

            await _dbContext.Recipes.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();

            if (recipeVM.SelectedCategoriesIds == null) return recipeVM;
            var categories = recipeVM.SelectedCategoriesIds.ConvertAll(m => new RecipeCategory()
            {
                RecipeId = recipe.Id,
                CategoryId = m
            });
            
            await _dbContext.RecipesCategories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();

            return recipeVM;
        }

    }
}

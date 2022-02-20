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
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;

        public RecipeService(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment environment,
            IFileService fileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _environment = environment;
            _fileService = fileService;
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            var list = await _dbContext.Recipes.Include(m => m.Images).ToListAsync();
            return list;
        }

        public async Task<Recipe> GetRecipe(ulong id)
        {
            var recipe = await _dbContext.Recipes
                .Where(r => r.Id == id)
                .Include(m => m.Images)
                .Include(m => m.ApplicationUser)
                .Include(m => m.Categories)
                .FirstOrDefaultAsync();

            var rates = Convert.ToSingle(await _dbContext.RecipeUserRates.Where(i => i.RecipeId == recipe.Id)
                .SumAsync(c => c.Rate));

            var count = Convert.ToSingle(await _dbContext.RecipeUserRates.CountAsync(i => i.RecipeId == recipe.Id));
            recipe.Rate = count == 0 ? 0 : rates / count;

            return recipe;
        }

        public async Task<List<Recipe>> GetRecipes(ulong categoryId)
        {
            if (categoryId == 0)
            {
                return await GetRecipes();
            }

            return await _dbContext.Categories
                .Include(c => c.Recipes)
                .ThenInclude(m => m.Images)
                .Where(c => c.Id == categoryId)
                .SelectMany(c => c.Recipes).ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipes(string userId)
        {
            if (userId is null)
            {
                return await GetRecipes();
            }

            return await _dbContext.Recipes.Include(m => m.Images).Include(m => m.ApplicationUser)
                .Where(c => c.ApplicationUserId == userId).ToListAsync();
        }

        public async Task<Recipe> AddRecipe(RecipeVM recipeVm)
        {
            var recipe = _mapper.Map<Recipe>(recipeVm);

            recipe.Images = await _fileService.SaveImages(recipeVm.Files);
            recipe.Categories = await _dbContext.Categories
                .Where(c => recipeVm.SelectedCategoriesIds.Contains(c.Id))
                .ToListAsync();
            await _dbContext.Recipes.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();
            return recipe;
        }

        public async Task DeleteRecipe(ulong id)
        {
            var recipe = await _dbContext.Recipes.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (recipe != null)
            {
                _dbContext.Recipes.Remove(recipe);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Recipe> EditRecipe(RecipeVM recipeVm)
        {
            var recipe = _mapper.Map<Recipe>(recipeVm);
            var dbRecipe = await _dbContext.Recipes.Where(r => r.Id == recipe.Id)
                .Include(r => r.Categories)
                .Include(r => r.Images)
                .FirstOrDefaultAsync();
            if (dbRecipe == null) return new Recipe();

            dbRecipe.Name = recipe.Name;
            dbRecipe.Categories = await _dbContext.Categories.Where(r => recipeVm.SelectedCategoriesIds.Contains(r.Id))
                .ToListAsync();
            dbRecipe.PreparationTime = recipe.PreparationTime;
            dbRecipe.Yields = recipe.Yields;
            dbRecipe.Description = recipe.Description;
            dbRecipe.Ingredients = recipe.Ingredients;
            dbRecipe.Directions = recipe.Directions;
            dbRecipe.Images.RemoveAll(i => !recipe.Images.Select(c => c.Id).ToList().Contains(i.Id));

            if (recipeVm.Files != null && recipeVm.Files.Any())
            {
                var savedImages = await _fileService.SaveImages(recipeVm.Files);
                dbRecipe.Images.AddRange(savedImages);
            }

            _dbContext.Recipes.Update(dbRecipe);
            await _dbContext.SaveChangesAsync();
            return dbRecipe;
        }

        public async Task<bool> Rate(string userId, ulong recipeId, int rate)
        {
            if (rate is > 5 or < 0) return false;
            var recipe = await _dbContext.Recipes.Where(m => m.Id == recipeId).FirstOrDefaultAsync();
            if (recipe == null) return false;

            var user = await _dbContext.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;

            _dbContext.RecipeUserRates.Update(new RecipeUserRate()
            {
                User = user,
                Recipe = recipe,
                Rate = rate
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
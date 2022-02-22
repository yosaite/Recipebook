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

   
        public async Task<Recipe> GetRecipe(ulong id)
        {
            var recipe = await _dbContext.Recipes
                .Where(r => r.Id == id)
                .Include(m => m.Images)
                .Include(m => m.ApplicationUser)
                .Include(m => m.Categories)
                .Include(c=>c.Comments).ThenInclude(z=>z.User)
                .FirstOrDefaultAsync();
            return recipe;
        }
        public async Task<RecipeVM> GetRecipeVM(ulong id)
        {
            var recipe = await _dbContext.Recipes
                .Where(r => r.Id == id)
                .Include(m => m.Images)
                .Include(m => m.ApplicationUser)
                .Include(m => m.Categories)
                .Include(c=>c.Comments).ThenInclude(z=>z.User)
                .Select(n=>new RecipeVM()
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Ingredients = n.Ingredients,
                    Directions = n.Directions,
                    PreparationTime = n.PreparationTime,
                    Yields = n.Yields,
                    Categories = n.Categories,
                    Images = n.Images,
                    Created = n.Created,
                    ApplicationUser = n.ApplicationUser,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Rate))/Convert.ToDouble(n.Rates.Count),1),
                    Comments = _mapper.Map<ICollection<CommentVM>>(n.Comments)
                })
                .FirstOrDefaultAsync();
            return recipe;
        }
        
        // Get all recipes
        public async Task<List<RecipeVM>> GetRecipesVM()
        {
            return await _dbContext.Recipes.Include(m => m.Images)
                .Include(m => m.ApplicationUser)
                .Select(n=>new RecipeVM()
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Images = n.Images,
                    Created = n.Created,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Rate))/Convert.ToDouble(n.Rates.Count),1)
                    
                })
                .ToListAsync();
        }
        // Get category recipes
        public async Task<List<RecipeVM>> GetRecipesVM(ulong categoryId)
        {
            if (categoryId == 0)
            {
                return await GetRecipesVM();
            }

            return await _dbContext.Categories.Where(z=>z.Id == categoryId)
                .SelectMany(c => c.Recipes).Include(d=>d.Images)
                .Include(t=>t.Rates)
                .Select(n=>new RecipeVM()
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Images = n.Images,
                    Created = n.Created,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Rate))/Convert.ToDouble(n.Rates.Count),1)
                    
                })
                .ToListAsync();
        }
        // Get user recipes
        public async Task<List<RecipeVM>> GetRecipesVM(string userId)
        {
            if (userId is null)
            {
                return await GetRecipesVM();
            }

            return await _dbContext.Recipes.Include(m => m.Images)
                .Include(m => m.ApplicationUser)
                .Where(c => c.ApplicationUserId == userId)
                .Select(n=>new RecipeVM()
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Images = n.Images,
                    Created = n.Created,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Rate))/Convert.ToDouble(n.Rates.Count),1)
                    
                })
                .ToListAsync();
        }

        public async Task<Recipe> AddRecipe(AddRecipeVM addRecipeVM)
        {
            var recipe = _mapper.Map<Recipe>(addRecipeVM);

            recipe.Images = await _fileService.SaveImages(addRecipeVM.Files);
            recipe.Categories = await _dbContext.Categories
                .Where(c => addRecipeVM.SelectedCategoriesIds.Contains(c.Id))
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
        public async Task<Recipe> EditRecipe(AddRecipeVM addRecipeVM)
        {
            var recipe = _mapper.Map<Recipe>(addRecipeVM);
            var dbRecipe = await _dbContext.Recipes.Where(r => r.Id == recipe.Id)
                .Include(r => r.Categories)
                .Include(r => r.Images)
                .FirstOrDefaultAsync();
            if (dbRecipe == null) return new Recipe();

            dbRecipe.Name = recipe.Name;
            dbRecipe.Categories = await _dbContext.Categories.Where(r => addRecipeVM.SelectedCategoriesIds.Contains(r.Id))
                .ToListAsync();
            dbRecipe.PreparationTime = recipe.PreparationTime;
            dbRecipe.Yields = recipe.Yields;
            dbRecipe.Description = recipe.Description;
            dbRecipe.Ingredients = recipe.Ingredients;
            dbRecipe.Directions = recipe.Directions;
            dbRecipe.Images.RemoveAll(i => !recipe.Images.Select(c => c.Id).ToList().Contains(i.Id));

            if (addRecipeVM.Files != null && addRecipeVM.Files.Any())
            {
                var savedImages = await _fileService.SaveImages(addRecipeVM.Files);
                dbRecipe.Images.AddRange(savedImages);
            }

            _dbContext.Recipes.Update(dbRecipe);
            await _dbContext.SaveChangesAsync();
            return dbRecipe;
        }
        public async Task<bool> Rate(string userId, ulong recipeId, int rate)
        {
            if (rate is > 5 or < 0) return false;
            var userRate = await _dbContext.RecipeUserRates
                .Where(r => r.RecipeId == recipeId && r.ApplicationUserId == userId).FirstOrDefaultAsync();
            if (userRate != null)
            {
                userRate.Rate = rate;
                _dbContext.RecipeUserRates.Update(userRate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            
            var recipe = await _dbContext.Recipes.Where(m => m.Id == recipeId).FirstOrDefaultAsync();
            if (recipe == null) return false;

            var user = await _dbContext.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;

            await _dbContext.RecipeUserRates.AddAsync(new RecipeUserRate()
            {
                User = user,
                Recipe = recipe,
                Rate = rate
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<double> GetUserRate(string userId, ulong recipeId)
        {
            return await _dbContext.RecipeUserRates.Where(c => c.ApplicationUserId == userId && c.RecipeId == recipeId).Select(z => z.Rate).FirstOrDefaultAsync();
        }

        public async Task<double> GetRecipeRate(ulong recipeId)
        {
            return Math.Round(Convert.ToDouble(await _dbContext.RecipeUserRates.Where(r => r.RecipeId == recipeId)
                .AverageAsync(z => z.Rate)),1);
        }
    }
}
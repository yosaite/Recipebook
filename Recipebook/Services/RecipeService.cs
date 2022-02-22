﻿using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Models;
using Recipebook.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Recipebook.Interfaces;

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
                .Include(m => m.User)
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
                .Include(m => m.User)
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
                    User = n.User,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Value))/Convert.ToDouble(n.Rates.Count),1),
                })
                .FirstOrDefaultAsync();
            return recipe;
        }
        
        // Get all recipes
        public async Task<List<RecipeVM>> GetRecipesVM()
        {
            return await _dbContext.Recipes.Include(m => m.Images)
                .Include(m => m.User)
                .Select(n=>new RecipeVM()
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Images = n.Images,
                    Created = n.Created,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Value))/Convert.ToDouble(n.Rates.Count),1)
                    
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
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Value))/Convert.ToDouble(n.Rates.Count),1)
                    
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
            var recipes = await _dbContext.Recipes.Include(m => m.Images)
                .Include(m => m.User)
                .Where(c => c.UserId == userId)
                .Select(n=>new RecipeVM()
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Images = n.Images,
                    Created = n.Created,
                    Rate = n.Rates.Count == 0?0:Math.Round(Convert.ToDouble(n.Rates.Sum(t=>t.Value))/Convert.ToDouble(n.Rates.Count),1)
                    
                })
                .ToListAsync();
            return recipes;
        }

        public async Task<Recipe> AddRecipe(AddRecipeVM addRecipeVM)
        {
            var recipe = _mapper.Map<Recipe>(addRecipeVM);
            var images = await _fileService.SaveImages(addRecipeVM.Files);
            if (images.Any()) recipe.Images = images;
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
            var userRate = await _dbContext.Rates
                .Where(r => r.RecipeId == recipeId && r.UserId == userId).FirstOrDefaultAsync();
            if (userRate != null)
            {
                userRate.Value = rate;
                _dbContext.Rates.Update(userRate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            
            var recipe = await _dbContext.Recipes.Where(m => m.Id == recipeId).FirstOrDefaultAsync();
            if (recipe == null) return false;

            var user = await _dbContext.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;

            await _dbContext.Rates.AddAsync(new Rate()
            {
                User = user,
                Recipe = recipe,
                Value = rate
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<double> GetUserRate(string userId, ulong recipeId)
        {
            return await _dbContext.Rates.Where(c => c.UserId == userId && c.RecipeId == recipeId).Select(z => z.Value).FirstOrDefaultAsync();
        }
        public async Task<double> GetRecipeRate(ulong recipeId)
        {
            return Math.Round(Convert.ToDouble(await _dbContext.Rates.Where(r => r.RecipeId == recipeId)
                .AverageAsync(z => z.Value)),1);
        }
    }
}
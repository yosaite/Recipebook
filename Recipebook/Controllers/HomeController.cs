using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Recipebook.Models;
using Recipebook.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Recipebook.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recipebook.Interfaces;

namespace Recipebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, IRecipeService recipeService, UserManager<User> userManager, ICategoryService categoryService)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
            _categoryService = categoryService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(ulong categoryId = 0, int page = 1, RecipeSort sort = RecipeSort.Newest)
        {
            List<RecipeVM> recipes; 
            if (categoryId == 0)
            {
                recipes = await _recipeService.GetRecipesVM(page, sort);
                ViewBag.RecipesCount = await _recipeService.GetRecipesVMCount();
                ViewBag.ListTitle = "";
                ViewBag.CategoryId = 0;
            }
            else
            {
                var category = await _categoryService.GetCategory(categoryId);
                recipes = await _recipeService.GetRecipesVM(categoryId, page, sort);
                ViewBag.RecipesCount = await _recipeService.GetRecipesVMCount(category.Id);
                ViewBag.ListTitle = category.Name;
                ViewBag.CategoryId = category.Id;
            }
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            return View("Index",recipes);
        }
        
        [HttpGet]
        [Authorize(Roles="User, Admin")]
        public async Task<IActionResult> IndexUser(int page = 1, RecipeSort sort = RecipeSort.Newest)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var recipes = await _recipeService.GetRecipesVM(userId, page, sort);
            ViewBag.RecipesCount = await _recipeService.GetRecipesVMCount(userId);
            ViewBag.ListTitle = "Moje przepisy";
            ViewBag.User = true;
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            return View("Index", recipes);
        }
        
        [HttpGet]
        [Authorize(Roles="User, Admin")]
        public async Task<IActionResult> IndexUserFavorite(int page = 1, RecipeSort sort = RecipeSort.Newest)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var recipes = await _recipeService.GetFavoriteRecipesVM(userId, page, sort);
            ViewBag.RecipesCount = await _recipeService.GetFavoriteRecipesVMCount(userId);
            ViewBag.ListTitle = "Ulubione przepisy";
            ViewBag.UserFavorite = true;
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            return View("Index", recipes);
        }
        
        [HttpGet]
        [Authorize(Roles="User, Admin")]
        public async Task<IActionResult> IndexSearch(string search, int page = 1, RecipeSort sort = RecipeSort.Newest)
        {
            var recipes = await _recipeService.SearchRecipesVM(search, page, sort);
            ViewBag.RecipesCount = await _recipeService.SearchRecipesVMCount(search);
            ViewBag.ListTitle = $"Wyniki wyszukiwania: {search}";
            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            return View("Index", recipes);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

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

namespace Recipebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, IRecipeService recipeService, UserManager<ApplicationUser> userManager, ICategoryService categoryService)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        public IActionResult Index(ulong categoryId = 0,string categoryName="")
        {
            ViewBag.ListTitle = categoryName;
            return View(_recipeService.GetRecipes(categoryId));
        }
        public IActionResult Recipe(ulong recipeId)
        {
            return View(_recipeService.GetRecipe(recipeId));
        }
        [Authorize(Roles="User, Admin")]
        public IActionResult UserRecipes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.ListTitle = "Moje przepisy";
            return View("Index", _recipeService.GetRecipes(userId));
        }
        [HttpGet]
        public IActionResult AddRecipe()
        {
            ViewBag.CategoriesSelectList = _categoryService.GetCatogory().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View();
        }
        [HttpPost]
        public IActionResult AddRecipe(RecipeVM recipeVM)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                recipeVM.ApplicationUserId = userId;
                _recipeService.AddRecipe(recipeVM);
                return RedirectToAction("Index");
            }
            ViewBag.CategoriesSelectList = _categoryService.GetCatogory().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View();
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

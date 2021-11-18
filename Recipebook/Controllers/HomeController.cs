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

namespace Recipebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IRecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
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
        public IActionResult AddRecipe()
        {
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

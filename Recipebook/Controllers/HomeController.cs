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

        public async Task<IActionResult> Index(ulong categoryId = 0,string categoryName="")
        {
            ViewBag.ListTitle = categoryName;
            return View(await _recipeService.GetRecipes(categoryId));
        }
        [Authorize(Roles="User, Admin")]
        public async Task<IActionResult> UserRecipes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.ListTitle = "Moje przepisy";
            return View("Index", await _recipeService.GetRecipes(userId));
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

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Recipebook.Models;
using Recipebook.Services;
using Recipebook.ViewModel;

namespace Recipebook.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;

        public RecipeController(ILogger<RecipeController> logger, IRecipeService recipeService, UserManager<ApplicationUser> userManager, ICategoryService categoryService)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Recipe(ulong recipeId)
        {
            return View(await _recipeService.GetRecipe(recipeId));
        }
        
        [HttpGet]
        [Authorize(Roles="User, Admin")]
        public IActionResult Add()
        {
            ViewBag.CategoriesSelectList = _categoryService.GetCatogory().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View();
        }
        [HttpPost]
        [Authorize(Roles="User, Admin")]
        [RequestSizeLimit(52428800)] 
        public async Task<IActionResult> Add(RecipeVM recipeVm)
        {
            
            if (ModelState.IsValid)
            {       
                var userId = _userManager.GetUserId(HttpContext.User);
                recipeVm.ApplicationUserId = userId;
                var result = await _recipeService.AddRecipe(recipeVm);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoriesSelectList = _categoryService.GetCatogory().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View();
        }

    }
}
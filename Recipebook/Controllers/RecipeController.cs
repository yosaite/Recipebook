using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public RecipeController(ILogger<RecipeController> logger, IRecipeService recipeService,
            UserManager<ApplicationUser> userManager, ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Recipe(ulong recipeId)
        {
            return View(await _recipeService.GetRecipe(recipeId));
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Add()
        {
            var model = new RecipeVM
            {
                CategoriesList = _categoryService.GetCategories().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View("AddOrEdit",model);
        }
        
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(ulong recipeId)
        {
            var recipe = await _recipeService.GetRecipe(recipeId);
            var recipeVm = _mapper.Map<RecipeVM>(recipe);
            recipeVm.SelectedCategoriesIds = recipe.Categories.Select(m => m.CategoryId).ToList();
            recipeVm.CategoriesList = _categoryService.GetCategories().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            ViewBag.ListTitle = $"Edytuj przepis - {recipe.Name}";
            ViewBag.Edit = true;
            return View("AddOrEdit",recipeVm);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        [RequestSizeLimit(52428800)]
        public async Task<IActionResult> AddOrEdit(RecipeVM recipeVm)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                recipeVm.ApplicationUserId = userId;
                if (recipeVm.Id == 0)
                {
                    await _recipeService.AddRecipe(recipeVm);
                }
                else
                { 
                    await _recipeService.EditRecipe(recipeVm);
                }
                
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Delete(ulong id)
        {
            var recipe = await _recipeService.GetRecipe(id);
            if (recipe == null) return RedirectToAction("Index", "Home");
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (recipe.ApplicationUserId == _userManager.GetUserId(HttpContext.User) ||
                await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _recipeService.DeleteRecipe(id);
            }
            return RedirectToAction("UserRecipes", "Home");
        }

    }
}
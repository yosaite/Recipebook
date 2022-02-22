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
using Recipebook.Interfaces;
using Recipebook.Models;
using Recipebook.Services;
using Recipebook.ViewModel;

namespace Recipebook.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public RecipeController(ILogger<RecipeController> logger, IRecipeService recipeService,
            UserManager<User> userManager, ICategoryService categoryService, IMapper mapper)
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
            var recipe = await _recipeService.GetRecipeVM(recipeId);
            var userId = _userManager.GetUserId(HttpContext.User);
            if(userId != null) recipe.UserRate = await _recipeService.GetUserRate(userId, recipeId);
            return View(recipe);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Add()
        {
            var model = new AddRecipeVM
            {
                CategoriesList = _categoryService.GetCategories().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            ViewBag.Edit = false;
            return View("AddOrEdit",model);
        }
        
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(ulong recipeId)
        {
            var recipe = await _recipeService.GetRecipe(recipeId);
            var recipeVm = _mapper.Map<AddRecipeVM>(recipe);
            recipeVm.SelectedCategoriesIds = recipe.Categories.Select(m => m.Id).ToList();
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
        public async Task<IActionResult> AddOrEdit(AddRecipeVM addRecipeVM)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                addRecipeVM.UserId = userId;
                if (addRecipeVM.Id != 0)
                {
                    var recipe = await _recipeService.EditRecipe(addRecipeVM);
                    return RedirectToAction("Recipe", "Recipe", new {recipeId = recipe.Id});
                }
                else
                { 
                    var recipe = await _recipeService.AddRecipe(addRecipeVM);
                    return RedirectToAction("Recipe", "Recipe", new {recipeId = recipe.Id});
                }
            }
            ViewBag.Edit = false;
            return View("AddOrEdit",addRecipeVM);
        }

        
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Delete(ulong recipeId)
        {
            var recipe = await _recipeService.GetRecipeVM(recipeId);
            if (recipe == null) return RedirectToAction("Index", "Home");
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (recipe.User.Id == _userManager.GetUserId(HttpContext.User) ||
                await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _recipeService.DeleteRecipe(recipeId);
            }
            return RedirectToAction("UserRecipes", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Rate(ulong recipeId, int rate)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            if(await _recipeService.Rate(user.Id, recipeId, rate))
            {
                return Json(await _recipeService.GetRecipeRate(recipeId));
            }
            return BadRequest();
        }
        
    }
}
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipebook.Models;
using Recipebook.Services;

namespace Recipebook.Controllers
{
    public class CommentController: Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CommentController(ILogger<RecipeController> logger, IRecipeService recipeService, UserManager<ApplicationUser> userManager, ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create(int recipeId)
        {
            return Ok();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipebook.Models;
using Recipebook.Services;
using Recipebook.ViewModel;

namespace Recipebook.Controllers
{
    public class CommentController: Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CommentController(ILogger<CommentController> logger, ICommentService commentService, UserManager<ApplicationUser> userManager, ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _commentService = commentService;
            _userManager = userManager;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Add(AddCommentVM addCommentVM)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(await _commentService.AddComment(user.Id, addCommentVM.RecipeId, addCommentVM.Content))
            {
                return Ok();
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<ICollection<CommentVM>> Get(ulong recipeId)
        {
            return await _commentService.GetComments(recipeId);
        }
    }
}
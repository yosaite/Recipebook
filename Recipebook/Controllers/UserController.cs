using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipebook.Interfaces;
using Recipebook.Models;

namespace Recipebook.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAvatar()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return NotFound();
            
            return Json(await _userService.GetAvatar(user));
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> AddFavorite(ulong recipeId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return NotFound();

            return Json(await _userService.Favorite(user.Id, recipeId));
        }
    }
}
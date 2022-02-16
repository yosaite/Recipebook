using Microsoft.AspNetCore.Mvc;
using Recipebook.Services;

namespace Recipebook.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View(categoryService.GetCategories());
        }
    }
}

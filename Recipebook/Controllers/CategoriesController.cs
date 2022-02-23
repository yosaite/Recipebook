using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipebook.Interfaces;
using Recipebook.Models;
using Recipebook.Services;
using Recipebook.ViewModel;

namespace Recipebook.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(AddCategoryVM addCategoryVm)
        {
            if (ModelState.IsValid)
            {
                if (addCategoryVm.Id == 0)
                {
                    await _categoryService.AddCategory(addCategoryVm);
                }
            }
            return RedirectToAction("Index", "Categories");
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            return View("AddOrEdit");
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ulong categoryId)
        {
            return View("AddOrEdit");
        }
        
        public IActionResult Index()
        {
            return View(_categoryService.GetCategories());
        }
    }
}

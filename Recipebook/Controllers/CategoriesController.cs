using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public CategoriesController(ICategoryService categoryService, IMapper mapper, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(AddCategoryVM addCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Edit = false;
                return View("AddOrEdit", addCategoryVm);
            }
            if (addCategoryVm.Id == 0)
            {
                await _categoryService.AddCategory(addCategoryVm);
                return RedirectToAction("Index");
            }
            await _categoryService.EditCategory(addCategoryVm);
            return RedirectToAction("Index");

        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var category = new AddCategoryVM();
            ViewBag.Edit = false;
            return View("AddOrEdit",category);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ulong categoryId)
        {
            var category = await _categoryService.GetCategory(categoryId);
            var categoryVm = _mapper.Map<AddCategoryVM>(category);
            ViewBag.Edit = true;
            return View("AddOrEdit",categoryVm);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ulong categoryId)
        {
            var category = await _categoryService.GetCategory(categoryId);
            if (category == null) return RedirectToAction("Index", "Home");
            
            await _categoryService.DeleteCategory(categoryId);
            
            return RedirectToAction("Index", "Categories");
        }
        
        public IActionResult Index()
        {
            return View(_categoryService.GetCategories());
        }
    }
}

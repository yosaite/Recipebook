﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipebook.Models;
using Recipebook.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Recipebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeService _recipeService;

        public HomeController(ILogger<HomeController> logger, IRecipeService recipeService)
        {
            _logger = logger;
            _recipeService = recipeService;
        }

        public IActionResult Index(ulong categoryId = 0,string categoryName="")
        {
            ViewBag.CategoryName = categoryName;
            return View(_recipeService.GetRecipes(categoryId));
        }
        public IActionResult Recipe(ulong recipeId)
        {
            return View(_recipeService.GetRecipe(recipeId));
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

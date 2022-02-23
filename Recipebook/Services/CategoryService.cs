using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Recipebook.Interfaces;
using Recipebook.ViewModel;

namespace Recipebook.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CategoryService(ApplicationDbContext dbContext, IMapper mapper, IFileService fileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileService = fileService;
        }
        public List<Category> GetCategories()
        {
            return _dbContext.Categories.Include(m => m.Image).OrderBy(c => c.Name).ToList();
        }

        public async Task AddCategory(AddCategoryVM addCategoryVm)
        {
            var category = _mapper.Map<Category>(addCategoryVm);
            var image = await _fileService.SaveImage(addCategoryVm.File);
            if (image != default) 
                category.Image = image;
            
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}

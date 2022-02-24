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
        
        public async Task<Category> GetCategory(ulong categoryId)
        {
            return await _dbContext.Categories.Include(d=>d.Image).Where(z => z.Id == categoryId).FirstOrDefaultAsync();
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

        public async Task EditCategory(AddCategoryVM addCategoryVm)
        {
            var category = _mapper.Map<Category>(addCategoryVm);
            var dbCategory = await _dbContext.Categories.Where(z => z.Id == category.Id).Include(b=>b.Image).FirstOrDefaultAsync();
            if (dbCategory == null) return;
            if (addCategoryVm.File != null)
            {
                var image = await _fileService.SaveImage(addCategoryVm.File);
                dbCategory.Image = image;
            }
            
            dbCategory.Name = category.Name;
            _dbContext.Categories.Update(dbCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategory(ulong categoryId)
        {
            var category = await _dbContext.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
            }
            
            await _dbContext.SaveChangesAsync();
        }

    }
}

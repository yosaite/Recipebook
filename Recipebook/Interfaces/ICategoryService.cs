using System.Collections.Generic;
using System.Threading.Tasks;
using Recipebook.Models;
using Recipebook.ViewModel;

namespace Recipebook.Interfaces
{
    public interface ICategoryService
    {
        public List<Category> GetCategories();
        Task AddCategory(AddCategoryVM addCategoryVm);
        Task EditCategory(AddCategoryVM addCategoryVm);
        Task<Category> GetCategory(ulong categoryId);
        Task DeleteCategory(ulong categoryId);
    }
}

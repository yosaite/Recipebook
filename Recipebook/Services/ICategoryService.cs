using Recipebook.Models;
using System.Collections.Generic;

namespace Recipebook.Services
{
    public interface ICategoryService
    {
        public List<Category> GetCategories();
    }
}

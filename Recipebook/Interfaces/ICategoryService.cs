using System.Collections.Generic;
using Recipebook.Models;

namespace Recipebook.Interfaces
{
    public interface ICategoryService
    {
        public List<Category> GetCategories();
    }
}

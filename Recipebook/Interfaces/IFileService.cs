using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Recipebook.Models;

namespace Recipebook.Interfaces
{
    public interface IFileService
    {
        Task<List<Image>> SaveImages(IEnumerable<IFormFile> formFiles);
        Task<Image> SaveImage(IFormFile formFile);
    }
}
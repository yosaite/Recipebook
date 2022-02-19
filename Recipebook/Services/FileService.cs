using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Recipebook.Data;
using Recipebook.Models;

namespace Recipebook.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public FileService(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<List<Image>> SaveImages(IEnumerable<IFormFile> formFiles)
        {
            var path = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var images = new List<Image>();
            foreach (var file in formFiles.Where(img=>img.Length > 0))
            {
                var img = new Image(){Path = $"{Guid.NewGuid()}.{file.FileName.Split('.').Last()}"};
                await using (var stream = File.Create(Path.Combine(path,img.Path)))
                {
                    await file.CopyToAsync(stream);
                }
                images.Add(img);
            }
            return images;
        }
    }
}
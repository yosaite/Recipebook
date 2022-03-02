using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Recipebook.Data;
using Recipebook.Interfaces;
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
            if (formFiles == null) return new List<Image>();
            var path = Path.Combine(_environment.WebRootPath, Setup.ImagesFolder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var images = new List<Image>();
            foreach (var file in formFiles.Where(img=>img.Length > 0))
            {
                var img = new Image(){File = $"{Guid.NewGuid()}.{file.FileName.Split('.').Last()}"};
                await using (var stream = File.Create(Path.Combine(path,img.File)))
                {
                    await file.CopyToAsync(stream);
                }
                images.Add(img);
            }
            return images;
        }
        
        public async Task<Image> SaveImage(IFormFile formFile)
        {
            if (formFile == null) return new Image();
            var path = Path.Combine(_environment.WebRootPath, Setup.ImagesFolder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var img = new Image(){File = $"{Guid.NewGuid()}.{formFile.FileName.Split('.').Last()}"};
            await using (var stream = File.Create(Path.Combine(path, img.File)))
            {
                await formFile.CopyToAsync(stream);
            }

            return img;
        }
        
    }
}
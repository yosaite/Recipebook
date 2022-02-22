using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Recipebook.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Recipebook.Interfaces;
using Recipebook.Models;
using Recipebook.ViewModel;

namespace Recipebook.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public CommentService(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<bool> AddComment(string userId, ulong recipeId, string comment)
        {
            var user = await _dbContext.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;
            var recipe = await _dbContext.Recipes.Where(m => m.Id == recipeId).FirstOrDefaultAsync();
            if (recipe == null) return false;
            await _dbContext.Comments.AddAsync(new Comment()
            {
                Recipe = recipe,
                User =  user,
                Content = comment
            });
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<CommentVM>> GetComments(ulong recipeId)
        {
            return await _dbContext.Comments.Where(r => r.RecipeId == recipeId).Include(u => u.User).ThenInclude(z=>z.Image).Select(n =>
                new CommentVM()
                {
                    Content = n.Content,
                    UserName = n.User.UserName,
                    Created = n.Created,
                    Avatar = n.User.Image.Path ?? "no-image.png"
                }).OrderByDescending(z=>z.Created).ToListAsync();
        }
    }
}
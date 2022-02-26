using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recipebook.Data;
using Recipebook.Interfaces;
using Recipebook.Models;

namespace Recipebook.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetAvatar(User user)
        {
            var avatar = await _dbContext.Users.Include(z => z.Image).Where(u => u.Id == user.Id).Select(z => z.Image)
                .FirstOrDefaultAsync();
            return avatar != null ? avatar.Path : "no-image.png";
        }

        public async Task<bool> Favorite(string userId, ulong recipeId)
        {
            var dbFavorite = await _dbContext.Favorites.Where(c => c.UserId == userId && c.RecipeId == recipeId)
                .FirstOrDefaultAsync();
            
            if (dbFavorite != null)
            {
                _dbContext.Favorites.Remove(dbFavorite);
                await _dbContext.SaveChangesAsync();
                
                return false;
            }

            var recipe = await _dbContext.Recipes.Where(m => m.Id == recipeId).FirstOrDefaultAsync();
            if (recipe == null) return false;
            
            var user = await _dbContext.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;
            
            var favorite = new Favorite()
            {
                Recipe = recipe,
                User = user
            };

            await _dbContext.Favorites.AddAsync(favorite);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> GetFavorite(string userId, ulong recipeId)
        {
            var dbFavorite = await _dbContext.Favorites.Where(c => c.UserId == userId && c.RecipeId == recipeId)
                .FirstOrDefaultAsync();

            return dbFavorite != null;
        }
    }
}
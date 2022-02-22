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
    }
}
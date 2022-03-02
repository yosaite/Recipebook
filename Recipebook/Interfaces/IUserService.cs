using System.Threading.Tasks;
using Recipebook.Models;

namespace Recipebook.Interfaces
{
    public interface IUserService
    {
        Task<string> GetAvatar(User user);
        Task<Image> GetAvatarImage(User user);
        Task<bool> Favorite(string userId, ulong recipeId);
        Task<bool> GetFavorite(string userId, ulong recipeId);
        Task<bool> SetAvatar(User user, Image image);
    }
}
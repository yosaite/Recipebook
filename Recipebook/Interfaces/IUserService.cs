using System.Threading.Tasks;
using Recipebook.Models;

namespace Recipebook.Interfaces
{
    public interface IUserService
    {
        Task<string> GetAvatar(User user);
    }
}
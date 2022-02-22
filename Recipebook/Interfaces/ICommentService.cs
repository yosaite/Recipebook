using System.Collections.Generic;
using System.Threading.Tasks;
using Recipebook.ViewModel;

namespace Recipebook.Interfaces
{
    public interface ICommentService
    {
        Task<bool> AddComment(string userId, ulong recipeId, string comment);
        Task<ICollection<CommentVM>> GetComments(ulong recipeId);
    }
}
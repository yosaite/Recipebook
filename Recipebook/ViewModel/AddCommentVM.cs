using System.Globalization;

namespace Recipebook.ViewModel
{
    public class AddCommentVM
    {
        public ulong RecipeId { get; set; }
        public string Content { get; set; }
    }
}
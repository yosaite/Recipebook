using System;

namespace Recipebook.ViewModel
{
    public class CommentVM
    {
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public string Avatar { get; set; }
    }
}
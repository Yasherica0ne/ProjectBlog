using System;

namespace ProjectBlog.Models
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            Date = DateTime.Now;
        }

        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
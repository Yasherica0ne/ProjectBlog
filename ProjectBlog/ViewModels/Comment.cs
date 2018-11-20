using System;

namespace ProjectBlog.ViewModels
{
    public class Comment
    {
        public Comment()
        {
            Date = DateTime.Now;
        }

        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
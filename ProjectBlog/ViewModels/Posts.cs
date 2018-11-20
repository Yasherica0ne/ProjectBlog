using System.Collections.Generic;

namespace ProjectBlog.ViewModels
{
    public class Posts
    {
        public List<Post> PostsList { get; set; }
        public string SearchString { get; set; }
    }
}
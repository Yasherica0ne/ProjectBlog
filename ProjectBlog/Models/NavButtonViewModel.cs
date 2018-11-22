namespace ProjectBlog.Models
{
    public class NavButtonViewModel
    {
        public NavButtonViewModel(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }
        public string Url { get; set; }
    }
}
using System.Collections.Generic;

namespace ProjectBlog.Models
{
    public class Slider
    {
        public Slider()
        {
            Urls = new List<string>();
        }

        public List<string> Urls { get; private set; }
        public int SlidesCount { get => Urls.Count; }
        public int Speed { get; set; }
    }
}
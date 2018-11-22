using System.Collections.Generic;

namespace ProjectBlog.Models
{
    public class SliderViewModel
    {
        public SliderViewModel()
        {
            Urls = new List<string>();
        }

        public List<string> Urls { get; private set; }
        public int SlidesCount { get => Urls.Count; }
        public int Speed { get; set; }
    }
}
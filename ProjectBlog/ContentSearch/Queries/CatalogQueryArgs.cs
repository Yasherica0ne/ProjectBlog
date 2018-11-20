using System.Collections.Generic;

namespace ProjectBlog.ContentSearch.Queries
{
    public class CatalogQueryArgs
    {
        public CatalogQueryArgs()
        {
            Page = 1;
            Size = 5;
        }

        public string Title { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Lang { get; set; }
    }
}
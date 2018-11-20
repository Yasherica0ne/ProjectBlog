using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace ProjectBlog.ContentSearch.SearchTypes
{
    public class ProductSearchResultItem: SearchResultItem
    {
        [IndexField("title")]
        public string Title { get; set; }

        [IndexField("url")]
        public string ItemUrl { get; set; }

        [IndexField("content")]
        public string PostContent { get; set; }

        [IndexField("sub title")]
        public string SubTitle { get; set; }

        [IndexField("category")]
        public string Category { get; set; }

        [IndexField("tags_list")]
        public string[] Tags { get; set; }
    }
}
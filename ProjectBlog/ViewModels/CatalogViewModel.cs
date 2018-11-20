using System.Collections.Generic;

namespace ProjectBlog.ViewModels
{
    public class CatalogViewModel
    {
        public string SearchString { get; set; }
        public int Page { get; set; }
        public int PostsPerPage { get; set; }
        public long Pages { get; set; }
        public string Lang { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public List<FacetViewModel> Categories { get; set; }
        public List<FacetViewModel> Tags { get; set; }

        public CatalogViewModel()
        {
            Products = new List<ProductViewModel>();
            Categories = new List<FacetViewModel>();
            Tags = new List<FacetViewModel>();
            SearchString = string.Empty;
            Page = 1;
        }
    }

    public class ProductViewModel
    {
        public string Theme { get; set; }
        public string Content { get; set; }
        public string SubTitle { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string[] Tags { get; set; }
    }

    public class FacetViewModel
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; }
    }
}
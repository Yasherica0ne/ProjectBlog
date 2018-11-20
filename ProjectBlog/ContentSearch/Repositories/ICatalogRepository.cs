using ProjectBlog.ContentSearch.Queries;
using ProjectBlog.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Linq;

namespace ProjectBlog.ContentSearch.Repositories
{
    public interface ICatalogRepository
    {
        SearchResults<ProductSearchResultItem> Get(CatalogQueryArgs args);
    }
}

using System.Linq;
using ProjectBlog.ContentSearch.Queries;
using ProjectBlog.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;

namespace ProjectBlog.ContentSearch.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly string IndexName = $"custom_{Sitecore.Context.Database.Name.ToLower()}_index";

        private ISearchIndex _index;

        private ISearchIndex Index => _index ?? (_index = ContentSearchManager.GetIndex(IndexName));

        private IProviderSearchContext _context;

        private IProviderSearchContext Context => _context ?? (_context = Index.CreateSearchContext());

        private bool SearchTags(CatalogQueryArgs args, ProductSearchResultItem item)
        {
            foreach(var tag in item.Tags)
            {
                if (args.Tags.Contains(tag)) return true;
            }
            return false;
        }

        public SearchResults<ProductSearchResultItem> Get(CatalogQueryArgs args)
        {
            var searchPredicate = PredicateBuilder.True<ProductSearchResultItem>();

            if (!string.IsNullOrEmpty(args.Title))
            {
                searchPredicate = searchPredicate.And(x => x.Title.Contains(args.Title)
                                                        || x.Content.Contains(args.Title)
                                                        || x.Tags.Contains(args.Title));
            }

            if (args.Categories.Count != 0)
            {
                searchPredicate = searchPredicate.And(x => args.Categories.Contains(x.Category));
            }

            if (args.Tags.Count != 0)
            {
                var searchPredicate2 = PredicateBuilder.True<ProductSearchResultItem>();
                foreach (var tag in args.Tags)
                {
                    searchPredicate2 = searchPredicate2.Or(x => x.Tags.Contains(tag));
                }
                searchPredicate = searchPredicate.And(searchPredicate2);
            }

            var result = Context.GetQueryable<ProductSearchResultItem>()
                .Where(searchPredicate)
                .Where(x => x.Language.Equals(args.Lang))
                .FacetOn(x => x.Category, 1)
                .FacetOn(x => x.Tags, 1)
                .Page(args.Page - 1, args.Size)
                .GetResults();

            return result;
        }
    }
}
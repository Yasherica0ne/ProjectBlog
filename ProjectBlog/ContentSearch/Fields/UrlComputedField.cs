using System.Linq;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Sites;

namespace ProjectBlog.ContentSearch.Fields
{
    public class UrlComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, nameof(indexable));

            if (indexable is SitecoreIndexableItem indexableItem)
            {
                Item item = indexableItem.Item;
                var urlOptions = LinkManager.GetDefaultUrlOptions();
                urlOptions.Language = item.Language;
                urlOptions.AlwaysIncludeServerUrl = false;

                string itemUrl = LinkManager.GetItemUrl(item, urlOptions);
                itemUrl = itemUrl.Substring(3);
                itemUrl = itemUrl.Substring(itemUrl.IndexOf('/'));
                itemUrl = itemUrl.Replace(item.Language.Name, "");
                itemUrl = itemUrl.Remove(itemUrl.IndexOf('/'), 2);
                return itemUrl;
            }

            Log.Warn($"{this} : unsupported IIndexable type : {indexable.GetType()}", this);
            return null;
        }


    }
}
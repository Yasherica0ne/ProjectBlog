using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;

namespace ProjectBlog.ContentSearch.Fields
{
    public class CategoryComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {

            Assert.ArgumentNotNull(indexable, nameof(indexable));

            if (indexable is SitecoreIndexableItem indexableItem)
            {
                using (new LanguageSwitcher(indexableItem.Item.Language))
                {
                    LookupField field = indexableItem.Item.Fields["Category"];
                    Item item = field.TargetItem;
                    string value = item.Fields["Value"].Value;
                    return value;
                }

            }

            Log.Warn($"{this} : unsupported IIndexable type : {indexable.GetType()}", this);
            return null;

        }


    }
}
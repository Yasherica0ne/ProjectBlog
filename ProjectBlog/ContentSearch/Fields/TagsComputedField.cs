using System;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Globalization;

namespace ProjectBlog.ContentSearch.Fields
{
    public class TagsComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            try
            {
                Assert.ArgumentNotNull(indexable, nameof(indexable));

                if (indexable is SitecoreIndexableItem indexableItem)
                {
                    using (new LanguageSwitcher(indexableItem.Item.Language))
                    {
                        MultilistField field = indexableItem.Item.Fields["Tags"];
                        var items = field.GetItems();
                        return items.Select(x => x.Fields["Value"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            Log.Warn($"{this} : unsupported IIndexable type : {indexable.GetType()}", this);
            return null;

        }


    }
}
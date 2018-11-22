using System;
using Sitecore;
using Sitecore.Collections;
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
            try
            {
                Assert.ArgumentNotNull(indexable, nameof(indexable));

                if (indexable is SitecoreIndexableItem indexableItem)
                {

                    Language currentLanguage = indexableItem.Item.Language;
                    using (new LanguageSwitcher(currentLanguage))
                    {
                        LookupField field = indexableItem.Item.Fields["Category"];
                        Item item = field.TargetItem;
                        string value = item.Fields["Value"].Value;
                        //Language.TryParse("en", out Language defaultLang);
                        //if (string.IsNullOrEmpty(value) && item.Language != defaultLang)
                        //{
                        //    Item fallbackItem = Context.Database.GetItem(item.ID, defaultLang);
                        //    value = fallbackItem.Fields["Category"].Value;
                        //}
                        return value;
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
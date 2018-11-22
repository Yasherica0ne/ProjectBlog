using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Links;
using System;

namespace ProjectBlog.Events
{
    public class PostCreationDataLoger
    {
        public void OnPostCreated(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter<Item>(args, 0);
            ID iD = new ID("{7CF36840-EDC9-4AF0-B525-B13B351860F6}");
            if (item.TemplateID.CompareTo(iD) == 0)
            {
                var options = LinkManager.GetDefaultUrlOptions();
                options.AlwaysIncludeServerUrl = true;
                string url = LinkManager.GetItemUrl(item);
                var stats = item.Statistics;
                Log.Info($"Created by {stats.CreatedBy}, post title: {item.Name}, link: {url}, created on {stats.Created} ", this);
            }
        }
    }
}
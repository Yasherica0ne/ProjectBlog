using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ProjectBlog.Pipelines
{
    public class CheckPage : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            string filePath = HttpContext.Current.Server.MapPath(args.Url.FilePath);
            if (IsValidItem() || args.LocalPath.StartsWith("/sitecore") || File.Exists(filePath)) return;
            Context.Item = Get404Page();
            if (Context.Item != null) Context.Items["Is404Page"] = "true";
        } 

        protected virtual bool IsValidItem()
        {
            if (Context.Item == null || Context.Item.Versions.Count == 0) return false;
            if (Context.Item.Visualization.Layout == null) return false;
            return true;
        }

        private Item Get404Page()
        {
            Item item = Context.Database.GetItem("{ED7A8E8B-2F66-4C53-866A-6663B8F218CA}");
            return item;
        }


    }
}
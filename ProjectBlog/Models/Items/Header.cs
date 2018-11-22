using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace ProjectBlog.Models.Items
{
    public class Header
    {
        public Header()
        {
            NavButtons = new List<NavButtonViewModel>();
        }

        public void AddButton(string name, string url)
        {
            NavButtons.Add(new NavButtonViewModel(name, url));
        }


        [SitecoreField("Site name")]
        public virtual string SiteName { get; set; }

        [SitecoreField("Site heading")]
        public virtual string SiteHeading { get; set; }

        [SitecoreField("Sub heading")]
        public virtual string SubHeading { get; set; }

        public string LangButton { get; set; }
        public List<NavButtonViewModel> NavButtons { get; private set; }
        public string BackgroundUrl { get; set; }
        public string LangSwitchUrl { get; set; }
    }
}
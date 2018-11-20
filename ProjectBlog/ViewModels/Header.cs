using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace ProjectBlog.ViewModels
{
    public class Header
    {
        public Header()
        {
            NavButtons = new List<NavButton>();
        }

        public void AddButton(string name, string url)
        {
            NavButtons.Add(new NavButton(name, url));
        }

        public string LangButton { get; set; }
        public List<NavButton> NavButtons { get; private set; }
        public string BackgroundUrl { get; set; }
        public string LangSwitchUrl { get; set; }
    }
}
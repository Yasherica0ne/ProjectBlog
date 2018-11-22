using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBlog.Models.Items
{
    public class BasePage
    {
        [SitecoreField("Page title")]
        public virtual string PageTitle { get; set; }

        [SitecoreField("Site name")]
        public virtual string SiteName { get; set; }

        [SitecoreField("Site heading")]
        public virtual string SiteHeading { get; set; }

        [SitecoreField("Sub heading")]
        public virtual string SubHeading { get; set; }

        [SitecoreField("Copyright")]
        public virtual string Copyright { get; set; }
    }
}
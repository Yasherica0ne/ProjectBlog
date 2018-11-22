using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBlog.Models.Items
{
    public class Footer
    {
        [SitecoreField("Copyright")]
        public virtual string Copyright { get; set; }
    }
}
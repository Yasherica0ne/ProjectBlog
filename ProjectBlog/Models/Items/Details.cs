using Glass.Mapper.Sc.Configuration.Attributes;

namespace ProjectBlog.Models.Items
{
    public class Details
    {

        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        [SitecoreField("Sub title")]
        public virtual string SubTitle { get; set; }

        [SitecoreField("Content")]
        public virtual string Content { get; set; }
    }
}
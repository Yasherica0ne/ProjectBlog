using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBlog.ViewModels
{
    public class NavButton
    {
        public NavButton(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }
        public string Url { get; set; }
    }
}
using Glass.Mapper.Sc;
using ProjectBlog.ContentSearch.Queries;
using ProjectBlog.ContentSearch.Repositories;
using ProjectBlog.Models;
using ProjectBlog.Models.Items;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjectBlog.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult LanguageSwitcher

        private string GetUrl(Item item)
        {
            string url = Sitecore.Links.LinkManager.GetItemUrl(item);
            return url;
        }

        private Item GetItem(string itemPath)
        {
            Item item = Context.Database.GetItem(itemPath);
            return item;
        }

        private Header GetHeader()
        {
            string homePath = Context.Site.StartPath;
            Item homeItem = GetItem(homePath);
            ISitecoreContext ctx = new SitecoreContext();
            var menu = ctx.GetCurrentItem<Header>();
            if (homeItem != null)
            {
                string name = homeItem.Fields["Page title"].Value;
                string url = "/";
                menu.AddButton(name, url);
                var children = homeItem.GetChildren();
                foreach (Item button in children)
                {
                    CheckboxField show = button.Fields["Show in navigation"];
                    if (show.Checked)
                    {
                        name = button.Fields["Page title"].Value;
                        url = GetUrl(button);
                        menu.AddButton(name, url);
                    }
                }
            }

            string langSwitchUrl = GetUrl(Context.Item) + "?sc_lang=";
            string buttonName = null;
            string currentLang = Context.Language.Name;
            switch (currentLang)
            {
                case "en":
                    {
                        langSwitchUrl += "ru-RU";
                        buttonName = "EN/RU";
                        break;
                    }
                case "ru-RU":
                    {
                        langSwitchUrl += "en";
                        buttonName = "RU/EN";
                        break;
                    }
            }

            menu.LangButton = buttonName;
            menu.LangSwitchUrl = langSwitchUrl;
            Item item = RenderingContext.CurrentOrNull.ContextItem;
            ImageField backgroundImage = new ImageField(item.Fields["Background image"]);
            menu.BackgroundUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(backgroundImage.MediaItem);
            return menu;
        }

        public ActionResult NewHeader()
        {
            Header menu = GetHeader();
            return PartialView("~/Views/Renderings/New Blog/Header.cshtml", menu);
        }
        // GET: Home
        public ActionResult Header()
        {
            Header menu = GetHeader();
            return PartialView("~/Views/Renderings/Header.cshtml", menu);
        }

        private List<ProductViewModel> GetPosts(Item[] items)
        {
            var list = new List<ProductViewModel>();

            foreach (var item in items)
            {
                list.Add(new ProductViewModel()
                {
                    Theme = item.Fields["Title"].Value,
                    Content = item.Fields["Sub title"].Value,
                    Url = GetUrl(item)
                });
            }
            return list;
        }

        public ActionResult TopPosts()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = GetItem(dataSourceId);
            MultilistField imagesList = dataSource.Fields["Posts"];
            Item[] posts = imagesList.GetItems();
            var postsList = GetPosts(posts);
            return PartialView("~/Views/Renderings/Home/Body.cshtml", postsList);
        }

    }
}
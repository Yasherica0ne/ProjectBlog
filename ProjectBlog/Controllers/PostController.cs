using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using ProjectBlog.ContentSearch.Queries;
using ProjectBlog.ContentSearch.Repositories;
using ProjectBlog.Models;
using ProjectBlog.Models.Items;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjectBlog.Controllers
{
    public class PostController : GlassController
    {

        private readonly ICatalogRepository _repository;

        public PostController(ICatalogRepository repository)
        {
            _repository = repository;
        }

        public ActionResult CommentForm()
        {
            return PartialView("~/Views/Renderings/Home/Posts/Details/CommentForm.cshtml");
        }

        public ActionResult Comments()
        {
            var children = Context.Item.GetChildren();
            List<CommentViewModel> list = new List<CommentViewModel>();
            foreach (Item item in children)
            {
                if (string.IsNullOrEmpty(item.Fields["Text"]?.Value)) continue;
                DateTime? date = null;
                if (DateTime.TryParse(item.Fields["Date"].Value, out DateTime newDate))
                {
                    date = newDate;
                }
                CommentViewModel comment = new CommentViewModel()
                {
                    Author = item.Fields["Author"].Value,
                    Text = item.Fields["Text"].Value,
                    Date = date.Value
                };
                list.Add(comment);
            }
            return PartialView("~/Views/Renderings/Home/Posts/Details/Comments.cshtml", list);
        }

        public ActionResult SearchBar()
        {
            return PartialView("~/Views/Renderings/Home/Posts/SearchBar.cshtml");
        }

        public void PublishItem(Database dbMaster, Item iParent)
        {
            try
            {
                Database dbWeb = Factory.GetDatabase("web");
                PublishOptions po = new PublishOptions(dbMaster, dbWeb, PublishMode.SingleItem, Context.Language, DateTime.Now)
                {
                    RootItem = iParent,
                    Deep = true // Publishing subitems
                };

                (new Publisher(po)).Publish();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception publishing items from custom pipeline! : " + ex, this);
            }
        }

        // GET: Post
        [HttpPost]
        public ActionResult CreateComment(CommentViewModel comment)
        {
            if (string.IsNullOrEmpty(comment.Text) || string.IsNullOrEmpty(comment.Author)) return Content("");
            string name = "Comment_" + Sitecore.DateUtil.IsoNow;
            Database masterDB = Factory.GetDatabase("master");
            var template = masterDB.GetTemplate("{B06B6CB9-7A5D-42D9-91E6-F094AEA4C0CD}");
            using (new LanguageSwitcher(Context.Language))
            {
                ID parentId = Context.Item.ID;
                Item masterParent = masterDB.GetItem(parentId);

                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    Item newItem = null;
                    try
                    {
                        newItem = masterParent.Add(name, template);
                        if (newItem != null)
                        {
                            newItem.Editing.BeginEdit();
                            newItem["Author"] = comment.Author;
                            newItem["Text"] = comment.Text;
                            newItem["Date"] = comment.Date.ToString();
                            newItem.Editing.EndEdit();
                            PublishItem(masterDB, masterParent);
                        }
                    }
                    catch
                    {
                        newItem.Editing.CancelEdit();
                    }
                }
            }

            return PartialView("~/Views/Renderings/Home/Posts/Details/NewComment.cshtml", comment);
        }

        public ActionResult LoadPosts(CatalogViewModel model)
        {
            CatalogViewModel newModel = LoadPostsFromIndex(model, false);
            return PartialView("~/Views/Renderings/Home/Posts/PostsList.cshtml", newModel);
        }

        public ActionResult NextPosts(string text)
        {
            //if (model.Page - 1 != 0) model.Page++;
            //return LoadPosts(model);
            //int kek = number;
            return PartialView("~/Views/Renderings/Home/Posts/ElementsNotFound.cshtml");
        }

        [HttpPost]
        public ActionResult OlderPosts(CatalogViewModel model)
        {
            if (model.Page != model.Pages) model.Page++;
            return LoadPosts(model);
        }

        private bool CheckFacets(List<FacetViewModel> fasets, string title)
        {
            if (fasets.Count == 0) return false;
            FacetViewModel current = fasets.Find(n => n.Title.Equals(title));
            if (current == null) return false;
            else return current.IsChecked;
        }

        private CatalogViewModel LoadPostsFromIndex(CatalogViewModel model, bool startFromBegining = true)
        {
            List<FacetViewModel> categories = model.Categories.Where(n => n.IsChecked).ToList();
            List<FacetViewModel> tags = model.Tags.Where(n => n.IsChecked).ToList();

            var args = new CatalogQueryArgs
            {
                Title = model.SearchString,
                Categories = categories.Select(category => category.Title).ToList(),
                Tags = tags.Select(tag => tag.Title).ToList(),
                Page = model.Page,
                Size = model.PostsPerPage,
                Lang = model.Lang
            };

            var results = _repository.Get(args);

            var newModel = new CatalogViewModel
            {
                Products = results.Select(x => new ProductViewModel
                {
                    Theme = x.Document.Title,
                    Content = x.Document.Content,
                    SubTitle = x.Document.SubTitle,
                    Url = x.Document.ItemUrl,
                    Category = x.Document.Category,
                    Tags = x.Document.Tags
                }).ToList(),
                Page = model.Page,
                Pages = model.Pages,
                PostsPerPage = model.PostsPerPage,
                SearchString = model.SearchString,
                Lang = model.Lang
            };

            double totalResults = results.TotalSearchResults;
            newModel.Pages = (long)Math.Ceiling(totalResults / model.PostsPerPage);

            if (startFromBegining)
            {
                var categoryFacets = results.Facets.Categories.FirstOrDefault(x => x.Name == "category");
                if (categoryFacets != null)
                {
                    newModel.Categories = categoryFacets.Values.Select(x => new FacetViewModel
                    {
                        Title = x.Name,
                        Count = x.AggregateCount
                    }).ToList();
                }

                var tagsFacets = results.Facets.Categories.FirstOrDefault(x => x.Name == "tags_list");
                if (tagsFacets != null)
                {
                    newModel.Tags = tagsFacets.Values.Select(x => new FacetViewModel
                    {
                        Title = x.Name,
                        Count = x.AggregateCount
                    }).ToList();
                }
            }

            return newModel;
        }

        public ActionResult Posts(CatalogViewModel model)
        {
            var item = Context.Item.Fields["PostsPerPage"].Value;
            int.TryParse(item, out int size);
            model.PostsPerPage = 2; //size;
            model.Lang = Context.Language.Name;

            CatalogViewModel newModel = LoadPostsFromIndex(model);

            return PartialView("~/Views/Renderings/Home/Posts/Posts.cshtml", newModel);
        }


        public ActionResult Details()
        {

            //ISitecoreContext ctx = GetContextItem<ISitecoreContext>();
            var item = GetLayoutItem<Details>();
            //var item = ctx.GetCurrentItem<Details>();

            return PartialView("~/Views/Renderings/Home/Posts/Details/Details.cshtml", item);
        }
    }
}
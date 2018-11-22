using ProjectBlog.Models;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Threading;
using System.Web.Mvc;

namespace ProjectBlog.Controllers
{
    public class SliderController : Controller
    {
        private Item GetItem(string id)
        {
            return Context.Database.GetItem(id);
        }
        // GET: Slider
        public ActionResult InitSlider()
        {
            Thread.Sleep(3000);
            try
            {
                throw new Exception("Artificial exception");
            }
            catch(Exception ex)
            {
                Log.Fatal(ex.StackTrace, this);
            }
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = GetItem(dataSourceId);
            MultilistField imagesList = dataSource.Fields["Images"];
            Item[] images = imagesList.GetItems();
            SliderViewModel slider = new SliderViewModel();
            foreach (Item image in images)
            {
                string imageUrl = MediaManager.GetMediaUrl(image);
                slider.Urls.Add(imageUrl);
            }
            var parameters = RenderingContext.CurrentOrNull.Rendering.Parameters;
            var slideSpeedItem = parameters["Slide speed"];
            var slideSpeed = GetItem(slideSpeedItem);
            var speed = slideSpeed.Fields["Speed"].Value;
            if (int.TryParse(speed, out int intSpeed))
            {
                slider.Speed = intSpeed * 1000;
            }
            return PartialView("~/Views/Renderings/Slider.cshtml", slider);
        }

        public ActionResult InitHeroBanner()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = GetItem(dataSourceId);
            MultilistField imagesList = dataSource.Fields["Images"];
            Item[] images = imagesList.GetItems();
            string url = null;
            if (images.Length != 0)
            {
                Random random = new Random();
                int number = random.Next(images.Length);
                url = MediaManager.GetMediaUrl(images[number]);
            }
            return PartialView("~/Views/Renderings/HeroBanner.cshtml", url);
        }
    }
}
using OctopusLibrary;
using OctopusLibrary.Filters;
using Parkheesung.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class XMLController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return Redirect("/XML/SiteMap");
        }

        [XMLDocument]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult SiteMap()
        {
            string Now = DateTime.Now.ToString("yyyy-MM-dd");
            SiteMapData map = new SiteMapData();
            map.Add(new SitemapURL(SiteUtility.Get(), Now, SiteMapData.Daily));
            map.Add(new SitemapURL(SiteUtility.Get("Profile", "Index"), Now, SiteMapData.Monthly));
            map.Add(new SitemapURL(SiteUtility.Get("Profile", "Career"), Now, SiteMapData.Monthly));
            map.Add(new SitemapURL(SiteUtility.Get("Profile", "Link"), Now, SiteMapData.Monthly));
            return View(map);
        }
    }
}
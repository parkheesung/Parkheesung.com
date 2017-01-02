using OctopusLibrary.Filters;
using Parkheesung.Domain.Abstract;
using Parkheesung.WebUI.Abstract;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class ProfileController : SubController
    {
        public ProfileController(IRepository rep, ISiteUtility site) : base(rep, site)
        {

        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            ViewBag.SubPage = "profile";
            return View();
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Career()
        {
            ViewBag.SubPage = "career";
            return View();
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Link()
        {
            ViewBag.SubPage = "link";
            return View();
        }
    }
}
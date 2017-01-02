using OctopusLibrary.Filters;
using Parkheesung.Domain.Abstract;
using Parkheesung.WebUI.Abstract;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class HomeController : DefaultController
    {
        public HomeController(IRepository rep, ISiteUtility site) : base(rep, site)
        {

        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            this.OnLoginMemberInfoFill();
            return View();
        }
    }
}
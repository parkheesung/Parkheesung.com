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

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
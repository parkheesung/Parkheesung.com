using Parkheesung.Domain.Abstract;
using Parkheesung.WebUI.Abstract;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class ProfileController : DefaultController
    {
        public ProfileController(IRepository rep, ISiteUtility site) : base(rep, site)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SNS()
        {
            return View();
        }
    }
}
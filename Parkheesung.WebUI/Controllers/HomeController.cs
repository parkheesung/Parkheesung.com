using OctopusLibrary.Filters;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            this.OnLoginMemberInfoFill();
            List<Github> list = await this.rep.GetGitHubsAsync(1, 5);
            return View(list);
        }
    }
}
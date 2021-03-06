﻿using OctopusLibrary.Filters;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class ProfileController : DefaultController
    {
        public ProfileController(IRepository rep, ISiteUtility site) : base(rep, site)
        {

        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            ViewBag.SubPage = "profile";

            this.OnLoginMemberInfoFill();

            return View();
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> Career()
        {
            ViewBag.SubPage = "career";

            this.OnLoginMemberInfoFill();

            List<Github> list = await this.rep.GetGitHubsAsync(1);

            return View(list);
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> Link()
        {
            ViewBag.SubPage = "link";

            this.OnLoginMemberInfoFill();

            List<Link> list = await this.rep.GetLinksAsync(1);

            return View(list);
        }
    }
}
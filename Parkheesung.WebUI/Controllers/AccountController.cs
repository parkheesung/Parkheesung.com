using Parkheesung.Domain.Abstract;
using Parkheesung.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class AccountController : SubController
    {
        public AccountController(IRepository rep, ISiteUtility site) : base(rep, site)
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
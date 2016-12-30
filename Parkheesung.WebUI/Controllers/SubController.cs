using Parkheesung.Domain.Abstract;
using Parkheesung.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class SubController : DefaultController
    {
        public SubController(IRepository _rep, ISiteUtility _site) : base(_rep, _site)
        {
            this.token = String.Empty;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.OnLoginMemberInfoFill();

            if (String.IsNullOrEmpty(this.token))
            {
                filterContext.Result = new RedirectResult("/Member/Login");
            }
            else
            {
                if (!(this.member != null && this.member.MemberID > 0))
                {
                    filterContext.Result = new RedirectResult("/Member/Login");
                }
            }
        }
    }
}
using Parkheesung.Domain;
using Parkheesung.WebUI.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Filters
{
    public class DoYouLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string token = String.Empty;
            HttpCookie myCookie = HttpContext.Current.Request.Cookies.Get(NameString.LoginCookie);
            if (myCookie != null && !String.IsNullOrEmpty(myCookie.Value))
            {
                token = OctopusLibrary.Crypto.AES256.Decrypt(myCookie.Value, PrivateMyInfo.Secret, true);
            }

            if (String.IsNullOrEmpty(token))
            {
                filterContext.Result = new RedirectResult("/Member/Login");
            }
        }
    }
}
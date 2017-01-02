using System;
using System.Web;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Filters
{
    public class XMLDocumentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Response.ContentType = "Text/XML";
            filterContext.HttpContext.Response.Charset = "UTF-8";
        }
    }
}
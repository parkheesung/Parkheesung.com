using OctopusLibrary;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Abstract;
using System;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class DefaultController : BaseController
    {
        public bool IsReal { get; set; }
        protected IRepository rep { get; set; }
        protected ISiteUtility site { get; set; }
        public Member member { get; set; }
        public string token { get; set; }
        public string SecretKey { get; set; }

        public DefaultController(IRepository _rep, ISiteUtility _site)
        {
            this.rep = _rep;
            this.site = _site;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.IsReal = this.GetSetting("IsReal").Equals("true");
            this.SecretKey = this.GetSetting("SecretKey");

            ViewBag.domain = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Host);
            ViewBag.Title = "Parkheesung";
            ViewBag.keywords = "박희성,Parkheesung,Parkheesung.com,프로그래머,개발자,웹개발자";
            ViewBag.description = "창작하는 개발자 박희성의 홈페이지입니다.";
            ViewBag.Attr = "style=\"background-color:#f1f2f6;\"";
            ViewBag.URL = Request.Url.Host;
            ViewBag.Image = string.Format("{0}://{1}/Content/img/FacebookShare.png", Request.Url.Scheme, Request.Url.Host);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            this.SetApplicationCache(filterContext, false);
        }

        protected virtual void OnLoginMemberInfoFill()
        {
            this.token = this.site.LoginTokenGet();

            if (!String.IsNullOrEmpty(this.token))
            {
                this.member = this.rep.GetMember(this.token);
                ViewBag.member = this.member;
            }
        }
    }
}
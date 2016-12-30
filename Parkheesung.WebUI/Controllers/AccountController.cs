using OctopusLibrary.Models;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Abstract;
using Parkheesung.WebUI.Models;
using System.Collections.Generic;
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
            if (this.site.SessionCheck(this.member))
            {
                return RedirectToAction("AccountList");
            }

            return View();
        }

        public ActionResult AccountList(string Keyword = "", long GroupID = -1)
        {
            if (!this.site.SessionCheck(this.member))
            {
                return RedirectToAction("Index");
            }

            

            return View();
        }

        [HttpPost]
        public JsonResult UserConfirm(string UserPWD)
        {
            ReturnData result = new ReturnData();

            if (this.member != null && this.member.MemberID > 0)
            {
                if (OctopusLibrary.Crypto.Sha512.ValidatePassword(UserPWD, member.Password))
                {
                    Session[NameString.LoginCookie] = member.UserToken;
                    result.Success(this.member.MemberID);
                }
                else
                {
                    result.Error("비밀번호가 일치하지 않습니다.");
                }
            }
            else
            {
                result.Error("로그인 후 이용해 주세요.");
            }

            return Json(result);
        }

    }
}
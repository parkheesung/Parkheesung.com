using OctopusLibrary.Filters;
using OctopusLibrary.Models;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Entities;
using Parkheesung.Domain.Models;
using Parkheesung.WebUI.Abstract;
using Parkheesung.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class MemberController : DefaultController
    {
        public MemberController(IRepository _rep, ISiteUtility _site) : base(_rep, _site)
        {

        }

        public ActionResult Index()
        {
            return RedirectToAction("Setup");
        }

        [Compress]
        [DoYouLogin]
        public ViewResult Setup()
        {
            this.OnLoginMemberInfoFill();

            Member member = this.rep.GetMember(this.site.LoginTokenGet());
            if (!(member != null && member.MemberID > 0))
            {
                Response.Redirect("/Home/Index");
                return null;
            }
            else
            {
                return View(member);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [Compress]
        public ActionResult NewPasswordToEmail()
        {
            return View();
        }

        [HttpPost]
        public JsonResult JoinMemberProc(JoinMember join)
        {
            ReturnData result = new ReturnData();

            if (ModelState.IsValid)
            {
                Member member = this.rep.JoinInfoConvertMember(join);
                result = this.rep.MemberAdd(member);
            }
            else
            {
                result.Error("필수값이 누락되었습니다.");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult JoinMemberWithFacebookProc(FacebookMember join)
        {
            ReturnData result = new ReturnData();

            if (ModelState.IsValid)
            {
                Member member = this.rep.JoinInfoConvertMemberWithFacebook(join);
                result = this.rep.MemberAdd(member);
            }
            else
            {
                result.Error("필수값이 누락되었습니다.");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoginMemberProc(string UserMail, string UserPWD)
        {
            ReturnData result = this.rep.MemberLogin(UserMail, UserPWD);

            if (result.Check)
            {
                this.rep.MemberLoginLogRegist(result.Code, this.site.GetUserIP());
                this.site.LoginCookieSave(result.Value);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ExternalMemberInfoCheck(string token)
        {
            Member member = new Member();

            if (!String.IsNullOrEmpty(token))
            {
                string Token = this.site.DecryptToken(token);

                if (!String.IsNullOrEmpty(Token))
                {
                    member = this.rep.GetMemberFromExternalToken(Token);
                    member.Password = "";
                    member.UserToken = "";
                }
            }

            return Json(member, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExternalLoginMemberProc(string UserMail, string UserPWD)
        {
            ReturnData result = this.rep.MemberLoginFromExternal(UserMail, UserPWD);

            if (result.Check)
            {
                this.rep.MemberLoginLogRegist(result.Code, this.site.GetUserIP());
                result.Value = this.site.CryptoToken(result.Value);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExternalLoginFacebookMemberProc(string FacebookID, string token)
        {
            ReturnData result = this.rep.MemberLoginFromFacebookExternal(FacebookID, token);

            if (result.Check)
            {
                this.rep.MemberLoginLogRegist(result.Code, this.site.GetUserIP());
                result.Value = this.site.CryptoToken(result.Value);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public RedirectResult Logout()
        {
            this.site.LoginCookieErase();
            return new RedirectResult("/Home/Index");
        }

        [HttpPost]
        public JsonResult NewPasswordToEmailProc(string UserMail)
        {
            ReturnData result = new ReturnData();

            if (!String.IsNullOrEmpty(UserMail))
            {
                Member member = this.rep.FindEmail(UserMail);
                if (member != null && member.MemberID > 0)
                {
                    var rtn = this.rep.GetNewPassword(member.MemberID);
                    if (rtn.Check)
                    {
                        result = this.site.SendMail(member.Email, rtn.Value);
                    }
                    else
                    {
                        result = rtn;
                    }
                }
                else
                {
                    result.Error("해당 이메일로 가입된 정보를 찾을 수 없습니다.");
                }
            }
            else
            {
                result.Error("이메일 주소를 입력해 주세요.");
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult FindUserWithFacebook(string email, string facebookID)
        {
            ReturnData result = new ReturnData();

            Member member = this.rep.FindEmailWithFacebookID(email, facebookID);
            if (member != null && member.MemberID > 0)
            {
                result.Success(member.MemberID, "", member.FacebookID);
            }
            else
            {
                result.Error("해당 페이스북 계정으로 가입된 내역이 없습니다.");
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult SaveSetupProc(SetupMember member)
        {
            ReturnData result = new ReturnData();

            this.OnLoginMemberInfoFill();

            if (String.IsNullOrEmpty(this.token))
            {
                result.Error("로그인 후 시도해 주세요.");
            }
            else
            {
                if (!(this.member != null && this.member.MemberID > 0))
                {
                    result.Error("로그인 후 시도해 주세요.");
                }
                else
                {
                    try
                    {
                        result = this.rep.UpdateMemberInfo(member, this.member.MemberID);
                    }
                    catch (Exception ex)
                    {
                        result.Error(ex + Convert.ToString(this.member.MemberID));
                    }
                }
            }

            return Json(result);
        }
    }
}
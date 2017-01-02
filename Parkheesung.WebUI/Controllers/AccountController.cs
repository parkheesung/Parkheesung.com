using OctopusLibrary;
using OctopusLibrary.Filters;
using OctopusLibrary.Models;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Abstract;
using Parkheesung.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Controllers
{
    public class AccountController : SubController
    {
        public AccountController(IRepository rep, ISiteUtility site) : base(rep, site)
        {
        }

        private bool SessionCheck()
        {
            if (Session[NameString.LoginCookie] == null)
            {
                return false;
            }
            else
            {
                if (member.UserToken.Equals(Convert.ToString(Session[NameString.LoginCookie])))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            if (SessionCheck())
            {
                return RedirectToAction("List");
            }

            return View();
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> List(string Keyword = "", long GroupID = -1)
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index");
            }

            var accountList = await this.rep.GetAccountListAsync(this.member.MemberID, Keyword, GroupID);
            var groupList = await this.rep.GetAccountGroupListAsync(this.member.MemberID);

            ViewBag.Keyword = Keyword;
            ViewBag.GroupID = GroupID;
            ViewBag.groupList = groupList;

            return View(accountList);
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> View(long AccountID, long GroupID = -1, string Keyword = "")
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index");
            }

            var groupList = await this.rep.GetAccountGroupListAsync(this.member.MemberID);
            var accountInfo = await this.rep.GetAccountAsync(this.member.MemberID, AccountID);

            ViewBag.Keyword = Keyword;
            ViewBag.GroupID = GroupID;
            ViewBag.groupList = groupList;

            return View(accountInfo);
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> Regist(long GroupID = -1)
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index");
            }

            var groupList = await this.rep.GetAccountGroupListAsync(this.member.MemberID);

            ViewBag.GroupID = GroupID;

            return View(groupList);
        }

        [Compress]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> ManagedGroup(long GroupID = -1)
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index");
            }

            var groupList = await this.rep.GetAccountGroupListAsync(this.member.MemberID);

            ViewBag.GroupID = GroupID;

            return View(groupList);
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

        [HttpPost]
        public JsonResult GroupSave(string GroupName, long GroupID = -1)
        {
            ReturnData result = new ReturnData();
            if (this.member != null && this.member.MemberID > 0)
            {
                result = this.rep.AccountGroupSave(this.member.MemberID, GroupName, GroupID);
            }
            else
            {
                result.Error("로그인 후 이용해 주세요.");
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GroupRemove(long GroupID = -1)
        {
            ReturnData result = new ReturnData();
            if (this.member != null && this.member.MemberID > 0)
            {
                result = this.rep.AccountGroupRemove(this.member.MemberID, GroupID);
            }
            else
            {
                result.Error("로그인 후 이용해 주세요.");
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult AccountSaveProc(Account account)
        {
            ReturnData result = new ReturnData();
            if (this.member != null && this.member.MemberID > 0)
            {
                result = this.rep.AccountSave(this.member.MemberID, account);
            }
            else
            {
                result.Error("로그인 후 이용해 주세요.");
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult AccountRemove(long AccountID)
        {
            ReturnData result = new ReturnData();
            if (this.member != null && this.member.MemberID > 0)
            {
                result = this.rep.AccountRemove(this.member.MemberID, AccountID);
            }
            else
            {
                result.Error("로그인 후 이용해 주세요.");
            }
            return Json(result);
        }
    }
}
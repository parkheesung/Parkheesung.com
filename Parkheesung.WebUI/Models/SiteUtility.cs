﻿using OctopusLibrary;
using OctopusLibrary.Models;
using Parkheesung.Domain;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Abstract;
using Parkheesung.WebUI.Controllers;
using System;
using System.Configuration;
using System.Text;
using System.Web;

namespace Parkheesung.WebUI.Models
{
    public class SiteUtility : ISiteUtility
    {
        private string Secret { get; set; }
        public SiteUtility()
        {
            this.Secret = ConfigurationManager.AppSettings["SecretKey"];
        }

        public string GetUserIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        public string CryptoToken(string originalString)
        {
            return OctopusLibrary.Crypto.AES256.Encrypt(originalString, this.Secret, true);
        }

        public string DecryptToken(string cryptoString)
        {
            return OctopusLibrary.Crypto.AES256.Decrypt(cryptoString, this.Secret, true);
        }

        public void LoginCookieSave(string CookieValue)
        {
            HttpCookie myCookie = new HttpCookie(NameString.LoginCookie);
            myCookie.Value = this.CryptoToken(CookieValue);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public void LoginCookieErase()
        {
            HttpCookie myCookie = new HttpCookie(NameString.LoginCookie);
            myCookie.Expires = DateTime.Now.AddDays(-1d);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public string LoginTokenGet()
        {
            string result = String.Empty;
            HttpCookie myCookie = HttpContext.Current.Request.Cookies.Get(NameString.LoginCookie);
            if (myCookie != null && !String.IsNullOrEmpty(myCookie.Value))
            {
                result = OctopusLibrary.Crypto.AES256.Decrypt(myCookie.Value, this.Secret, true);
            }
            return result;
        }

        public ReturnData SendMail(string toMail, string Code)
        {
            ReturnData result = new ReturnData();

            using (var mail = new MailSender())
            {
                try
                {
                    mail.FromMailSet("no-reply@vNote.net", "no-reply@vNote.net");
                    mail.AddMail(toMail);
                    mail.Subject = "[vNote] 비밀번호찾기 메일입니다.";
                    StringBuilder builder = new StringBuilder(128);
                    builder.Append(OctopusLibrary.Utility.FileHandler.ReadFile(HttpContext.Current.Server.MapPath("~/Content/HTML/newPasswordMailForm.html"), Encoding.UTF8));
                    builder.Replace("{Domain}", String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host));
                    builder.Replace("{Code}", Code);
                    mail.Message = builder.ToString();
                    mail.Send();
                    result.Success(1, "메일이 발송되었습니다.");
                }
                catch (Exception ex)
                {
                    result.Error(ex);
                }
            }

            return result;
        }

        public Member Me(DefaultController controller)
        {
            return controller.member;
        }

        public static string Get(params string[] parameters)
        {
            StringBuilder domain = new StringBuilder();
            domain.AppendFormat("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host);
            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    domain.AppendFormat("/{0}", parameters[i]);
                }
            }
            return domain.ToString();
        }
    }
}
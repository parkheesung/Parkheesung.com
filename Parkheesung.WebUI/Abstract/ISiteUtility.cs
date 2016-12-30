using OctopusLibrary.Models;
using Parkheesung.Domain.Entities;
using Parkheesung.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parkheesung.WebUI.Abstract
{
    public interface ISiteUtility
    {
        void LoginCookieSave(string CookieValue);
        void LoginCookieErase();
        string LoginTokenGet();
        string GetUserIP();
        ReturnData SendMail(string toMail, string Code);
        Member Me(DefaultController controller);
        string CryptoToken(string originalString);
        string DecryptToken(string cryptoString);
    }
}

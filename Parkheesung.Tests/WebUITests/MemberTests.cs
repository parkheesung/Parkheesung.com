using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parkheesung.WebUI.Controllers;
using Parkheesung.Domain.Abstract;
using Moq;
using Parkheesung.Domain.Entities;
using OctopusLibrary.Models;
using Parkheesung.Domain.Models;
using System.Web.Mvc;
using Parkheesung.WebUI.Abstract;

namespace Parkheesung.Tests.WebUITests
{
    [TestClass]
    public class MemberTests
    {
        [TestMethod]
        public void WebUI_Member_JoinMemberProcTest()
        {
            //Assign
            JoinMember join = new JoinMember
            {
                Email = "Test3@Test.com",
                Name = "홍길동",
                Password = "Test1234"
            };

            Member member = new Member
            {
                Email = join.Email,
                Name = join.Name,
                Password = OctopusLibrary.Crypto.Sha512.CreateHash(join.Password),
                RegDate = DateTime.Now,
                IsEnabled = true,
                UserToken = Guid.NewGuid().ToString()
            };

            ReturnData rtn = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = String.Empty,
                Value = String.Empty
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.MemberAdd(member)).Returns(rtn);
            rep.Setup(m => m.JoinInfoConvertMember(join)).Returns(member);

            //Act
            MemberController target = new MemberController(rep.Object, null);
            JsonResult result = target.JoinMemberProc(join);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
            Assert.AreEqual(rst.Code, rtn.Code);
        }

        [TestMethod]
        public void WebUI_Member_LoginMemberProcTest()
        {
            //Assign
            string UserMail = "Test3@test.com";
            string UserPWD = "Test1234";
            string UserIP = "127.0.0.1";

            ReturnData rtn1 = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = "",
                Value = Guid.NewGuid().ToString()
            };

            ReturnData rtn2 = new ReturnData
            {
                Check = true,
                Code = 2,
                Message = "",
                Value = ""
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.MemberLogin(UserMail, UserPWD)).Returns(rtn1);
            rep.Setup(m => m.MemberLoginLogRegist(rtn1.Code, UserIP)).Returns(rtn2);

            Mock<ISiteUtility> site = new Mock<ISiteUtility>();
            site.Setup(m => m.LoginCookieSave(rtn1.Value));
            site.Setup(m => m.GetUserIP()).Returns(UserIP);

            //Act
            MemberController target = new MemberController(rep.Object, site.Object);
            JsonResult result = target.LoginMemberProc(UserMail, UserPWD);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
            Assert.AreEqual(rst.Code, rtn1.Code);
        }

        [TestMethod]
        public void WebUI_Member_ExternalLoginMemberProcTest()
        {
            //Assign
            string UserMail = "Test3@test.com";
            string UserPWD = "Test1234";
            string UserIP = "127.0.0.1";
            string Token = "testtoken";

            ReturnData rtn1 = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = "",
                Value = Guid.NewGuid().ToString()
            };

            ReturnData rtn2 = new ReturnData
            {
                Check = true,
                Code = 2,
                Message = "",
                Value = ""
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.MemberLoginFromExternal(UserMail, UserPWD)).Returns(rtn1);
            rep.Setup(m => m.MemberLoginLogRegist(rtn1.Code, UserIP)).Returns(rtn2);

            Mock<ISiteUtility> site = new Mock<ISiteUtility>();
            site.Setup(m => m.CryptoToken(rtn1.Value)).Returns(Token);
            site.Setup(m => m.GetUserIP()).Returns(UserIP);

            //Act
            MemberController target = new MemberController(rep.Object, site.Object);
            JsonResult result = target.ExternalLoginMemberProc(UserMail, UserPWD);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
            Assert.AreEqual(rst.Code, rtn1.Code);
        }

        [TestMethod]
        public void WebUI_Member_ExternalMemberInfoCheckTest()
        {
            //Assign
            string Token = "testtoken";
            string DecToken = "complete";

            Member member = new Member
            {
                MemberID = 1,
                Email = "Test3@test.com",
                Name = "홍길동",
                Password = OctopusLibrary.Crypto.Sha512.CreateHash("testpass"),
                RegDate = DateTime.Now,
                IsEnabled = true,
                UserToken = Guid.NewGuid().ToString()
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.GetMemberFromExternalToken(DecToken)).Returns(member);

            Mock<ISiteUtility> site = new Mock<ISiteUtility>();
            site.Setup(m => m.DecryptToken(Token)).Returns(DecToken);
            
            //Act
            MemberController target = new MemberController(rep.Object, site.Object);
            JsonResult result = target.ExternalMemberInfoCheck(Token);
            Member rst = result.Data as Member;

            //Assert
            Assert.IsNotNull(rst);
            Assert.AreEqual(rst.MemberID, member.MemberID);
        }

        [TestMethod]
        public void WebUI_Member_LogoutTest()
        {
            //Assign
            Mock<ISiteUtility> site = new Mock<ISiteUtility>();
            site.Setup(m => m.LoginCookieErase());

            //Act
            MemberController target = new MemberController(null, site.Object);
            RedirectResult result = target.Logout();
            string result_uri = "/Home/Index";

            //Assert
            Assert.AreEqual(result_uri, result.Url);
        }

        [TestMethod]
        public void WebUI_Member_SetupTest()
        {
            //Assign
            string token = "TestToken";

            Member member = new Member
            {
                MemberID = 1,
                Name = "홍길동",
                UserToken = token,
                IsEnabled = true
            };

            Mock<ISiteUtility> site = new Mock<ISiteUtility>();
            site.Setup(m => m.LoginTokenGet()).Returns(token);

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.GetMember(token)).Returns(member);

            //Act
            MemberController target = new MemberController(rep.Object, site.Object);
            ViewResult result = target.Setup();
            Member rtn = result.Model as Member;

            //Assert
            Assert.IsNotNull(rtn);
            Assert.IsInstanceOfType(rtn, typeof(Member));
            Assert.AreEqual(member.MemberID, rtn.MemberID);
        }

        [TestMethod]
        public void WebUI_Member_NewPasswordToEmailProcTest()
        {
            //Assign
            Member member = new Member
            {
                MemberID = 1,
                Email = "test@test.com",
                Name = "홍길동"
            };

            ReturnData rtn1 = new ReturnData
            {
                Check = true,
                Code = 1,
                Value = "test",
                Message = ""
            };

            ReturnData rtn2 = new ReturnData
            {
                Check = true,
                Code = 1,
                Value = "test",
                Message = ""
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.FindEmail(member.Email)).Returns(member);
            rep.Setup(m => m.GetNewPassword(member.MemberID)).Returns(rtn1);
            Mock<ISiteUtility> site = new Mock<ISiteUtility>();
            site.Setup(m => m.SendMail(member.Email, rtn1.Value)).Returns(rtn2);

            //Act
            MemberController target = new MemberController(rep.Object, site.Object);
            JsonResult result = target.NewPasswordToEmailProc(member.Email);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsNotNull(rst);
            Assert.IsTrue(rst.Check);
            Assert.AreEqual(rst.Code, rtn2.Code);
        }

        [TestMethod]
        public void WebUI_Member_FindUserWithFacebookTest()
        {
            //Assign
            Member member = new Member
            {
                MemberID = 1,
                Email = "test@test.com",
                Name = "홍길동",
                FacebookID = "1010101010"
            };

            ReturnData rtn = new ReturnData
            {
                Check = true,
                Code = 1,
                Value = "test",
                Message = ""
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.FindEmailWithFacebookID(member.Email, member.FacebookID)).Returns(member);

            //Act
            MemberController target = new MemberController(rep.Object, null);
            JsonResult result = target.FindUserWithFacebook(member.Email, member.FacebookID);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsNotNull(rtn);
            Assert.IsNotNull(rst);
            Assert.IsTrue(rst.Check);
            Assert.AreEqual(rst.Code, rtn.Code);
        }
    }
}

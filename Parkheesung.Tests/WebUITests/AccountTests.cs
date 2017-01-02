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
    public class AccountTests
    {
        private Member member;
        public AccountTests()
        {
            this.member = new Member
            {
                MemberID = 1,
                Email = "Test3@Test.com",
                Name = "홍길동",
                Password = OctopusLibrary.Crypto.Sha512.CreateHash("Test1234"),
                RegDate = DateTime.Now,
                IsEnabled = true,
                UserToken = Guid.NewGuid().ToString()
            };
        }

        [TestMethod]
        public void WebUI_Account_GroupRegistTest()
        {
            //Assign
            string GroupName = "Test";
            long GroupID = -1;

            ReturnData rtn = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = String.Empty,
                Value = String.Empty
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.AccountGroupSave(this.member.MemberID, GroupName, GroupID)).Returns(rtn);

            //Act
            AccountController target = new AccountController(rep.Object, null);
            target.member = this.member;
            JsonResult result = target.GroupSave(GroupName, GroupID);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
        }

        [TestMethod]
        public void WebUI_Account_GroupRemoveTest()
        {
            //Assign
            long GroupID = -1;

            ReturnData rtn = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = String.Empty,
                Value = String.Empty
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.AccountGroupRemove(this.member.MemberID, GroupID)).Returns(rtn);

            //Act
            AccountController target = new AccountController(rep.Object, null);
            target.member = this.member;
            JsonResult result = target.GroupRemove(GroupID);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
        }

        [TestMethod]
        public void WebUI_Account_SaveTest()
        {
            //Assign
            ReturnData rtn = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = String.Empty,
                Value = String.Empty
            };

            Account account = new Account()
            {
                AccessURL = "test.com",
                GroupID = 1,
                Memo = "",
                Title = "Test",
                UserID = "Test",
                UserPWD = "Test1234",
            };

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.AccountSave(this.member.MemberID, account)).Returns(rtn);

            //Act
            AccountController target = new AccountController(rep.Object, null);
            target.member = this.member;
            JsonResult result = target.AccountSaveProc(account);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
        }

        [TestMethod]
        public void WebUI_Account_RemoveTest()
        {
            //Assign
            ReturnData rtn = new ReturnData
            {
                Check = true,
                Code = 1,
                Message = String.Empty,
                Value = String.Empty
            };

            long AccountID = 1;

            Mock<IRepository> rep = new Mock<IRepository>();
            rep.Setup(m => m.AccountRemove(this.member.MemberID, AccountID)).Returns(rtn);

            //Act
            AccountController target = new AccountController(rep.Object, null);
            target.member = this.member;
            JsonResult result = target.AccountRemove(AccountID);
            ReturnData rst = result.Data as ReturnData;

            //Assert
            Assert.IsTrue(rst.Check);
        }
    }
}

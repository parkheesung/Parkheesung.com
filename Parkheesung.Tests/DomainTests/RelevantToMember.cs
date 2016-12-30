using Microsoft.VisualStudio.TestTools.UnitTesting;
using OctopusLibrary.Models;
using Parkheesung.Domain.Entities;
using Parkheesung.Domain.Models;
using Parkheesung.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace Parkheesung.Tests.DomainTests
{
    [TestClass]
    public class RelevantToMember
    {
        [TestMethod]
        public async Task Domain_Member_JoinTest()
        {
            //Assign
            Member test = new Member
            {
                Email = "Test1@test.com",
                Password = OctopusLibrary.Crypto.Sha512.CreateHash("testuser1234"),
                Name = "홍길동",
                IsEnabled = true,
                RegDate = DateTime.Now,
                UserToken = Guid.NewGuid().ToString()
            };

            EntityRepository rep = new EntityRepository();

            //Act
            ReturnData check = await rep.MemberAddAsync(test);

            //Assert
            Assert.IsTrue(check.Check);
            Assert.IsTrue(check.Code > 0);

            //finish
            rep.EraseMember(check.Code);
        }

        [TestMethod]
        public async Task Domain_Member_LoginTest()
        {
            //Assign
            Member test = new Member
            {
                Email = "Test2@test.com",
                Password = OctopusLibrary.Crypto.Sha512.CreateHash("testuser1234"),
                Name = "홍길동",
                IsEnabled = true,
                RegDate = DateTime.Now,
                UserToken = Guid.NewGuid().ToString()
            };

            EntityRepository rep = new EntityRepository();
            ReturnData rtn = rep.MemberAdd(test);

            //Act
            ReturnData check = await rep.MemberLoginAsync(test.Email, "testuser1234");

            //Assert
            Assert.IsTrue(check.Check);
            Assert.IsTrue(check.Code > 0);

            //finish
            rep.EraseMember(rtn.Code);
        }

        [TestMethod]
        public void Domain_Member_UpdateMemberInfoTest()
        {
            //Assign
            Member test = new Member
            {
                Email = "Test4@test.com",
                Password = OctopusLibrary.Crypto.Sha512.CreateHash("testuser1234"),
                Name = "홍길동",
                IsEnabled = true,
                RegDate = DateTime.Now,
                UserToken = Guid.NewGuid().ToString()
            };

            SetupMember setup = new SetupMember
            {
                Name = test.Name,
                NowPass = "testuser1234",
                NewPass = "12341234"
            };

            EntityRepository rep = new EntityRepository();
            ReturnData rtn = rep.MemberAdd(test);

            //Act
            ReturnData check = rep.UpdateMemberInfo(setup, rtn.Code);

            //Assert
            Assert.IsTrue(check.Check);
            Assert.IsTrue(check.Code > 0);

            //finish
            rep.EraseMember(rtn.Code);
        }
    }
}

using OctopusLibrary.Models;
using Parkheesung.Domain.Entities;
using Parkheesung.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parkheesung.Domain.Abstract
{
    public interface IRepository
    {
        ReturnData MemberAdd(Member member);
        Task<ReturnData> MemberAddAsync(Member member);
        ReturnData MemberLogin(string Email, string Password);
        Task<ReturnData> MemberLoginAsync(string Email, string Password);
        ReturnData MemberLoginLogRegist(long MemberID, string UserIP);
        Member GetMember(string token);
        Member GetMember(long MemberID);
        ReturnData EraseMember(long MemberID);
        Member JoinInfoConvertMember(JoinMember join);
        Member JoinInfoConvertMemberWithFacebook(FacebookMember join);
        Member FindEmail(string Email);
        ReturnData GetNewPassword(long MemberID);
        Member FindEmailWithFacebookID(string Email, string FacebookID);
    }
}

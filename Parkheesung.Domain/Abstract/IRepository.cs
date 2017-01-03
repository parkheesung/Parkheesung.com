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
        ReturnData MemberLoginFromExternal(string Email, string Password);
        ReturnData MemberLoginFromFacebookExternal(string FacebookID, string token);
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
        ReturnData UpdateMemberInfo(SetupMember setup, long MemberID);
        Member GetMemberFromExternalToken(string Token);

        List<AccountView> GetAccountList(long MemberID, string Keyword = "", long GroupID = -1);
        Task<List<AccountView>> GetAccountListAsync(long MemberID, string Keyword = "", long GroupID = -1);
        List<AccountGroupView> GetAccountGroupList(long MemberID);
        Task<List<AccountGroupView>> GetAccountGroupListAsync(long MemberID);
        ReturnData AccountGroupSave(long MemberID, string GroupName, long GroupID = -1);
        ReturnData AccountGroupRemove(long MemberID, long GroupID);
        ReturnData AccountSave(long MemberID, Account account);
        Account GetAccount(long MemberID, long AccountID);
        Task<Account> GetAccountAsync(long MemberID, long AccountID);
        ReturnData AccountRemove(long MemberID, long AccountID);
        ReturnData CreateTokenAuth(long MemberID);

        List<Github> GetGitHubs(long MemberID);
        Task<List<Github>> GetGitHubsAsync(long MemberID);
        List<Github> GetGitHubs(long MemberID, int TopCount);
        Task<List<Github>> GetGitHubsAsync(long MemberID, int TopCount);
        List<Link> GetLinks(long MemberID);
        Task<List<Link>> GetLinksAsync(long MemberID);
        List<Link> GetLinks(long MemberID, int TopCount);
        Task<List<Link>> GetLinksAsync(long MemberID, int TopCount);
    }
}

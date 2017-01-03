using Parkheesung.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctopusLibrary.Models;
using Parkheesung.Domain.Entities;
using Parkheesung.Domain.Models;
using Parkheesung.Domain.Database;
using System.Data.SqlClient;
using OctopusLibrary.Utility;

namespace Parkheesung.Domain.Repository
{
    public class EntityRepository : IRepository
    {
        #region [회원관련]

        /// <summary>
        /// 회원가입 프로세스
        /// </summary>
        /// <param name="member">가입할 정보</param>
        /// <returns>ReturnData</returns>
        public ReturnData MemberAdd(Member member)
        {
            ReturnData result = new ReturnData();

            if (member == null)
            {
                result.Error("입력 정보가 올바르지 않습니다. 다시 입력해 주세요.");
            }
            else if (String.IsNullOrEmpty(member.Email))
            {
                result.Error("이메일 정보를 입력해 주세요.");
            }
            else if (String.IsNullOrEmpty(member.Name))
            {
                result.Error("이름을 입력하세요.");
            }
            else if (String.IsNullOrEmpty(member.Password))
            {
                result.Error("비밀번호를 입력하세요.");
            }
            else
            {
                using (var context = new EFDbContext())
                {
                    if (context.Members.Where(x => x.Email.ToLower() == member.Email.ToLower()).Count() > 0)
                    {
                        result.Error("이미 사용중인 이메일 입니다.");
                    }
                    else
                    {
                        context.Members.Add(member);
                        context.SaveChanges();
                        result.Success(member.MemberID);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 회원가입 프로세스 (비동기)
        /// </summary>
        /// <param name="member">가입할 정보</param>
        /// <returns>ReturnData</returns>
        public Task<ReturnData> MemberAddAsync(Member member)
        {
            return Task.Factory.StartNew(() => MemberAdd(member));
        }

        /// <summary>
        /// 로그인 처리 프로세스
        /// </summary>
        /// <param name="Email">회원 이메일</param>
        /// <param name="Password">비밀번호</param>
        /// <returns>ReturnData</returns>
        public ReturnData MemberLogin(string Email, string Password)
        {
            ReturnData result = new ReturnData();

            if (String.IsNullOrEmpty(Email))
            {
                result.Error("이메일 정보를 입력해 주세요.");
            }
            else if (String.IsNullOrEmpty(Password))
            {
                result.Error("이름을 입력하세요.");
            }
            else
            {
                using (var context = new EFDbContext())
                {
                    Member member = context.Members.Where(x => x.IsEnabled == true)
                                                   .Where(x => x.Email.ToLower() == Email.ToLower())
                                                   .FirstOrDefault();

                    if (member != null && member.MemberID > 0)
                    {
                        if (OctopusLibrary.Crypto.Sha512.ValidatePassword(Password, member.Password))
                        {
                            result.Success(member.MemberID, "", member.UserToken);
                        }
                        else if (member.IsFacebook && member.FacebookID.Equals(Password))
                        {
                            result.Success(member.MemberID, "", member.UserToken);
                        }
                        else
                        {
                            result.Error("비밀번호가 일치하지 않습니다.");
                        }
                    }
                    else
                    {
                        result.Error("해당 이메일로 가입된 내역이 없습니다.");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 로그인 처리 프로세스
        /// </summary>
        /// <param name="Email">회원 이메일</param>
        /// <param name="Password">비밀번호</param>
        /// <returns>ReturnData</returns>
        public ReturnData MemberLoginFromExternal(string Email, string Password)
        {
            ReturnData result = new ReturnData();

            if (String.IsNullOrEmpty(Email))
            {
                result.Error("이메일 정보를 입력해 주세요.");
            }
            else if (String.IsNullOrEmpty(Password))
            {
                result.Error("이름을 입력하세요.");
            }
            else
            {
                using (var context = new EFDbContext())
                {
                    Member member = context.Members.Where(x => x.IsEnabled == true)
                                                   .Where(x => x.Email.ToLower() == Email.ToLower())
                                                   .FirstOrDefault();

                    if (member != null && member.MemberID > 0)
                    {
                        if (OctopusLibrary.Crypto.Sha512.ValidatePassword(Password, member.Password))
                        {
                            TokenAuth auth = new TokenAuth()
                            {
                                IsEnabled = true,
                                MemberID = member.MemberID,
                                ExpiredDate = DateTime.Now.AddMonths(1),
                                RegDate = DateTime.Now,
                                UserToken = Guid.NewGuid().ToString(),
                                FacebookToken = ""
                            };

                            context.TokenAuths.Add(auth);
                            context.SaveChanges();

                            if (auth.TokenAuthID > 0)
                            {
                                result.Success(member.MemberID, "", auth.UserToken);
                            }
                            else
                            {
                                result.Error("권한 발급이 실패하였습니다.");
                            }
                        }
                        else
                        {
                            result.Error("비밀번호가 일치하지 않습니다.");
                        }
                    }
                    else
                    {
                        result.Error("해당 이메일로 가입된 내역이 없습니다.");
                    }
                }
            }

            return result;
        }

        public ReturnData CreateTokenAuth(long MemberID)
        {
            ReturnData result = new ReturnData();

            using (var context = new EFDbContext())
            {
                Member member = context.Members.Where(x => x.IsEnabled == true)
                                               .Where(x => x.MemberID == MemberID)
                                               .FirstOrDefault();

                if (member != null && member.MemberID > 0)
                {
                    TokenAuth auth = new TokenAuth()
                    {
                        IsEnabled = true,
                        MemberID = member.MemberID,
                        ExpiredDate = DateTime.Now.AddMonths(1),
                        RegDate = DateTime.Now,
                        UserToken = Guid.NewGuid().ToString(),
                        FacebookToken = ""
                    };

                    context.TokenAuths.Add(auth);
                    context.SaveChanges();

                    if (auth.TokenAuthID > 0)
                    {
                        result.Success(member.MemberID, "", auth.UserToken);
                    }
                    else
                    {
                        result.Error("권한 발급이 실패하였습니다.");
                    }
                }
                else
                {
                    result.Error("해당 이메일로 가입된 내역이 없습니다.");
                }
            }

            return result;
        }

        public ReturnData MemberLoginFromFacebookExternal(string FacebookID, string token)
        {
            ReturnData result = new ReturnData();

            if (String.IsNullOrEmpty(FacebookID))
            {
                result.Error("페이스북 아이디를 입력해 주세요.");
            }
            else if (String.IsNullOrEmpty(token))
            {
                result.Error("토큰을 입력하세요.");
            }
            else
            {
                using (var context = new EFDbContext())
                {
                    Member member = context.Members.Where(x => x.IsEnabled == true)
                                                   .Where(x => x.FacebookID.Trim() == FacebookID.Trim())
                                                   .FirstOrDefault();

                    if (member != null && member.MemberID > 0)
                    {
                        TokenAuth auth = new TokenAuth()
                        {
                            IsEnabled = true,
                            MemberID = member.MemberID,
                            ExpiredDate = DateTime.Now.AddMonths(1),
                            RegDate = DateTime.Now,
                            UserToken = Guid.NewGuid().ToString(),
                            FacebookToken = token
                        };

                        context.TokenAuths.Add(auth);
                        context.SaveChanges();

                        if (auth.TokenAuthID > 0)
                        {
                            result.Success(member.MemberID, "", auth.UserToken);
                        }
                        else
                        {
                            result.Error("권한 발급이 실패하였습니다.");
                        }
                    }
                    else
                    {
                        result.Error("해당 이메일로 가입된 내역이 없습니다.");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 로그인 처리 프로세스 (비동기)
        /// </summary>
        /// <param name="Email">회원 이메일</param>
        /// <param name="Password">비밀번호</param>
        /// <returns>ReturnData</returns>
        public Task<ReturnData> MemberLoginAsync(string Email, string Password)
        {
            return Task.Factory.StartNew(() => MemberLogin(Email, Password));
        }

        /// <summary>
        /// 로그인 로그 기록
        /// </summary>
        /// <param name="MemberID">로그인한 사용자 고유번호</param>
        /// <param name="UserIP">로그인한 아이피</param>
        /// <returns>ReturnData</returns>
        public ReturnData MemberLoginLogRegist(long MemberID, string UserIP)
        {
            ReturnData result = new ReturnData();

            MemberLoginLog log = new MemberLoginLog
            {
                MemberID = MemberID,
                UserIP = UserIP,
                RegDate = DateTime.Now
            };

            using (var context = new EFDbContext())
            {
                context.MemberLoginLogs.Add(log);
                context.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// 회원정보 조회
        /// </summary>
        /// <param name="token">권한토큰</param>
        /// <returns>Member</returns>
        public Member GetMember(string token)
        {
            Member member = new Member();

            using (var context = new EFDbContext())
            {
                member = context.Members.Where(x => x.IsEnabled == true)
                                        .Where(x => x.UserToken == token)
                                        .FirstOrDefault();

                if (member == null || member.MemberID < 1)
                {
                    var auth = context.TokenAuths.Where(x => x.UserToken.Equals(token))
                                                              .Where(x => x.IsEnabled)
                                                              .Where(x => x.ExpiredDate >= DateTime.Now)
                                                              .FirstOrDefault();

                    if (auth != null && auth.TokenAuthID > 0)
                    {
                        member = context.Members.Where(x => x.IsEnabled == true)
                                                .Where(x => x.MemberID == auth.MemberID)
                                                .FirstOrDefault();
                    }
                }
            }

            return member;
        }

        /// <summary>
        /// 회원정보 조회
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns>Member</returns>
        public Member GetMember(long MemberID)
        {
            Member member = new Member();

            using (var context = new EFDbContext())
            {
                member = context.Members.Where(x => x.IsEnabled == true)
                                        .Where(x => x.MemberID == MemberID)
                                        .FirstOrDefault();
            }

            return member;
        }

        /// <summary>
        /// 해당 멤버를 삭제합니다.
        /// </summary>
        /// <param name="MemberID">멤버 고유번호</param>
        /// <returns>ReturnData</returns>
        public ReturnData EraseMember(long MemberID)
        {
            ReturnData result = new ReturnData();

            using (var context = new EFDbContext())
            {
                Member member = context.Members.Where(x => x.MemberID == MemberID).FirstOrDefault();
                if (member != null && member.MemberID > 0)
                {
                    context.Members.Remove(member);
                    context.SaveChanges();
                    result.Success(1);
                }
            }

            return result;
        }

        /// <summary>
        /// 가입정보를 회원정보 모델로 변경합니다.
        /// </summary>
        /// <param name="join">가입정보 모델</param>
        /// <returns>Member</returns>
        public Member JoinInfoConvertMember(JoinMember join)
        {
            Member member = new Member
            {
                Email = join.Email,
                Name = join.Name,
                IsEnabled = true,
                Password = OctopusLibrary.Crypto.Sha512.CreateHash(join.Password),
                RegDate = DateTime.Now,
                UserToken = Guid.NewGuid().ToString(),
                IsFacebook = false,
                FacebookID = ""
            };

            return member;
        }

        /// <summary>
        /// 회원정보 갱신
        /// </summary>
        /// <param name="setup">변경할 회원 정보</param>
        /// <param name="MemberID">회원 고유 번호</param>
        /// <returns>ReturnData</returns>
        public ReturnData UpdateMemberInfo(SetupMember setup, long MemberID)
        {
            ReturnData result = new ReturnData();

            using (var context = new EFDbContext())
            {
                Member member = context.Members.Where(x => x.MemberID == MemberID).FirstOrDefault();

                if (member != null && member.MemberID > 0)
                {
                    if (String.IsNullOrEmpty(setup.NowPass))
                    {
                        member.Name = setup.Name;
                        member.LastUpdate = DateTime.Now;
                        context.SaveChanges();
                        result.Success(member.MemberID);
                    }
                    else
                    {
                        if (OctopusLibrary.Crypto.Sha512.ValidatePassword(setup.NowPass, member.Password))
                        {
                            member.Name = setup.Name;
                            member.LastUpdate = DateTime.Now;
                            member.Password = OctopusLibrary.Crypto.Sha512.CreateHash(setup.NewPass);
                            context.SaveChanges();
                            result.Success(member.MemberID);
                        }
                        else
                        {
                            result.Error("비밀번호가 일치하지 않습니다.");
                        }
                    }
                }
                else
                {
                    result.Error("대상이 없습니다.");
                }
            }

            return result;
        }

        /// <summary>
        /// 이메일을 기준으로 회원정보를 찾습니다.
        /// </summary>
        /// <param name="Email">찾고자하는 이메일 주소</param>
        /// <returns>Member</returns>
        public Member FindEmail(string Email)
        {
            Member member = new Member();

            using (var context = new EFDbContext())
            {
                member = context.Members.Where(x => x.Email.ToLower() == Email.ToLower()).FirstOrDefault();
            }

            return member;
        }

        /// <summary>
        /// 이메일과 페이스북 아이디로 계정 조회
        /// </summary>
        /// <param name="Email">이메일</param>
        /// <param name="FacebookID">페이스북 아이디</param>
        /// <returns>Member</returns>
        public Member FindEmailWithFacebookID(string Email, string FacebookID)
        {
            Member member = new Member();

            using (var context = new EFDbContext())
            {
                member = context.Members.Where(x => x.Email.ToLower() == Email.ToLower())
                                        .Where(x => x.FacebookID == FacebookID)
                                        .FirstOrDefault();
            }

            return member;
        }

        /// <summary>
        /// 비밀번호 신규발급 (난수10자리)
        /// </summary>
        /// <param name="MemberID">회원고유번호</param>
        /// <returns>ReturnData</returns>
        public ReturnData GetNewPassword(long MemberID)
        {
            ReturnData result = new ReturnData();

            using (var context = new EFDbContext())
            {
                try
                {
                    Member member = context.Members.Where(x => x.MemberID == MemberID).FirstOrDefault();

                    if (member != null && member.MemberID > 0)
                    {
                        //임시 비밀번호를 난수 10자리로 생성합니다.
                        string newPass = Randomizer.CreateGenerate(10);
                        member.Password = OctopusLibrary.Crypto.Sha512.CreateHash(newPass);
                        member.LastUpdate = DateTime.Now;
                        context.SaveChanges();
                        result.Success(member.MemberID, "", newPass);
                    }
                    else
                    {
                        result.Error("대상을 찾을 수 없습니다.");
                    }
                }
                catch (SqlException sqlex)
                {
                    result.Error(sqlex);
                }
                catch (Exception ex)
                {
                    result.Error(ex);
                }
            }

            return result;
        }

        public Member JoinInfoConvertMemberWithFacebook(FacebookMember join)
        {
            if (String.IsNullOrEmpty(join.FacebookID))
            {
                return null;
            }
            else
            {

                Member member = new Member
                {
                    Email = join.Email,
                    Name = join.Name,
                    IsEnabled = true,
                    Password = OctopusLibrary.Crypto.Sha512.CreateHash(join.Password),
                    RegDate = DateTime.Now,
                    UserToken = Guid.NewGuid().ToString(),
                    IsFacebook = true,
                    FacebookID = join.FacebookID,
                    UserPhotoURL = String.Format("https://graph.facebook.com/{0}/picture?width=100&height=100", join.FacebookID)
                };

                return member;
            }
        }

        /// <summary>
        /// 외부에서 로그인 정보를 확인할 경우
        /// </summary>
        /// <param name="Token">외부 토큰</param>
        /// <returns>Member</returns>
        public Member GetMemberFromExternalToken(string Token)
        {
            Member member = new Member();

            if (!String.IsNullOrEmpty(Token))
            {
                using (var context = new EFDbContext())
                {
                    TokenAuth auth = context.TokenAuths.Where(x => x.UserToken == Token)
                                                                         .Where(x => x.IsEnabled)
                                                                         .Where(x => x.ExpiredDate > DateTime.Now)
                                                                         .FirstOrDefault();

                    if (auth != null && auth.TokenAuthID > 0)
                    {
                        member = context.Members.Where(x => x.MemberID == auth.MemberID).FirstOrDefault();
                    }
                }
            }

            return member;
        }

        #endregion [회원관련]

        #region [계정관리]

        public List<AccountView> GetAccountList(long MemberID, string Keyword = "", long GroupID = -1)
        {
            List<AccountView> result = new List<AccountView>();

            using (var context = new EFDbReadOnlyContext())
            {
                if (String.IsNullOrEmpty(Keyword))
                {
                    if (GroupID > 0)
                    {
                        result = context.AccountView.Where(x => x.MemberID == MemberID)
                                                                .Where(x => x.GroupID == GroupID)
                                                                .OrderByDescending(x => x.AccountID)
                                                                .ToList();
                    }                                      
                    else
                    {
                        result = context.AccountView.Where(x => x.MemberID == MemberID)
                                                                .OrderByDescending(x => x.AccountID)
                                                                .ToList();
                    }
                }
                else
                {
                    if (GroupID > 0)
                    {
                        result = context.AccountView.Where(x => x.MemberID == MemberID)
                                                                .Where(x => x.GroupID == GroupID)
                                                                .Where(x => x.Title.Contains(Keyword) || x.AccessURL.Contains(Keyword) || x.Memo.Contains(Keyword))
                                                                .OrderByDescending(x => x.AccountID)
                                                                .ToList();
                    }
                    else
                    {
                        result = context.AccountView.Where(x => x.MemberID == MemberID)
                                                                .Where(x => x.Title.Contains(Keyword) || x.AccessURL.Contains(Keyword) || x.Memo.Contains(Keyword))
                                                                .OrderByDescending(x => x.AccountID)
                                                                .ToList();
                    }
                }
            }

            return result;
        }

        public Task<List<AccountView>> GetAccountListAsync(long MemberID, string Keyword = "", long GroupID = -1)
        {
            return Task.Factory.StartNew(() => GetAccountList(MemberID, Keyword, GroupID));
        }

        public List<AccountGroupView> GetAccountGroupList(long MemberID)
        {
            List<AccountGroupView> result = new List<AccountGroupView>();

            result.Add(new AccountGroupView()
            {
                MemberID = MemberID,
                GroupID = -1,
                GroupCount = 0,
                GroupName = "전체보기"
            });

            using (var context = new EFDbReadOnlyContext())
            {
                var tmp = context.AccountGroupView.Where(x => x.MemberID == MemberID)
                                                                    .OrderBy(x => x.GroupName)
                                                                    .ToList();

                result.AddRange(tmp);
                result[0].GroupCount = tmp.Sum(x => x.GroupCount);
            }

            return result;
        }

        public Task<List<AccountGroupView>> GetAccountGroupListAsync(long MemberID)
        {
            return Task.Factory.StartNew(() => GetAccountGroupList(MemberID));
        }

        public Account GetAccount(long MemberID, long AccountID)
        {
            Account result = new Account();

            using (var context = new EFDbContext())
            {
                result = context.Accounts.Where(x => x.AccountID == AccountID)
                                                   .Where(x => x.MemberID == MemberID)
                                                   .Take(1)
                                                   .FirstOrDefault();
            }

            return result;
        }

        public Task<Account> GetAccountAsync(long MemberID, long AccountID)
        {
            return Task.Factory.StartNew(() => GetAccount(MemberID, AccountID));
        }

        public ReturnData AccountGroupSave(long MemberID, string GroupName, long GroupID = -1)
        {
            ReturnData result = new ReturnData();
            AccountGroup group = null;

            using (var context = new EFDbContext())
            {
                if (GroupID > 0)
                {
                    group = context.AccountGroups.Where(x => x.GroupID == GroupID).FirstOrDefault();
                    if (group != null || group.GroupID > 0)
                    {
                        if (group.MemberID == MemberID)
                        {
                            group.GroupName = GroupName;
                            group.LastUpdate = DateTime.Now;
                            context.SaveChanges();
                            result.Success(group.GroupID);
                        }
                        else
                        {
                            result.Error("수정할 권한이 없습니다.");
                        }
                    }
                    else
                    {
                        group = new AccountGroup()
                        {
                            GroupName = GroupName,
                            MemberID = MemberID,
                            RegDate = DateTime.Now,
                            LastUpdate = DateTime.Now
                        };
                        context.AccountGroups.Add(group);
                        context.SaveChanges();
                        result.Success(group.GroupID);
                    }
                }
                else
                {
                    group = new AccountGroup()
                    {
                        GroupName = GroupName,
                        MemberID = MemberID,
                        RegDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    };
                    context.AccountGroups.Add(group);
                    context.SaveChanges();
                    result.Success(group.GroupID);
                }
            }

            return result;
        }

        public ReturnData AccountGroupRemove(long MemberID, long GroupID)
        {
            ReturnData result = new ReturnData();
            AccountGroup group = null;

            using (var context = new EFDbContext())
            {
                group = context.AccountGroups.Where(x => x.GroupID == GroupID).FirstOrDefault();
                if (group != null && group.GroupID > 0)
                {
                    if (group.MemberID == MemberID)
                    {
                        context.AccountGroups.Remove(group);
                        context.SaveChanges();
                        result.Success(1);
                    }
                    else
                    {
                        result.Error("삭제할 권한이 없습니다.");
                    }
                }
                else
                {
                    result.Error("대상을 찾을 수 없습니다.");
                }
            }

            return result;
        }

        public ReturnData AccountSave(long MemberID, Account account)
        {
            ReturnData result = new ReturnData();

            using (var context = new EFDbContext())
            {
                if (account != null && account.AccountID > 0)
                {
                    Account tmp = context.Accounts.Where(x => x.AccountID == account.AccountID).FirstOrDefault();
                    if (tmp != null && tmp.AccountID == account.AccountID && tmp.MemberID == MemberID)
                    {
                        tmp.AccessURL = account.AccessURL;
                        tmp.Memo = account.Memo;
                        tmp.Title = account.Title;
                        tmp.UserID = account.UserID;
                        tmp.UserPWD = account.UserPWD;
                        tmp.GroupID = account.GroupID;
                        tmp.LastUpdate = DateTime.Now;
                        context.SaveChanges();
                        result.Success(tmp.AccountID, "", String.Format("{0}", account.GroupID));
                    }
                    else
                    {
                        result.Error("수정 권한이 없습니다.");
                    }                                         
                }
                else
                {
                    account.RegDate = DateTime.Now;
                    account.LastUpdate = DateTime.Now;
                    account.MemberID = MemberID;
                    context.Accounts.Add(account);
                    context.SaveChanges();
                    result.Success(account.AccountID, "", String.Format("{0}", account.GroupID));
                }
            }

            return result;
        }

        public ReturnData AccountRemove(long MemberID, long AccountID)
        {
            ReturnData result = new ReturnData();

            using (var context = new EFDbContext())
            {
                var account = context.Accounts.Where(x => x.AccountID == AccountID).FirstOrDefault();

                if (account != null && account.AccountID > 0)
                {
                    if (account.MemberID == MemberID)
                    {
                        context.Accounts.Remove(account);
                        context.SaveChanges();
                        result.Success(1);
                    }
                    else
                    {
                        result.Error("삭제할 권한이 없습니다.");
                    }
                }
                else
                {
                    result.Error("대상을 찾을 수 없습니다.");
                }
            }

            return result;
        }

        #endregion [계정관리]

        public List<Github> GetGitHubs(long MemberID)
        {
            List<Github> result = new List<Github>();

            using (var context = new EFDbContext())
            {
                result = context.Githubs.Where(x => x.MemberID == MemberID)
                                                 .OrderByDescending(x => x.GithubID)
                                                 .ToList();
            }

            return result;
        }

        public Task<List<Github>> GetGitHubsAsync(long MemberID)
        {
            return Task.Factory.StartNew(() => GetGitHubs(MemberID));
        }

        public List<Github> GetGitHubs(long MemberID, int TopCount)
        {
            List<Github> result = new List<Github>();

            using (var context = new EFDbContext())
            {
                result = context.Githubs.Where(x => x.MemberID == MemberID)
                                                 .OrderByDescending(x => x.GithubID)
                                                 .Take(TopCount)
                                                 .ToList();
            }

            return result;
        }

        public Task<List<Github>> GetGitHubsAsync(long MemberID, int TopCount)
        {
            return Task.Factory.StartNew(() => GetGitHubs(MemberID, TopCount));
        }

        public List<Link> GetLinks(long MemberID)
        {
            List<Link> result = new List<Link>();

            using (var context = new EFDbContext())
            {
                result = context.Links.Where(x => x.MemberID == MemberID)
                                              .OrderByDescending(x => x.LinkID)
                                              .ToList();
            }

            return result;
        }

        public Task<List<Link>> GetLinksAsync(long MemberID)
        {
            return Task.Factory.StartNew(() => GetLinks(MemberID));
        }

        public List<Link> GetLinks(long MemberID, int TopCount)
        {
            List<Link> result = new List<Link>();

            using (var context = new EFDbContext())
            {
                result = context.Links.Where(x => x.MemberID == MemberID)
                                              .OrderByDescending(x => x.LinkID)
                                              .Take(TopCount)
                                              .ToList();
            }

            return result;
        }

        public Task<List<Link>> GetLinksAsync(long MemberID, int TopCount)
        {
            return Task.Factory.StartNew(() => GetLinks(MemberID, TopCount));
        }
    }
}

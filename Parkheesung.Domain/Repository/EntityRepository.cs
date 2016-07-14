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
                        member.UserPhotoURL = setup.UserPhotoURL;
                        member.LastUpdate = DateTime.Now;
                        context.SaveChanges();
                        result.Success(member.MemberID);
                    }
                    else
                    {
                        if (OctopusLibrary.Crypto.Sha512.ValidatePassword(setup.NowPass, member.Password))
                        {
                            member.Name = setup.Name;
                            member.UserPhotoURL = setup.UserPhotoURL;
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

        #endregion [회원관련]
    }
}

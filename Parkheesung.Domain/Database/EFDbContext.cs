using Parkheesung.Domain.Entities;
using System.Data.Entity;

namespace Parkheesung.Domain.Database
{
    public class EFDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberLoginLog> MemberLoginLogs { get; set; }
        public DbSet<TokenAuth> TokenAuths { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }


        public EFDbContext() : base("name=EFDbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

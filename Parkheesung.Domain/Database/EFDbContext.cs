using Parkheesung.Domain.Entities;
using System.Data.Entity;

namespace Parkheesung.Domain.Database
{
    public class EFDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberLoginLog> MemberLoginLogs { get; set; }


        public EFDbContext() : base("name=EFDbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

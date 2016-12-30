using Parkheesung.Domain.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Parkheesung.Domain.Database
{
    public class EFDbReadOnlyContext : DbContext
    {
        public EFDbReadOnlyContext() : base("name=EFDbContext")
        {

        }

        public DbQuery<AccountView> AccountView
        {
            get
            {
                return Set<AccountView>().AsNoTracking();
            }
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountView>();
        }
    }
}

using Parkheesung.Domain.Migrations;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Parkheesung.Domain.Database
{
    public class EFDbMigrator : DbMigrator
    {
        public EFDbMigrator(DbMigrationsConfiguration configuration) : base(configuration)
        {
        }

        public static void StartUp()
        {
            System.Data.Entity.Database.SetInitializer<EFDbContext>(new MigrateDatabaseToLatestVersion<EFDbContext, Configuration>());
            using (var context = new EFDbContext())
            {
                var dbMigrator = new EFDbMigrator(new Configuration());
                dbMigrator.BaseUpdate();
            }
        }

        public void BaseUpdate()
        {
            base.Update();
        }
    }
}

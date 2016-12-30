using Parkheesung.Domain.Migrations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Text;

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
            using (var context = new EFDbContext())
            {
                StringBuilder builder = new StringBuilder(128);
                builder.AppendLine("IF Exists(Select * from sys.all_views where name='AccountView')");
                builder.AppendLine("BEGIN");
                builder.AppendLine("DROP VIEW AccountView");
                builder.AppendLine("END");
                context.Database.ExecuteSqlCommand(builder.ToString());

                builder.Clear();
                builder.AppendLine("CREATE VIEW [dbo].[AccountView]");
                builder.AppendLine("AS");
                builder.AppendLine("SELECT  A.AccountID, A.MemberID, A.Title, A.UserID, A.UserPWD, A.AccessURL, A.Memo, A.IsActive, A.RegDate, ");
                builder.AppendLine("A.LastUpdate, B.GroupID, B.GroupName");
                builder.AppendLine("FROM     dbo.Accounts AS A INNER JOIN");
                builder.AppendLine("dbo.AccountGroups AS B ON A.GroupID = B.GroupID");
                context.Database.ExecuteSqlCommand(builder.ToString());
            }
        }
    }
}

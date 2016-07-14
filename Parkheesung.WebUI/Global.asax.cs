using Autofac.Integration.Mvc;
using OctopusLibrary.Filters;
using Parkheesung.Domain.Database;
using Parkheesung.WebUI.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Parkheesung.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Compress 압축 전송은 web.config에 IsReal값이 true일때만 적용합니다.
            bool IsReal = System.Configuration.ConfigurationManager.AppSettings["IsReal"].Equals("true");
            if (IsReal)
            {
                GlobalFilters.Filters.Add(new CompressAttribute());
            }
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            EFDbMigrator.StartUp();

            var container = AutofacConfig.Create(typeof(MvcApplication).Assembly);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

using System.Web.Optimization;

namespace Parkheesung.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Script/Base").Include("~/Scripts/Base.js", "~/Scripts/default.js", "~/Scripts/jquery-Json-1.0.1.js", "~/Scripts/jquery-loading-0.1.0.js", "~/Scripts/jquery-popup-0.1.9.js"));
            bundles.Add(new ScriptBundle("~/Script/bootstrap").Include("~/Scripts/site.min.js"));
            bundles.Add(new StyleBundle("~/Style/Base").Include("~/Content/PopLayer.css", "~/Content/Loading.css", "~/Content/Site.css"));
            bundles.Add(new StyleBundle("~/Style/bootstrap").Include("~/Content/bootstrap.css", "~/Content/css/bootflat.css"));
            //실제 축소(minify) 작업은 web.config의 IsReal값이 true일때만 수행합니다.
            BundleTable.EnableOptimizations = System.Configuration.ConfigurationManager.AppSettings["IsReal"].Equals("true");
        }
    }
}
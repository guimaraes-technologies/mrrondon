using System.Web.Optimization;

namespace MrRondon.Presentation.Mvc
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/basic").Include(
                "~/Content/semantic.css",
                "~/Content/plugins/lobibox/lobibox.css",
                "~/Content/site/basic.css",
                "~/Content/site/responsive.css"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/plugins/lobibox/lobibox.js"));
            
            BundleTable.EnableOptimizations = false;
        }
    }
}
using System.Web.Optimization;

namespace MrRondon.Presentation.Mvc
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //CSS
            bundles.Add(new StyleBundle("~/Content/basic").Include(
                "~/Content/semantic.css",
                "~/Content/plugins/lobibox/lobibox.css",
                "~/Content/site/basic.css",
                "~/Content/site/responsive.css"));

            //SCRIPTS
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));
            
            bundles.Add(new ScriptBundle("~/bundles/basic").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/semantic.js",
                "~/Scripts/plugins/lobibox/lobibox.js",
                "~/Scripts/site/basic.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                "~/Scripts/jquery.mask.js",
                "~/Scripts/site/validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/site/datatable.js"));

            bundles.Add(new ScriptBundle("~/bundles/company").Include(
                "~/Scripts/site/contact.js",
                "~/Scripts/site/company.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
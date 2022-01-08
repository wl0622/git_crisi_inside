using System.Web;
using System.Web.Optimization;

namespace inside.crsri
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}-min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/crsri/css/index.css",
                "~/Content/themes/crsri/css/children.css",
                "~/Content/themes/crsri/css/float-box.css",
                "~/Content/themes/crsri/css/style.css",
                  "~/Content/themes/crsri/css/footerbox.css"
                ));
        }
    }
}
using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
namespace HiLToysWebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));
        //}
        public static void RegisterBundles(BundleCollection bundles)
        {
               bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                           "~/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                           "~/Scripts/jquery.validate*"));
                   bundles.Add(new StyleBundle("~/Content/css").Include(
                             "~/Content/bootstrap.css",
                             "~/Content/site.css"));
                   bundles.Add(new StyleBundle("~/Content/mobilecss").Include("~/Content/jquery.mobile*"));
                   bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jquery.mobile*"));
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));
            bundles.Add(new ScriptBundle("~/bundles/Retinajs").Include(
                            "~/Scripts/Retina/js/function.js",
                             "~/Scripts/jquery-1.8.0.min.js",
                            "~/Scripts/Retina/js/jquery.carouFredSel-5.5.0-packed.js",
                            "~/Scripts/Retina/js/jquery.carouFredSel-6.2.1-packed.js",
                            "~/Scripts/Retina/js/modernizr.custom.js"
                           ));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });
        }
    }
}

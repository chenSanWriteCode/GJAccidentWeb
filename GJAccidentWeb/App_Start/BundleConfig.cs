using System.Web;
using System.Web.Optimization;

namespace GJAccidentWeb
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            //bundles.Add(new StyleBundle("~/bundles/zuiCSS").Include("~/Scripts/dist/css/zui.css", "~/Scripts/dist/lib/datagrid/zui.datagrid.css", "~/Scripts/dist/lib/chosen/zui.chosen.css", "~/Scripts/dist/lib/datetimepicker/datetimepicker.min.css", "~/Scripts/dist/lib/bootbox/bootbox.min.css", "~/Content/site.css"));
            //bundles.Add(new ScriptBundle("~/bundles/zuiJS").Include("~/Scripts/dist/js/zui.js", "~/Scripts/dist/lib/datagrid/zui.datagrid.js", "~/Scripts/dist/lib/chosen/zui.chosen.js", "~/Scripts/dist/lib/datetimepicker/datetimepicker.min.js", "~/Scripts/dist/lib/bootbox/bootbox.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/bundles/zuiCSS").Include("~/Scripts/dist/css/zui.css", "~/Scripts/dist/lib/datagrid/zui.datagrid.css", "~/Scripts/dist/lib/datatable/zui.datatable.css", "~/Scripts/dist/lib/chosen/chosen.min.css", "~/Scripts/dist/lib/datetimepicker/datetimepicker.min.css", "~/Scripts/dist/lib/bootbox/bootbox.min.css", "~/Scripts/dist/lib/dashboard/zui.dashboard.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/zuiJS").Include("~/Scripts/dist/lib/jquery/jquery.js", "~/Scripts/dist/lib/jquery/jquery.cookie.js", "~/Scripts/dist/js/zui.js", "~/Scripts/dist/lib/datagrid/zui.datagrid.js", "~/Scripts/dist/lib/datatable/zui.datatable.js", "~/Scripts/dist/lib/chosen/chosen.min.js", "~/Scripts/dist/lib/datetimepicker/datetimepicker.min.js", "~/Scripts/dist/lib/selectable/zui.selectable.js", "~/Scripts/dist/lib/bootbox/bootbox.min.js", "~/Scripts/dist/lib/dashboard/zui.dashboard.min.js", "~/Scripts/dist/lib/chart/zui.chart.min.js"));
        }
    }
}

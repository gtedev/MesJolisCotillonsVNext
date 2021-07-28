using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MesJolisCotillons.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Admin

            bundles.Add(new ScriptBundle("~/bundles/admin").Include("~/Scripts/Override.js",
                                                                    "~/Scripts/Direct.js",
                                                                    "~/Scripts/Admin/Admin_app.js",
                                                                    "~/Scripts/Admin/Admin_Widget.js",
                                                                    "~/Scripts/Admin/Admin_ux.js",
                                                                    "~/Scripts/Admin/Admin_util.js",
                                                                    "~/Scripts/Admin/Admin_plugin.js",
                                                                    "~/Scripts/Admin/User/Admin_store_User.js",
                                                                    "~/Scripts/Admin/User/Admin_ux_User.js",
                                                                    "~/Scripts/Admin/Design/Admin_Design_store.js",
                                                                    "~/Scripts/Admin/Design/Admin_Design_ux.js",
                                                                    "~/Scripts/Admin/Command/Admin_ux_Command.js",
                                                                    "~/Scripts/Admin/Command/Admin_store_Command.js",
                                                                    "~/Scripts/Admin/Document/Admin_store_Document.js",
                                                                     //"~/Scripts/Admin/Document/Admin_ux_Document.js",
                                                                    "~/Scripts/Admin/Product/Admin_store_ProductManagement.js",
                                                                    "~/Scripts/Admin/Product/Admin_ux_ProductManagement.js"));


            #endregion

            #region AdminCss
            bundles.Add(new StyleBundle("~/Content/AdminCss")
                   .Include("~/Content/_Admin.css"));
            #endregion

            #region Mes JolisCotillonsCss
            bundles.Add(new StyleBundle("~/Content/MesJolisCotillonsCssPackage")
                      .Include("~/Content/_MesJolisCotillons.css",
                               "~/Content/Products.css",
                               "~/Content/Cart.css",
                                "~/Content/Address.css",
                               "~/Content/Command.css",
                               "~/Content/Nouveautes.css",
                               "~/Content/Header.css",
                               "~/Content/Login.css",
                               "~/Content/Customer.css",
                               "~/Content/Information.css",
                               "~/Content/Footer.css",
                               "~/Content/Inscription.css",
                               "~/Content/noty-2.3.8/demo/buttons.css",
                               "~/Content/noty-2.3.8/js/animate.css"));
            #endregion

            #region Mes JolisCotillonsCss Mobile
            bundles.Add(new StyleBundle("~/Content/MesJolisCotillonsCssPackageMobile")
                      .Include("~/Content/CannotHandleMobile.css"));
            #endregion

            #region ExtJs 

            bundles.Add(new ScriptBundle("~/bundles/extensions").Include("~/Content/Ext.ux.icon/Ext.ux.icon.js",
                                                                         "~/Content/noty-2.3.8/js/noty/packaged/jquery.noty.packaged.js",
                                                                         "~/Content/Ext.ux.window.Notification/Ext.ux.window.Notification.js"));

            bundles.Add(new StyleBundle("~/Content/noty-2.3.8/Animate").Include(
                                        "~/Content/noty-2.3.8/js/animate.css"));

            bundles.Add(new StyleBundle("~/Content/Ext.ux.window.Notification/style").Include(
                                        "~/Content/Ext.ux.window.Notification/Ext.ux.window.Notification.css"));

            bundles.Add(new ScriptBundle("~/bundles/extjs-all").Include("~/Content/extjs6/ext-all-debug-6.2.0.js",
                                                                        "~/Content/extjs6/ext-locale-fr.js"));

            #endregion

            #region Extjs Theme 

            #region Triton
            bundles.Add(new StyleBundle("~/Content/extjs6/style")
                  .Include("~/Content/extjs6/themes/theme-triton/theme-triton-all-debug_1.css",
                           "~/Content/extjs6/themes/theme-triton/theme-triton-all-debug_2.css"));
            #endregion

            #region Crisp
            //bundles.Add(new StyleBundle("~/Content/extjs/themes/ext-theme/style")
            //      .Include("~/Content/extjs6/themes/theme-crisp/theme-crisp-all-debug_1.css",
            //              "~/Content/extjs6/themes/theme-crisp/theme-crisp-all-debug_2.css"));
            #endregion

            #region Gray
            //bundles.Add(new StyleBundle("~/Content/extjs/themes/ext-theme/style")
            //      .Include("~/Content/extjs6/themes/theme-gray/theme-gray-all-debug_1.css",
            //              "~/Content/extjs6/themes/theme-gray/theme-gray-all-debug_2.css"));
            #endregion

            #region Aria
            //bundles.Add(new StyleBundle("~/Content/extjs/themes/ext-theme/style")
            //      .Include("~/Content/extjs6/themes/theme-aria/theme-aria-all-debug_1.css",
            //              "~/Content/extjs6/themes/theme-aria/theme-aria-all-debug_2.css"));
            #endregion

            #region Neptune
            //bundles.Add(new StyleBundle("~/Content/extjs/themes/ext-theme/style")
            //      .Include("~/Content/extjs6/themes/theme-neptune/theme-neptune-all-debug_1.css",
            //              "~/Content/extjs6/themes/theme-neptune/theme-neptune-all-debug_2.css"));
            #endregion

            #endregion

            #region MjcTools
            bundles.Add(new ScriptBundle("~/bundles/MjcTools").Include("~/Scripts/MjcTools.js"));
            #endregion


#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

        }
    }
}
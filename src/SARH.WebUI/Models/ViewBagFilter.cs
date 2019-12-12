using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SARH.WebUI.Models
{
    public class ViewBagFilter : IActionFilter
    {
        private static readonly string Enabled = "Enabled";
        private static readonly string Disabled = string.Empty;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                // SmartAdmin Toggle Features
                controller.ViewBag.AppSidebar = Enabled;
                controller.ViewBag.AppHeader = Enabled;
                controller.ViewBag.AppLayoutShortcut = Enabled;
                controller.ViewBag.AppFooter = Enabled;
                controller.ViewBag.ShortcutMenu = Enabled;
                controller.ViewBag.ChatInterface = Disabled;
                controller.ViewBag.LayoutSettings = Disabled;

                // SmartAdmin Default Settings
                controller.ViewBag.App =  "ISOSA";
                controller.ViewBag.AppName =  "ISOSA SARH";
                controller.ViewBag.AppFlavor =  "ASP.NET Core 2.2";
                controller.ViewBag.AppFlavorSubscript =  "SEED";
                controller.ViewBag.User =  "Administrador ISOSA";
                controller.ViewBag.Email =  "admin@isosa.com";
                controller.ViewBag.Twitter =  "ISOSA";
                controller.ViewBag.Avatar = "icons8-user-96.png";
                controller.ViewBag.Version =  "1.0.0";
                controller.ViewBag.Bs4v =  "1.0";
                controller.ViewBag.Logo =  "logo.png";
                controller.ViewBag.LogoM =  "logo.png";
                controller.ViewBag.Copyright =  "2019 © SmartAdmin by&nbsp;<a href='https = //www.gotbootstrap.com' class='text-primary fw-500' title='gotbootstrap.com' target='_blank'>gotbootstrap.com</a>";
                controller.ViewBag.CopyrightInverse =  "2019 © SmartAdmin by&nbsp;<a href='https = //www.gotbootstrap.com' class='text-white opacity-40 fw-500' title='gotbootstrap.com' target='_blank'>gotbootstrap.com</a>";
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

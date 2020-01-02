using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SmartAdmin.WebUI.Areas.Identity.Pages.Account;

namespace SARH.WebUI.Models
{
    public class ViewBagFilter : IActionFilter
    {
        private static readonly string Enabled = "Enabled";
        private static readonly string Disabled = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ViewBagFilter(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

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
                controller.ViewBag.Twitter =  "ISOSA";
                controller.ViewBag.Avatar = "icons8-user-96.png";
                controller.ViewBag.Version =  "1.0.0";
                controller.ViewBag.Bs4v =  "1.0";
                controller.ViewBag.Logo =  "logo.png";
                controller.ViewBag.LogoM =  "logo.png";

                var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");
                var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
                string userName = loginInfo != null ? loginInfo.Email : null;

                if (!string.IsNullOrEmpty(userName)) 
                {
                    controller.ViewBag.User = userName;
                    controller.ViewBag.Email = userName;
                }

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

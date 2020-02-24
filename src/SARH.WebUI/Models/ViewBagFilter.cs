using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ViewBagFilter(IHttpContextAccessor httpContextAccessor,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._signInManager = signInManager;
            this._userManager = userManager;
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


                var claims = this._signInManager.Context.User;
                if (this._signInManager.IsSignedIn(claims)) 
                {
                    var user = this._userManager.GetUserAsync(claims).Result;
                    var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");
                    var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
                    string userName = loginInfo != null ? loginInfo.Email : user.Email;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        controller.ViewBag.User = userName;
                        controller.ViewBag.Email = userName;
                        _httpContextAccessor.HttpContext.Session.SetString("loginmodel", JsonConvert.SerializeObject(new LoginModel.InputModel() 
                        {
                            Email = userName,
                            Password = "",
                            RememberMe = false
                        }));
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

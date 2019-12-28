using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SARH.WebUI.Models.Hub;
using SmartAdmin.WebUI.Areas.Identity.Pages.Account;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SARH.WebUI.Hubs
{
    public class NotificationHub : Hub
    {

        private readonly IRepository<EmployeeFormat> _formatRepository;
        private static readonly ConcurrentDictionary<string, UserHubModels> Users =
            new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;

        public NotificationHub(IRepository<EmployeeFormat> formatRepository,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<IdentityUser> signInManager)
        {
            this._formatRepository = formatRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._signInManager = signInManager;
        }


        public async Task SendMessage(string user, string message, int approbedItem) 
        {

            if (Users.Any()) 
            {
                await Clients.Client(Users.FirstOrDefault().Value.ConnectionIds.FirstOrDefault()).SendAsync("ReceiveMessage", user, message);
            }

            //await Clients.All.SendAsync("ReceiveMessage", user, message);

        }

        public string GetConnectionId() 
        {
            return Context.ConnectionId;
        }

        public override Task OnConnectedAsync()
        {


            var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");

            var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;

            string userName = loginInfo != null ? loginInfo.Email : null;

           
            string connectionId = Context.ConnectionId;

            if (userName != null) 
            {
                var user = Users.GetOrAdd(userName, _ => new UserHubModels
                {
                    UserName = userName,
                    ConnectionIds = new HashSet<string>()
                });

                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.Add(connectionId);
                    if (user.ConnectionIds.Count == 1)
                    {
                    }
                }
            }


            return base.OnConnectedAsync();
        }


    }
}

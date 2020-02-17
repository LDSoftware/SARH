using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SARH.WebUI.Factories;
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
        private readonly IRepository<FormatApprover> _formatApproverRepository;
        private readonly IOrganigramaModelFactory _organigramaModelFactory;
        private readonly INotificationModelFactory _notificationModelFactory;
        private static readonly ConcurrentDictionary<string, UserHubModels> Users =
            new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;

        public NotificationHub(IRepository<EmployeeFormat> formatRepository,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<IdentityUser> signInManager,
            IRepository<FormatApprover> formatApproverRepository,
            IOrganigramaModelFactory organigramaModelFactory,
            INotificationModelFactory notificationModelFactory)
        {
            this._formatRepository = formatRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._signInManager = signInManager;
            this._formatApproverRepository = formatApproverRepository;
            this._organigramaModelFactory = organigramaModelFactory;
            this._notificationModelFactory = notificationModelFactory;
        }


        public async Task SendMessage(string user, string message, int approbedItem) 
        {

            string sendHub = string.Empty;
            var employees = this._organigramaModelFactory.GetAllData();
            List<string> _hubsId = new List<string>();
            string row = string.Empty;

            if (!approbedItem.Equals(0)) 
            {
                var emp = this._formatRepository.GetElement(approbedItem);
                if (emp != null) 
                {
                    var empdet = employees.Employess.Where(u => u.Id.Equals(emp.EmployeeId));
                    if (empdet.Any()) 
                    {
                        var emdet = empdet.FirstOrDefault();

                        var approvers = _formatApproverRepository.SearhItemsFor(y => y.Area.Equals(emdet.Area)
                            && y.Centro.Equals(emdet.JobCenter)
                            && y.Departamento.Equals(emdet.Category));

                        if (approvers.Any()) 
                        {
                            approvers.ToList().ForEach((e) =>
                            {
                                var appr = employees.Employess.Where(b => b.RowId.ToLower().Equals(e.RowGuid.ToString().ToLower()));
                                if (appr.Any()) 
                                {
                                    var att = appr.FirstOrDefault();
                                    var urt = Users.Where(i => i.Key.Equals(att.UserName));
                                    if (urt.Any()) 
                                    {
                                        var r = urt.FirstOrDefault().Value.ConnectionIds.Last();
                                        _hubsId.Add(r);
                                    }
                                }
                            });
                        }
                    }
                }
            }



             _hubsId.ForEach(async hubclient => 
            {
                var notify = _notificationModelFactory.Notification;
                row = $" {notify}";

                await Clients.Client(hubclient).SendAsync("ReceiveMessage", user, message, row);
            });

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

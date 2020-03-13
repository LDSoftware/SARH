using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Notification;
using SARH.WebUI.Models.Organigrama;
using SmartAdmin.WebUI.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Components
{
    public class Notification : ViewComponent
    {
        private readonly INotificationModelFactory _notificationModelFactory;


        public Notification(INotificationModelFactory notificationModelFactory)
        {
            this._notificationModelFactory = notificationModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            NotificationModel model = new NotificationModel() 
            {
                Notifications = _notificationModelFactory.Notification,
                LastVacationsNotificationsItems = _notificationModelFactory.LastVacationsNotificationItems,
                LastPermissionsNotificationsItems = _notificationModelFactory.LastPermissionsNotificationItems,
                LastOthersNotificationsItems = _notificationModelFactory.LastOthersNotificationItems,
                NotificactionsItems = _notificationModelFactory.NotificaticonsItems
            };

            return View(model);
        }
    }
}

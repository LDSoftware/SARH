using SARH.WebUI.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface INotificationModelFactory
    {
        int Notification { get; set; }
        List<NotificacionModelItem> AllNotificaticonsItems { get; set; }
        List<NotificacionModelItem> NotificaticonsItems { get; set; }
        List<NotificacionModelItem> LastVacationsNotificationItems { get; set; }
        List<NotificacionModelItem> LastPermissionsNotificationItems { get; set; }
        List<NotificacionModelItem> LastOthersNotificationItems { get; set; }
    }
}

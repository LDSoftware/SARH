using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Notification
{
    public class NotificationModel
    {

        public NotificationModel()
        {
            NotificactionsItems = new List<NotificacionModelItem>();
            LastVacationsNotificationsItems = new List<NotificacionModelItem>();
            LastPermissionsNotificationsItems = new List<NotificacionModelItem>();
            LastOthersNotificationsItems = new List<NotificacionModelItem>();
        }

        public int Notifications { get; set; }
        public List<NotificacionModelItem> NotificactionsItems { get; set; }
        public List<NotificacionModelItem> LastVacationsNotificationsItems { get; set; }
        public List<NotificacionModelItem> LastPermissionsNotificationsItems { get; set; }
        public List<NotificacionModelItem> LastOthersNotificationsItems { get; set; }
    }

    public class NotificacionModelItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string Deparment { get; set; }
        public string JobTitle { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public string CreateDate { get; set; }
        public string Type { get; set; }
        public string Period { get; set; }
        public string ApproverWorkFlow { get; set; }
        public string EmployeeSustitution { get; set; }
        public string Estatus { get; set; }
    }

}

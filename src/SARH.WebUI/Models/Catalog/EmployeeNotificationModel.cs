using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Catalog
{
    public class EmployeeNotificationModel
    {

        public EmployeeNotificationModel()
        {
            Notifications = new List<EmployeeNotificationItem>();
        }

        public IList<EmployeeNotificationItem> Notifications { get; set; }
    }

    public class EmployeeNotificationItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Centro { get; set; }
        public string Notifier { get; set; }
        public string Puesto { get; set; }
        public string Email { get; set; }
        public string RowGuid { get; set; }
    }

}

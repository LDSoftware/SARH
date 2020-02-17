using SARH.WebUI.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Formats
{
    public class FormatApproverPendingModel
    {

        public FormatApproverPendingModel()
        {
            FormatEmployee = new FormatApproverEmployee();
            FormatDetail = new NotificacionModelItem();
            Approvers = new List<ForrmatApproverWorkflow>();
        }

        public FormatApproverEmployee FormatEmployee { get; set; }
        public NotificacionModelItem FormatDetail { get; set; }
        public List<ForrmatApproverWorkflow> Approvers { get; set; }

    }

    public class FormatApproverEmployee
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string JobTitle { get; set; }
        public string Departamento { get; set; }
        public string PicturePath { get; set; }
        public string PendingVacations { get; set; }
    }

    public class FormatAppoveDetail
    {
        public int Id { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Type { get; set; }
    }


}

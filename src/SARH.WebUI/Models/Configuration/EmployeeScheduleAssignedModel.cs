using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Configuration
{
    public class EmployeeScheduleAssignedModel
    {

        public EmployeeScheduleAssignedModel()
        {
            EmployeeScheduleAssignedList = new List<EmployeeScheduleAssignedItem>();
        }

        public List<EmployeeScheduleAssignedItem> EmployeeScheduleAssignedList { get; set; }
    }

    public class EmployeeScheduleAssignedItem
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ScheduleWorkday { get; set; }
        public string ScheduleMeal { get; set; }
        public string ToleranceWorkday { get; set; }
        public string ToleranceMeal { get; set; }
        public string ScheduleWeekEnd { get; set; }
        public string ToleranceWeekEnd { get; set; }
        public string EsTemporal { get; set; }
    }


}

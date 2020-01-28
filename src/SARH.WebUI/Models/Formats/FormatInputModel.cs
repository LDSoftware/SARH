using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Formats
{
    public class FormatInputModel
    {
        public string EmployeeId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PermissionType { get; set; }
        public string Comments { get; set; }
        public string EmployeeSubId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool WithPay { get; set; }
    }
}

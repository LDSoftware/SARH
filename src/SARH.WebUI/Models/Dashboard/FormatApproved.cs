using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Dashboard
{
    public class FormatApproved
    {
        public FormatApproved()
        {
            FormaApprovedtItems = new List<FormatApprovedItem>();
        }

        public string Date { get; set; }
        public int TotalFormats { get; set; }
        public List<FormatApprovedItem> FormaApprovedtItems { get; set; }
    }

    public class FormatApprovedItem 
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Type { get; set; }
        public string PeriodDates { get; set; }
        public string PeriodHours { get; set; }
        public string workflow { get; set; }
    }

}

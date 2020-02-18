using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Report
{
    public class ReportEmployeeDetailModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string JobTitle { get; set; }
        public string DetailType { get; set; }
        public string Fecha { get; set; }
        public int Type { get; set; }
    }
}

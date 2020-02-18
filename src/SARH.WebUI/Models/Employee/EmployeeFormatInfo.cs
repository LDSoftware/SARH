using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Employee
{
    public class EmployeeFormatInfo
    {
        public string EmployeeId { get; set; }
        public string Area { get; set; }
        public string Centro { get; set; }
        public string Departament { get; set; }
        public string JobTitle { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string FormatName { get; set; }
        public string FormatType { get; set; }
        public string Comments { get; set; }
        public string FechaSolicitud { get; set; }
        public string Suplente { get; set; }
        public string Aprobadores { get; set; }
        public string AdditionalInfo { get; set; }
        public bool Approved { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Employee
{
    public class EmployeeDocumentModel
    {
        public string EmployeeId { get; set; }
        public int DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public bool IsValid { get; set; }
        public bool Checked { get; set; }
    }
}

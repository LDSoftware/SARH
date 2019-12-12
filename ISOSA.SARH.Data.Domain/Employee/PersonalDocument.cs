using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Employee
{
    public class PersonalDocument : EntityBase
    {
        public string DocumentPathInfo { get; set; }
        public string EmployeeID { get; set; }
        public int DocumentType { get; set; }
        public bool IsValid { get; set; }
        public bool Checked { get; set; }
    }
}

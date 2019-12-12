using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Assignation
{
    public class EmployeeObjectAsignation : EntityBase
    {
        public int AssignationType { get; set; }
        public string EmployeeId { get; set; }
        public string AssignationDetail { get; set; }
        public string PathPDF { get; set; }
        public string DocumentId { get; set; }
    }
}

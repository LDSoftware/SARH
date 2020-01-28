using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Formats
{
    public class EmployeeFormat : EntityBase
    {
        public string EmployeeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PermissionType { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovalWorkFlow { get; set; }
        public string Comments { get; set; }
        public string EmployeeSubstitute { get; set; }
        public bool Declined { get; set; }
        public bool SubstituteApprove { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool WithPay { get; set; }
    }
}

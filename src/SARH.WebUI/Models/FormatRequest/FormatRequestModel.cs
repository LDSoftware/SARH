using SARH.WebUI.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.FormatRequest
{
    public class FormatRequestModel
    {

        public FormatRequestModel()
        {
            EmployeeFormats = new List<EmployeeFormatItem>();
            EmployeeVacations = new EmployeeVacation();
        }

        public string Employee { get; set; }
        public string FulName { get; set; }
        public string Picture { get; set; }
        public string JobTitle { get; set; }
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public int TotalPermisos { get; set; }
        public List<EmployeeFormatItem> EmployeeFormats { get; set; }
        public EmployeeVacation EmployeeVacations { get; set; }
        public string DetallesFormato { get; set; }

    }

    public class EmployeeFormatItem
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string CreateDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PermissionType { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalWorkFlow { get; set; }
        public string Comments { get; set; }
        public string EmployeeSubstitute { get; set; }
        public string Approved { get; set; }
        public string ApprovedComments { get; set; }

    }

}

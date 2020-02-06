using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Employee
{
    public class EmployeeHistoryModel
    {
        public EmployeeHistoryModel()
        {
            EmployeeHistory = new List<EmployeeHistoryItem>();
        }

        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }
        public List<EmployeeHistoryItem> EmployeeHistory { get; set; }

    }

    public class EmployeeHistoryItem
    {
        public int Id { get; set; }
        public string RegisterDate { get; set; }
        public string EmployeeId { get; set; }
        public string Descripcion { get; set; }
        public string JobLast { get; set; }
        public string JobActual { get; set; }
        public string DateChange { get; set; }
        public string UserId { get; set; }
    }

}

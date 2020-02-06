using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Employee
{
    public class EmployeeHistory : EntityBase
    {
        public string EmployeeId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Descripcion { get; set; }
        public string JobLast { get; set; }
        public Guid RowGuidLast { get; set; }
        public string JobActual { get; set; }
        public Guid RowGuidActual { get; set; }
        public DateTime DateChange { get; set; }
        public string UserId { get; set; }
    }
}

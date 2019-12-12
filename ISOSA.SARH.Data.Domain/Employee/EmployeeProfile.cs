using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Employee
{
    public class EmployeeProfile : EntityBase
    {
        public string EmployeeId { get; set; }
        public string ProfileSectionValues { get; set; }
    }
}

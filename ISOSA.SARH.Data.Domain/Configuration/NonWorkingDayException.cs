using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Configuration
{
    public class NonWorkingDayException : EntityBase
    {
        public int HolidayId { get; set; }
        public string EmployeeId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Formats
{
    public class EmployeeFormatSubstitute
    {
        public string EmployeeId { get; set; }
        public int ScheduleWorkDay { get; set; }
        public int ScheduleMeal { get; set; }
        public DateTime ValidOf { get; set; }
        public DateTime ValidUntil { get; set; }
        public int IdFormat { get; set; }
    }
}




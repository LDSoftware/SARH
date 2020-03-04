using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Dashboard
{
    public class EmployeeScheduleDate : EntityBase
    {
        public string Employee { get; set; }
        public string StartJobDay { get; set; }
        public string EndJobDay { get; set; }
        public string StartMealDay { get; set; }
        public string EndMealDay { get; set; }
        public DateTime RegisterDate { get; set; }
        public int LastUpdateId { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Configuration
{
    public class EmployeeScheduleAssigned : EntityBase
    {
        public string EmployeeId { get; set; }
        public int IdScheduleWorkday { get; set; }
        public int IdScheduleMeal { get; set; }
        public int ToleranceWorkday { get; set; }
        public int ToleranceMeal { get; set; }

    }
}

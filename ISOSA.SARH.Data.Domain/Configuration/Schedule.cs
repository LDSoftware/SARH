using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Configuration
{
    public class Schedule : EntityBase
    {
        public string Description { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public bool Enabled { get; set; }
        public DateTime? EffectiveTimeStart { get; set; }
        public DateTime? EffectiveTimeEnd { get; set; }
        public int TypeSchedule { get; set; }
        public int StartHourAnticipated { get; set; }
        public bool Weekend { get; set; }
        public string StartHourWeekend { get; set; }
        public string EndHourWeekend { get; set; }
    }
}

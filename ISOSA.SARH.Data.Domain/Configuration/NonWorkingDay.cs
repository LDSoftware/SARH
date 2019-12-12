using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Configuration
{
    public class NonWorkingDay : EntityBase
    {
        public DateTime Holiday { get; set; }
        public string Description { get; set; }
    }
}

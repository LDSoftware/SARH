using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Formats
{
    public class FormatApprovedEmployee : EntityBase
    {
        public Guid RowGuid { get; set; }
        public int ApprovedOrder { get; set; }
    }
}

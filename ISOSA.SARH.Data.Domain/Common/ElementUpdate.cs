using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Common
{
    public class ElementUpdate : EntityBase
    {
        public int ElementType { get; set; }
        public DateTime? UpdateSync { get; set; }
    }
}

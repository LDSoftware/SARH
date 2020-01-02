using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Formats
{
    public class FormatApprover : EntityBase
    {
        public string Area { get; set; }
        public string Centro { get; set; }
        public string Departamento { get; set; }
        public string Puesto { get; set; }
        public Guid RowGuid { get; set; }
        public int Orden { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Employee
{
    public class EmployeeVacation
    {
        public string Employee { get; set; }
        public int Antiguedad { get; set; }
        public int TotalDias { get; set; }
        public int DiasTomados { get; set; }
        public int DiasDisponibles { get; set; }
        public int DiasPorAño { get; set; }
    }
}

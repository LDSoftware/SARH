using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Employee
{
    public class EmployeeOrganigrama : EntityBase
    {
        public string Area { get; set; }
        public string Puesto { get; set; }
        public DateTime? InicioTurno { get; set; }
        public DateTime? InicioAlimento { get; set; }
        public DateTime? FinAlimento { get; set; }
        public DateTime? FinTurno { get; set; }
        public DateTime? SabadoInicio { get; set; }
        public DateTime? SabadoFinal { get; set; }
        public Guid RowGuid { get; set; }
        public string Departamento { get; set; }
        public string TipoRoll { get; set; }
        public Guid? IdentPuesto { get; set; }
        public decimal? PorcentajeReparto { get; set; }
        public decimal? Porcentaje100 { get; set; }
        public decimal? PorcentajeCT { get; set; }
        public string Centro { get; set; }
        public bool? Jefatura { get; set; }
        public string Codigo { get; set; }
    }
}

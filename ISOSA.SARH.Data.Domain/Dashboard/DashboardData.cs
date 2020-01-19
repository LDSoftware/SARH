using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Dashboard
{
    public class DashboardData
    {
        [Key]
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Area { get; set; }
        public string Centro { get; set; }
        public string Departamento { get; set; }
        public string Puesto { get; set; }
        public DateTime RegisterDate { get; set; }
        public string StartWorkDate { get; set; }
        public string StartJobDay { get; set; }
        public int RetardoEntrada { get; set; }
        public string StartMealDate { get; set; }
        public string StartMealDay { get; set; }
        public int SalidaAnticipadaComida { get; set; }
        public string EndMealDate { get; set; }
        public string EndMealDay { get; set; }
        public int RetardoEntradaComida { get; set; }
        public string EndWorkDate { get; set; }
        public string EndJobDay { get; set; }
        public int SalidaAnticipada { get; set; }
    }
}

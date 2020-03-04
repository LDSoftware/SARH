using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Dashboard
{
    public class PersonalDashboardData
    {

        public PersonalDashboardData()
        {
            Days = new List<PersonalDashboardDataItem>();
        }

        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Centro { get; set; }
        public string Puesto { get; set; }
        public int TotalDays { get; set; }
        public string Picture { get; set; }
        public decimal PorcentajeRetardos { get; set; }
        public decimal PorcentajeSalidasAnticipadasComida { get; set; }
        public decimal PorcentajeRetardosRegresoComida { get; set; }
        public decimal PorcentajeSalidasAnticipadas { get; set; }
        public List<PersonalDashboardDataItem> Days { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }

    }

    public class PersonalDashboardDataItem 
    {
        public string RegisterDate { get; set; }
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

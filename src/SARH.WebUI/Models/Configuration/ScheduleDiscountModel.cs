using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Configuration
{
    public class ScheduleDiscountModel 
    {
        public ScheduleDiscountModel()
        {
            SchedulesDiscounts = new List<ScheduleDiscountModelItem>();
        }

        public List<ScheduleDiscountModelItem> SchedulesDiscounts { get; set; }
    }

    public class ScheduleDiscountModelItem
    {
        public int Id { get; set; }
        public string TipoDescuento { get; set; }
        public string Descripcion { get; set; }
        public string Discount { get; set; }
        public string Habilitado { get; set; }
    }

    public class ScheduleDiscountInput
    {
        public int Id { get; set; }
        public string Dias { get; set; }
        public string Porcentaje { get; set; }
        public string Tipo { get; set; }
        public string Combinacion { get; set; }
        public int RangeInitial { get; set; }
        public int RangeEnd { get; set; }
    }

}

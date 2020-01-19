using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Dashboard
{
    public class DashboardModel
    {

        public DashboardModel()
        {
            DashboardFiltersApply = new DashboardFilters();
        }

        public int EntryDelay { get; set; }
        public int EndDayDelay { get; set; }
        public int NoEntryCheck { get; set; }
        public int FoodStartDelay { get; set; }
        public int FoodEndDelay { get; set; }
        public int NoFoodEntryCheck { get; set; }
        public decimal AverageEntryDelay { get; set; }
        public decimal AverageJobPermission { get; set; }
        public IList<DashboardEmployeeDetailModel> EmployeeDetail { get; set; }
        public DashboardFilters DashboardFiltersApply { get; set; }
        public string FilterDate { get; set; }
    }
}

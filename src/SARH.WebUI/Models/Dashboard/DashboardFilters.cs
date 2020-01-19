using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Dashboard
{
    public class DashboardFilters
    {

        public DashboardFilters()
        {
            StartJobDelay = true;
            NoEntryCheckShow = true;
            EndJobAnticipate = true;
            FoodStartDelayShow = true;
            FoodEndDelayShow = true;
            NoFoodEntryCheckShow = true;
        }
        
        public bool StartJobDelay { get; set; }
        public bool NoEntryCheckShow { get; set; }
        public bool EndJobAnticipate { get; set; }
        public bool FoodStartDelayShow { get; set; }
        public bool FoodEndDelayShow { get; set; }
        public bool NoFoodEntryCheckShow { get; set; }
    }
}

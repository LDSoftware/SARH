using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Configuration
{
    public class NonWorkingDayModel
    {
        public NonWorkingDayModel()
        {
            NonWorkingDays = new List<NonWorkingModelItem>();
        }

        public List<NonWorkingModelItem> NonWorkingDays { get; set; }
        public List<int> HolidayYears { get; set; }
    }

    public class NonWorkingModelItem 
    {
        public int Id { get; set; }
        public string Holiday { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
    }

}

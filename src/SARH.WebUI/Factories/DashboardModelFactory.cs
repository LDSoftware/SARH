using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SARH.WebUI.Models.Dashboard;

namespace SARH.WebUI.Factories
{
    public class DashboardModelFactory : IDashboardModelFactory
    {
        public DashboardModel GetDay(DateTime date)
        {

            DashboardModel model = new DashboardModel();
            model.EmployeeDetail = new List<DashboardEmployeeDetailModel>();

            return model;
        }

        public DashboardModel GetToday()
        {
            DashboardModel model = new DashboardModel() 
            {
                AverageEntryDelay =  0,
                AverageJobPermission = 0,
                EntryDelay = 0,
                EndDayDelay = 0,
                FoodStartDelay = 0,
                FoodEndDelay = 0,
                NoFoodEntryCheck = 0,
                NoEntryCheck = 0,
                EmployeeDetail = new List<DashboardEmployeeDetailModel>() 
                {
                }
            
            };

            return model;
        }
    }
}

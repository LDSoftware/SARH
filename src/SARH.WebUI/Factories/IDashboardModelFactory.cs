using SARH.WebUI.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IDashboardModelFactory
    {
        DashboardModel GetToday();
        DashboardModel GetDay(DateTime date);
    }
}

using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Components
{
    public class DashboardTodayViewComponent : ViewComponent
    {

        public DashboardTodayViewComponent()
        {

        }

        public IViewComponentResult Invoke(IList<DashboardEmployeeDetailModel> model)
        {
            return View(model);
        }

    }
}

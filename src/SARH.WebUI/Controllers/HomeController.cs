using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Dashboard;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SARH.WebUI.Controllers
{
    public class HomeController : Controller
    {

        #region Variables

        private readonly IDashboardModelFactory _dashboardModelFactory;
        private readonly IEmployeeFormatModelFactory _employeeFormatModelFactory;

        #endregion

        #region Constructor

        public HomeController(IDashboardModelFactory dashboardModelFactory,
            IEmployeeFormatModelFactory employeeFormatModelFactory)
        {
            this._dashboardModelFactory = dashboardModelFactory;
            this._employeeFormatModelFactory = employeeFormatModelFactory;
        }

        #endregion

        public IActionResult Index(string filters = "")
        {

            var formats = this._employeeFormatModelFactory.GetAllApprovedFormats(DateTime.Now);

            DashboardFilters filter = new DashboardFilters();
            if (!string.IsNullOrEmpty(filters)) 
            {
                var lst = filters.Split('-');
                filter.StartJobDelay = lst[0] == "0" ? false : true;
                filter.EndJobAnticipate = lst[1] == "0" ? false : true;
                filter.NoEntryCheckShow = lst[2] == "0" ? false : true;
                filter.FoodStartDelayShow = lst[3] == "0" ? false : true;
                filter.FoodEndDelayShow = lst[4] == "0" ? false : true;
                filter.NoFoodEntryCheckShow = lst[5] == "0" ? false : true;
            }
            var model = this._dashboardModelFactory.GetToday(filter);
            model.DashboardFiltersApply = filter;
            model.TotalFormatVacations = formats.Where(t => t.FormatName.ToLower().Contains("vacacion")).Count();
            model.TotalFormatPermissions = formats.Where(t => t.FormatName.ToLower().Contains("permiso")).Count();
            return View(model);
        }
    }
}

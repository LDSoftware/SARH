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
    public class DashboardHistoryController : Controller
    {

        #region Variables

        private readonly IDashboardModelFactory _dashboardModelFactory;

        #endregion

        #region Constructor

        public DashboardHistoryController(IDashboardModelFactory dashboardModelFactory)
        {
            this._dashboardModelFactory = dashboardModelFactory;
        }

        #endregion


        // GET: /<controller>/
        public IActionResult Index(string date, string filters = "")
        {
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
            var model = this._dashboardModelFactory.GetDay(date, filter);
            model.FilterDate = !string.IsNullOrEmpty(date) ? date : DateTime.Now.ToShortDateString();
            model.DashboardFiltersApply = filter;

            return View(model);
        }
    }
}

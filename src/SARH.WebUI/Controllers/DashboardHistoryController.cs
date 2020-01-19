using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;

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
        public IActionResult Index(string date)
        {
            return View(this._dashboardModelFactory.GetDay(date));
        }
    }
}

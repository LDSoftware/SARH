using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SARH.WebUI.Controllers
{
    public class HomeController : Controller
    {

        #region Variables

        private readonly IDashboardModelFactory _dashboardModelFactory;

        #endregion

        #region Constructor

        public HomeController(IDashboardModelFactory dashboardModelFactory)
        {
            this._dashboardModelFactory = dashboardModelFactory;
        }

        #endregion

        public IActionResult Index()
        {
            return View(this._dashboardModelFactory.GetToday());
        }
    }
}

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
        private readonly IOrganigramaModelFactory _organigramaModelFactory;
        private readonly IEmployeeFormatModelFactory _employeeFormatModelFactory;

        #endregion

        #region Constructor

        public DashboardHistoryController(IDashboardModelFactory dashboardModelFactory, 
            IOrganigramaModelFactory organigramaModelFactory,
            IEmployeeFormatModelFactory employeeFormatModelFactory)
        {
            this._dashboardModelFactory = dashboardModelFactory;
            this._organigramaModelFactory = organigramaModelFactory;
            this._employeeFormatModelFactory = employeeFormatModelFactory;
        }

        #endregion


        // GET: /<controller>/
        public IActionResult Index(string date, string filters = "")
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
            var model = this._dashboardModelFactory.GetDay(date, filter);
            model.FilterDate = !string.IsNullOrEmpty(date) ? date : DateTime.Now.ToShortDateString();
            model.DashboardFiltersApply = filter;

            model.TotalFormatVacations = formats.Where(t => t.FormatName.ToLower().Contains("vacacion")).Count();
            model.TotalFormatPermissions = formats.Where(t => t.FormatName.ToLower().Contains("permiso")).Count();


            return View(model);
        }

        public JsonResult GetRegisterData(string EmployeeId, string date)
        {
            var model = this._dashboardModelFactory.GetDashboardDetail(EmployeeId, date, new DashboardFilters()
            {
                EndJobAnticipate = true,
                FoodEndDelayShow = true,
                FoodStartDelayShow = true,
                NoEntryCheckShow = true,
                NoFoodEntryCheckShow = true,
                StartJobDelay = true
            });

            var employee = this._organigramaModelFactory.GetEmployeeData(EmployeeId);

            var response = new
            {
                employeename = $"{employee.GeneralInfo.Id} - {employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName} {employee.GeneralInfo.LastName2}",
                rows = model
            };

            return Json(response);
        }



        public IActionResult EmployeePersonalDashboard(string employee, string date) 
        {
            if (string.IsNullOrEmpty(date)) 
            {
                date = DateTime.Now.ToShortDateString();
            }

            DateTime currentDate = DateTime.Parse(date);
            DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var emp = this._organigramaModelFactory.GetEmployeeData(employee);
            var model = _dashboardModelFactory.GetPersonalDashboardData(int.Parse(employee).ToString("00000"), startDate, endDate);
            model.Picture = emp.GeneralInfo.Picture;


            return View(model);
        }

    }
}

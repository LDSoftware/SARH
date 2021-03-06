﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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



        public IActionResult EmployeePersonalDashboard(string employee, string date, string fdate = "") 
        {

            DateTime currentDate;
            DateTime startDate;
            DateTime endDate;


            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToShortDateString();
                currentDate = DateTime.Parse(date);
                startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                endDate = startDate.AddMonths(1).AddDays(-1);

            }
            else 
            {
                startDate = DateTime.Parse(date);
                endDate = DateTime.Parse(fdate);
            }


            var model = _dashboardModelFactory.GetPersonalDashboardData(int.Parse(employee).ToString("00000"), startDate, endDate);

            model.FechaInicial = startDate.ToShortDateString();
            model.FechaFinal = endDate.ToShortDateString();

            return View(model);
        }

        public FileResult ExportPersonalDashboard(string employee, string date, string fdate = "")
        {

            DateTime currentDate;
            DateTime startDate;
            DateTime endDate;


            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToShortDateString();
                currentDate = DateTime.Parse(date);
                startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                endDate = startDate.AddMonths(1).AddDays(-1);

            }
            else
            {
                startDate = DateTime.Parse(date);
                endDate = DateTime.Parse(fdate);
            }


            var model = _dashboardModelFactory.GetPersonalDashboardData(int.Parse(employee).ToString("00000"), startDate, endDate);

            model.FechaInicial = startDate.ToShortDateString();
            model.FechaFinal = endDate.ToShortDateString();

            var sb = new StringBuilder();

            sb.Append("Id Empleado,Nombre,Fecha,Entrada,Registro,Salida Comida,Registro,Entrada Comida,Registro,Salida,Registro");

            model.Days.ForEach(d =>
            {
                sb.AppendLine();
                sb.Append($"{model.EmployeeId},{model.Name},{d.RegisterDate},{d.StartWorkDate},{d.StartJobDay},{d.StartMealDate},{d.StartMealDay},{d.EndMealDate},{d.EndMealDay},{d.EndWorkDate},{d.EndJobDay}");
            });

            return File(new System.Text.UTF8Encoding().GetBytes(sb.ToString()), "text/csv", $"DashboardPersonal-{model.EmployeeId}-{date.Replace("/", string.Empty)}-{fdate.Replace("/", string.Empty)}.csv");
        }


    }
}

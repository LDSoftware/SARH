using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Employee;
using SARH.WebUI.Models.EmployeeProfile;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SARH.WebUI.Controllers
{
    public class HistoryEmployeeController : Controller
    {

        // GET: /<controller>/
        public IActionResult Index(string employee, 
        [FromServices] IRepository<EmployeeHistory> empHistory,
        [FromServices] IOrganigramaModelFactory organigramaFactory)
        {
            var orgData = organigramaFactory.GetEmployeeData(employee);
            var history = empHistory.SearhItemsFor(e => e.EmployeeId.Equals(orgData.GeneralInfo.Id));

            EmployeeHistoryModel model = new EmployeeHistoryModel()
            {
                EmployeeId = employee,
                Area = orgData.Area,
                JobCenter = orgData.JobCenter,
                FirstName = orgData.GeneralInfo.FirstName,
                LastName = orgData.GeneralInfo.LastName,
                EmployeeHistory = history.Select(y => new EmployeeHistoryItem() 
                { 
                    DateChange = y.DateChange.ToShortDateString(),
                    Descripcion = y.Descripcion,
                    EmployeeId = y.EmployeeId,
                    JobActual = y.JobActual,
                    JobLast = y.JobLast,
                    RegisterDate = y.RegisterDate.ToShortDateString(),
                    UserId = y.UserId,
                    Id = y.Id
                }).ToList()

            };


           return View(model);
        }
    }
}

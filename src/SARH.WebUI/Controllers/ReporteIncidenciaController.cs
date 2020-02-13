using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Dashboard;
using SARH.WebUI.Models.Report;

namespace SARH.WebUI.Controllers
{
    public class ReporteIncidenciaController : Controller
    {

        [ActionName("Visualizar")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GenerateIncidenciaReportData(string dateIni, string dateFin,
        [FromServices] IDashboardModelFactory dashboardModelFactory,
        [FromServices] IRepository<EmployeeFormat> employeeFormatRepository,
        [FromServices] IRepository<DocumentType> documentTypeRepository) 
        {

            List<ReportEmployeeDetailModel> model = new List<ReportEmployeeDetailModel>();
            var dias = Math.Abs(Math.Round((DateTime.Parse(dateIni) - DateTime.Parse(dateFin)).TotalDays, 0));
            if (dias == 0) 
            {
                dias = 1;
            }

            for (int i = 0; i < dias; i++)
            {
                var t = DateTime.Parse(dateIni).AddDays(i);
                var results = dashboardModelFactory.GetNoRegistry(t.ToShortDateString());
                model.AddRange(results);
            }

            var data = new { faltas = model.ToArray(), faltastotal = model.Count };


            return Json(data);
        }



    }
}
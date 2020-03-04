using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SARH.Core.PdfCreator;
using SARH.Core.PdfCreator.Interface;
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
        [FromServices] IRepository<DocumentType> documentTypeRepository,
        [FromServices] IEmployeeFormatModelFactory employeeFormatModelFactory) 
        {

            List<ReportEmployeeDetailModel> modelFaltas = new List<ReportEmployeeDetailModel>();
            List<ReportEmployeeDetailModel> modelIncapacidades = new List<ReportEmployeeDetailModel>();
            List<ReportEmployeeDetailModel> modelVacaciones = new List<ReportEmployeeDetailModel>();
            List<ReportEmployeeDetailModel> modelPermisos = new List<ReportEmployeeDetailModel>();


            var dias = Math.Abs(Math.Round((DateTime.Parse(dateIni) - DateTime.Parse(dateFin)).TotalDays, 0));
            if (dias == 0) 
            {
                dias = 1;
            }

            for (int i = 0; i < dias; i++)
            {
                var t = DateTime.Parse(dateIni).AddDays(i);
                var results = dashboardModelFactory.GetNoRegistry(t.ToShortDateString());
                modelFaltas.AddRange(results);

                var formats = employeeFormatModelFactory.GetAllApprovedFormats(t);

                var f = formats.Where(d => d.FormatName.ToLower().Contains("incapacidad"));
                modelIncapacidades.AddRange(f.Select(h => new ReportEmployeeDetailModel() 
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId
                }));

                var g = formats.Where(d => d.FormatName.ToLower().Contains("vacacion"));
                modelVacaciones.AddRange(g.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId
                }));

                var p = formats.Where(d => d.FormatName.ToLower().Contains("permiso"));
                modelPermisos.AddRange(p.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId
                }));
            }


            var data = new {
                incapacidades = modelIncapacidades.ToArray(),
                incapacidadesTotal = modelIncapacidades.Count,
                faltas = modelFaltas.ToArray(),
                faltastotal = modelFaltas.Count,
                vacaciones = modelVacaciones.ToArray(),
                totalvacaciones = modelVacaciones.Count,
                permisos = modelPermisos.ToArray(),
                totalpermisos = modelPermisos.Count
            };


            return Json(data);
        }


        public FileResult ExportIncidenciaReportData(string dateIni, string dateFin,
        [FromServices] IDashboardModelFactory dashboardModelFactory,
        [FromServices] IEmployeeFormatModelFactory employeeFormatModelFactory)
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

                var formats = employeeFormatModelFactory.GetAllApprovedFormats(t);

                var f = formats.Where(d => d.FormatName.ToLower().Contains("incapacidad"));
                model.AddRange(f.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId,
                    Type = 2
                }));

                var g = formats.Where(d => d.FormatName.ToLower().Contains("vacacion"));
                model.AddRange(g.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId,
                    Type = 3
                }));

                var p = formats.Where(d => d.FormatName.ToLower().Contains("permiso"));
                model.AddRange(p.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId,
                    Type = 4
                }));
            }


            var sb = new StringBuilder();

            sb.Append("Id Empleado,Nombre,Area,Centro,Puesto,Periodo,Observaciones,Tipo");

            model.ForEach(d =>
            {
                sb.AppendLine();
                sb.Append($"{d.ID},{d.Name},{d.Area},{d.JobCenter},{d.JobTitle},{d.Fecha},{d.DetailType},{GetTypeModel(d.Type)}");
            });

            return File(new System.Text.UTF8Encoding().GetBytes(sb.ToString()), "text/csv", $"IncidenciasPeriodo-{dateIni.Replace("/",string.Empty)}-{dateFin.Replace("/",string.Empty)}.csv");
        }

        public FileResult PrintIncidenciaReportData(string dateIni, string dateFin,
        [FromServices] IDashboardModelFactory dashboardModelFactory,
        [FromServices] IEmployeeFormatModelFactory employeeFormatModelFactory,
        [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
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

                var formats = employeeFormatModelFactory.GetAllApprovedFormats(t);

                var f = formats.Where(d => d.FormatName.ToLower().Contains("incapacidad"));
                model.AddRange(f.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId,
                    Type = 2
                }));

                var g = formats.Where(d => d.FormatName.ToLower().Contains("vacacion"));
                model.AddRange(g.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId,
                    Type = 3
                }));

                var p = formats.Where(d => d.FormatName.ToLower().Contains("permiso"));
                model.AddRange(p.Select(h => new ReportEmployeeDetailModel()
                {
                    Area = h.Area,
                    JobCenter = h.Centro,
                    JobTitle = h.JobTitle,
                    Fecha = $"{h.StartDate} - {h.EndDate}",
                    Name = h.Name,
                    DetailType = h.Comments,
                    ID = h.EmployeeId,
                    Type = 4
                }));
            }

            IConfigPdf config = new ConfigPdf()
            {
                FontPathPdf = configManager.FontPathPdf,
                ImgPathPdf = configManager.ImgPathPdf,
                FontPathBarCode = configManager.FontPathBarCode
            };
            PdfManager manager = new PdfManager(config);


            string fileName = $"{Path.GetTempPath()}IncidenciasPeriodo-{dateIni.Replace("/", string.Empty)}-{dateFin.Replace("/", string.Empty)}.pdf";

            if (System.IO.File.Exists(fileName)) 
            {
                System.IO.File.Delete(fileName);
            }

            try
            {
                manager.CreateIncidenciasReport(new Core.PdfCreator.FormatData.DocumentInfoPdfData()
                {
                    TitleDocumento = "Reporte Diario de Incidencias",
                    FormatId = $"{dateIni}-{dateFin}",
                    IncidenciasReport = model.Select(n => new Core.PdfCreator.FormatData.ReportEmployeeDetail() 
                    {
                        Area = n.Area,
                        DetailType = n.DetailType,
                        Fecha = n.Fecha,
                        JobCenter = n.JobCenter,
                        JobTitle = n.JobTitle,
                        ID = n.ID,
                        Name = n.Name,
                        Type = n.Type
                    }).ToList()
                }, fileName);
            }
            catch (Exception ex)
            {
                byte[] FileBytesError = System.Text.Encoding.UTF8.GetBytes(ex.Message);
                return File(FileBytesError, "text/plain");
            }


            byte[] FileBytes = System.IO.File.ReadAllBytes(fileName);
            return File(FileBytes, "application/pdf");
        }

        private string GetTypeModel(int type) 
        {
            string rtype = string.Empty;

            switch (type)
            {
                case 1:
                    rtype = "Falta";
                    break;
                case 2:
                    rtype = "Incapacidad";
                    break;
                case 3:
                    rtype = "Vacaciones";
                    break;
                case 4:
                    rtype = "Permisos";
                    break;
            }

            return rtype;
        }


        private bool ValidateScheduleHour(string employeeId, string date) 
        {


            return false;
        }



    }
}
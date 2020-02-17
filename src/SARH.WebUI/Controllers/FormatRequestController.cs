using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SARH.Core.Configuration;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Employee;
using SARH.WebUI.Models.FormatRequest;
using SARH.WebUI.Models.Formats;

namespace SARH.WebUI.Controllers
{
    public class FormatRequestController : Controller
    {
        [AllowAnonymous]
        [ActionName("Acceso")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [ActionName("Principal")]
        public IActionResult MainRequestFormat(string pinEmployee, 
            [FromServices] IRepository<PermissionType> _permissionTypeRepository, 
            [FromServices] IOrganigramaModelFactory organigramaModelFactory, 
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] INomipaqEmployeeVacationModelFactory nomipaqEmployeeVacations)
        {
            var data = System.Convert.FromBase64String(pinEmployee);
            string base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            var getEmployeeId = base64Decoded.Split('|');

            ViewBag.permissionType = _permissionTypeRepository.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });

            var employees = organigramaModelFactory.GetAllData().Employess;

            var info = organigramaModelFactory.GetEmployeeData(getEmployeeId[1].TrimStart(new Char[] { '0' }));

            FormatRequestModel model = new FormatRequestModel()
            {
                Employee = $"{info.GeneralInfo.Id}",
                FulName = $"{info.GeneralInfo.Id}-{info.GeneralInfo.FirstName} {info.GeneralInfo.LastName}",
                Picture = info.GeneralInfo.Picture,
                JobTitle = $"Puesto: {info.GeneralInfo.JobTitle}",
                Area = $"Area: {info.Area}",
                JobCenter = $"Centro de trabajo: {info.JobCenter}",
                EmployeeVacations = nomipaqEmployeeVacations.GetEmployeeVacations(getEmployeeId[1])
            };

            if (formatRepository.SearhItemsFor(y => y.EmployeeId.Equals(getEmployeeId[1].TrimStart(new Char[] { '0' }))).Any()) 
            {
                model.EmployeeFormats.AddRange(formatRepository.SearhItemsFor(y => y.EmployeeId.Equals(getEmployeeId[1].TrimStart(new Char[] { '0' }))).Select(r => new EmployeeFormatItem()
                {
                    Id = r.Id,
                    EmployeeId = r.EmployeeId,
                    ApprovalDate = r.ApprovalDate.HasValue ? r.ApprovalDate.Value.ToShortDateString() : "",
                    Comments = r.Comments,
                    CreateDate = r.CreateDate.ToShortDateString(),
                    EndDate = r.EndDate.ToShortDateString(),
                    PermissionType = _permissionTypeRepository.GetElement(r.PermissionType).Description,
                    StartDate = $"({r.StartDate.ToShortDateString()})-({r.EndDate.ToShortDateString()})",
                    EmployeeSubstitute = employees.Where(k=>k.Id.Equals(r.EmployeeSubstitute.TrimStart('0'))).FirstOrDefault().Name
                }));

                //var permissions = formatRepository.SearhItemsFor(y => y.EmployeeId.Equals("17") && )
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult SearchBy(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            string value = string.Empty;
            string id = string.Empty;
            int row = 0;

            var employee = _organigramaModelFactory.GetEmployeeData(inputtext.TrimStart(new Char[] { '0' }));
            if (employee.Area != null)
            {
                if (employee.RowId != null)
                {
                    id = $"{typesearch}|{employee.GeneralInfo.Id}";
                    value = $"{inputtext}-{ employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName}";
                    row = 1;
                }
            }

            return Json(new { id = id, value = value, rows = row });
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveFormat(FormatInputModel format, 
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory,
            [FromServices] IConfigurationManager configurationManager,
            [FromServices] INomipaqEmployeeVacationModelFactory nomipaqEmployeeVacations)
        {

            var vacations = nomipaqEmployeeVacations.GetEmployeeVacations(format.EmployeeId);
            string vacationsConfig = string.Empty;
            if (vacations != null)
            {
                vacationsConfig = JsonConvert.SerializeObject(vacations);
            }

            var element = new EmployeeFormat()
            {
                Comments = format.Comments,
                EmployeeId = format.EmployeeId.TrimStart(new Char[] { '0' }),
                EmployeeSubstitute = string.IsNullOrEmpty(format.EmployeeSubId) ? "0" : format.EmployeeSubId,
                EndDate = DateTime.Parse(format.EndDate),
                PermissionType = format.PermissionType,
                StartDate = DateTime.Parse(format.StartDate),
                CreateDate = DateTime.Today,
                ApprovalDate = null,
                ApprovalWorkFlow = "",
                EndTime = format.EndTime,
                StartTime = format.StartTime,
                WithPay = format.WithPay,
                NomipaqVacations = vacationsConfig
            };

            formatRepository.Create(element);

            var suplente = organigramaModelFactory.GetEmployeeData(format.EmployeeSubId.TrimStart(new Char[] { '0' }));
            var solicitante = organigramaModelFactory.GetEmployeeData(format.EmployeeId.TrimStart(new Char[] { '0' }));

            string urlRoute = $"http://{configurationManager.ServerIP}/FormatRequest/ConfirmacionSuplente?IdFormat={element.Id.ToString()}";

            if (suplente != null && !string.IsNullOrEmpty(suplente.GeneralInfo.Email))
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(suplente.GeneralInfo.Email);
                    mail.From = new MailAddress(configurationManager.MailUsername);
                    mail.Subject = "Solicitud de Confirmación para cubrir puesto";

                    string htmlString = $@"<html>
                      <body>
                      <p>Hola, <br>{suplente.GeneralInfo.FirstName} {suplente.GeneralInfo.LastName} {suplente.GeneralInfo.LastName2}</br></p>
                      <p><br>{solicitante.GeneralInfo.JobTitle}</br> solicita tu confirmación para cubrir su puesto durante el periodo:</p>
                      <p>{format.StartDate}-{format.EndDate}</p>
                      <p>Da click en el siguiente enlace para confirmar que estas enterado y aceptas su solicitud</p>
                      <p><a href = '{urlRoute}'>Confirmar solicitud</a></p>
                      </body>
                      </html>
                     ";

                    mail.Body = htmlString;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(configurationManager.MailServer, int.Parse(configurationManager.MailPort));
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential(configurationManager.MailUsername, configurationManager.MailUserpassword);
                    smtp.Send(mail);
                }
                catch
                {
                }
            }



            return Json(new { idformat = element.Id });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetFormatData(int id, 
            [FromServices] IRepository<EmployeeFormat> formatRepository, 
            [FromServices] IOrganigramaModelFactory _organigramaModelFactory,
            [FromServices] IRepository<PermissionType> _permissionTypeRepository)
        {
            StringBuilder sb = new StringBuilder();

            var result = formatRepository.GetElement(id);
            string type = _permissionTypeRepository.GetElement(result.PermissionType).Description;

            string name = "";
            if (result.EmployeeSubstitute != "0")
            {
                var employee = _organigramaModelFactory.GetEmployeeData(result.EmployeeSubstitute.TrimStart(new Char[] { '0' }));
                name = $"{employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName}";
            }

            if (type.ToLower().Contains("permiso") || type.ToLower().Contains("pase"))
            {
                sb.Append($"Hora inicial : {result.StartTime}{Environment.NewLine}");
                sb.Append($"Hora final : {result.EndTime}{Environment.NewLine}");
                if (result.WithPay)
                {
                    sb.Append($"Con goze de sueldo : Si{Environment.NewLine}");
                }
                else
                {
                    sb.Append($"Con goze de sueldo : No{Environment.NewLine}");
                }
            }
            else if (type.ToLower().Contains("vacacion"))
            {
                if (!string.IsNullOrEmpty(result.NomipaqVacations))
                {
                    EmployeeVacation empvac = JsonConvert.DeserializeObject<EmployeeVacation>(result.NomipaqVacations);
                    sb.Append($"Total de días : {empvac.TotalDias}{Environment.NewLine}");
                    sb.Append($"Total de días gozados : {empvac.DiasTomados}{Environment.NewLine}");
                    sb.Append($"Total de días disponibles : {empvac.DiasDisponibles}{Environment.NewLine}");
                }
            }

            return Json(new
            {
                type = type,
                comment = result.Comments,
                dateini = result.StartDate.ToShortDateString(),
                datefin = result.EndDate.ToShortDateString(),
                sustitute = name,
                config = sb.ToString()
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult DeleteFormatData(int id, [FromServices] IRepository<EmployeeFormat> formatRepository)
        {

            var result = formatRepository.GetElement(id);

            if (result != null) 
            {
                formatRepository.Delete(result);
            }

            return Json("ok");
        }

        [AllowAnonymous]
        [ActionName("ConfirmacionSuplente")]
        public IActionResult ConfirmFormat(string IdFormat,
        [FromServices] IRepository<EmployeeFormat> formatRepository)
        {

            var format = formatRepository.GetElement(int.Parse(IdFormat));
            ViewBag.AlreadyApprove = true;

            if (format != null) 
            {
                if (format.SubstituteApprove == false)
                {
                    format.SubstituteApprove = true;
                    formatRepository.Update(format);
                    ViewBag.AlreadyApprove = false;
                }
            }


            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult ValidatePIN(string pin,[FromServices] IRepository<EmployeeAditionalInfo> employeeRepository) 
        {
            bool isvalid = false;
            string employeeid = string.Empty;
            var getPIN = employeeRepository.SearhItemsFor(p => p.EMP_PIN.Equals(pin));

            if (getPIN != null) 
            {
                isvalid = true;
                employeeid = getPIN.FirstOrDefault().EMP_EmployeeID;
            }


            return Json(new { isvalid = isvalid, employeeid = employeeid });
        }












    }
}
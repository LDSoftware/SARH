using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using ISOSADataMigrationTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SARH.Core.Configuration;
using SARH.WebUI.Factories;
using SARH.WebUI.Models;
using SARH.WebUI.Models.Employee;
using SARH.WebUI.Models.FormatRequest;
using SARH.WebUI.Models.Formats;
using SmartAdmin.WebUI.Areas.Identity.Pages.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SARH.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private List<Breadcrumb> _breadcrum;
        private List<Hierarchy> _hierarchies;
        private List<EmployeeAditionalInfo> _employeeManager;

        public EmployeeController(IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository,
            IRepository<EmployeeAditionalInfo> isosaemployeesRepository)
        {
            _breadcrum = new List<Breadcrumb>();


            _hierarchies = isosaemployeesOrganigramaRepository.GetAll().Select(o => new Hierarchy()
            {
                Area = o.Area,
                Departamento = o.Departamento,
                IdentPuesto = o.IdentPuesto.HasValue ? o.IdentPuesto.Value.ToString().ToLower() : "NULL",
                Puesto = o.Puesto,
                RowGuid = o.RowGuid.ToString().ToLower()
            }).ToList();

            _employeeManager = isosaemployeesRepository.SearhItemsFor(u => u.EMP_StatusCode.Equals("Active")).ToList();


        }

        [ActionName("HierarchyAssigned")]
        public IActionResult Index(string assignedHGuid, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav("a9abae18-c608-45f2-9061-6dc035803fd7", _hierarchies));

            ViewBag.AssignedHGuid = assignedHGuid;
            ViewBag.EmployeeID = employeeID.TrimStart(new Char[] { '0' });

            return View(model);
        }

        [HttpPost]
        public JsonResult SetNewJob(string jobGuid, string employeeGuid, 
            [FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository,
            [FromServices] IRepository<EmployeeHistory> employeeHistoryRepo,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory) 
        {

            isosaemployeesOrganigramaRepository.CreateTableBackup("Horarios");

            var source = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.RowGuid.ToString().Equals(employeeGuid));
            var destiny = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.RowGuid.ToString().Equals(jobGuid));

            //var modellogin = httpContextAccessor.HttpContext.Session.GetString("loginmodel");
            //var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
            //string userName = loginInfo != null ? loginInfo.Email : null;

            //string IdEmp = organigramaModelFactory.GetEmployeeIDByRowGuid(source.FirstOrDefault().RowGuid);

            //employeeHistoryRepo.Create(new EmployeeHistory()
            //{
            //    EmployeeId = IdEmp,
            //    RegisterDate = DateTime.Now,
            //    Descripcion = "El empleado fue asignado a nuevo puesto",
            //    JobActual = destiny.FirstOrDefault().Puesto,
            //    JobLast = source.FirstOrDefault().Puesto,
            //    RowGuidActual = destiny.FirstOrDefault().RowGuid,
            //    RowGuidLast = source.FirstOrDefault().RowGuid,
            //    DateChange = DateTime.Now,
            //    UserId = userName
            //});

            Guid nGuid = Guid.NewGuid();
            var s = source.First();
            s.RowGuid = nGuid;
            var subs = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.IdentPuesto.ToString().Equals(employeeGuid)).ToList();
            isosaemployeesOrganigramaRepository.Update(s);
            if (subs.Any()) 
            {
                subs.ForEach((y) =>
                {
                    y.IdentPuesto = nGuid;
                    isosaemployeesOrganigramaRepository.Update(y);
                });
            }
            var d = destiny.First();
            d.RowGuid = Guid.Parse(employeeGuid);
            isosaemployeesOrganigramaRepository.Update(d);
            var dest = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.IdentPuesto.ToString().Equals(jobGuid)).ToList();
            if (dest.Any())
            {
                dest.ForEach((y) =>
                {
                    y.IdentPuesto = Guid.Parse(employeeGuid);
                    isosaemployeesOrganigramaRepository.Update(y);
                });
            }

            return Json("ok");
        }

        private List<ListItem> CreateNav(string rootId, List<Hierarchy> data)
        {
            List<ListItem> result = new List<ListItem>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).OrderBy(k => k.Puesto).ToList();

            var empName = _employeeManager.Where(h => h.HrowGuid.Value.ToString().ToLower().Equals(rootId));
            string empN = string.Empty;
            string empR = string.Empty;
            if (empName.Any())
            {
                empN = empName.FirstOrDefault().EMP_FirstName + " " + empName.FirstOrDefault().EMP_LastName;
                empR = empName.FirstOrDefault().EMP_EmployeeID;
            }

            ListItem m = new ListItem()
            {
                Text = char.ToUpper(element.Puesto[0]) + element.Puesto.Substring(1).ToLower(),
                I18n = element.RowGuid,
                Title = empN,
                Tags = element.Puesto.ToLower(),
                Route = Url.Action("EmployeeDetail", "Organigrama", new { employeeid = empR })
            };

            elements.ForEach(g =>
            {
                var t = CreateNav(g.RowGuid, data);
                m.Items.AddRange(t);
            });

            result.Add(m);

            return result;
        }

        [ActionName("HierarchyAssignedSub")]
        public IActionResult HierarchyPartial(string rowid, string empRowId, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav(rowid, _hierarchies));

            var pp = _hierarchies.Where(l => l.RowGuid.Equals(rowid));

            ViewBag.AssignedHGuid = empRowId;
            ViewBag.EmployeeID = employeeID;

            return View(model);
        }


        [ActionName("HierarchyAssignedReactive")]
        public IActionResult IndexReactive(string assignedHGuid, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav("a9abae18-c608-45f2-9061-6dc035803fd7", _hierarchies));

            ViewBag.AssignedHGuid = assignedHGuid;
            ViewBag.EmployeeID = employeeID.TrimStart(new Char[] { '0' });

            return View(model);
        }

        [ActionName("HierarchyAssignedSubReactive")]
        public IActionResult HierarchyPartialReactive(string rowid, string empRowId, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav(rowid, _hierarchies));

            var pp = _hierarchies.Where(l => l.RowGuid.Equals(rowid));

            ViewBag.AssignedHGuid = empRowId;
            ViewBag.EmployeeID = employeeID;

            return View(model);
        }

        [HttpPost]
        public JsonResult SetNewJobReactive(string jobGuid, string employeeId, 
            [FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository,
            [FromServices] IRepository<EmployeeAditionalInfo> employeAdditionalInfoRepository,
            [FromServices] IRepository<EmployeeHistory> employeeHistoryRepo,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory)
        {

            isosaemployeesOrganigramaRepository.CreateTableBackup("Horarios");
            var destiny = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.RowGuid.ToString().Equals(jobGuid));
            var employee = employeAdditionalInfoRepository.SearhItemsFor(f => f.EMP_EmployeeID.Equals(int.Parse(employeeId).ToString("00000")));

            var modellogin = httpContextAccessor.HttpContext.Session.GetString("loginmodel");
            var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
            string userName = loginInfo != null ? loginInfo.Email : null;

            var orgEmp = organigramaModelFactory.GetEmployeeData(employeeId);

            employeeHistoryRepo.Create(new EmployeeHistory()
            {
                EmployeeId = employee.FirstOrDefault().EMP_EmployeeID,
                RegisterDate = DateTime.Now,
                Descripcion = "El empleado fue asignado a nuevo puesto",
                JobActual = destiny.FirstOrDefault().Puesto,
                JobLast = orgEmp.GeneralInfo.JobTitle,
                RowGuidActual = destiny.FirstOrDefault().RowGuid,
                RowGuidLast = employee.FirstOrDefault().HrowGuid.Value,
                DateChange = DateTime.Now,
                UserId = userName
            });

            bool success = true;

            try
            {
                if (employee.Any())
                {
                    var emp = employee.First();
                    emp.HrowGuid = Guid.Parse(jobGuid);
                    employeAdditionalInfoRepository.Update(emp);
                }
            }
            catch
            {
                success = false;
            }


            return Json(new { success = success, guidresponse = jobGuid });
        }



        public List<Breadcrumb> CreateBreadcrum()
        {
            List<Breadcrumb> _breadC = new List<Breadcrumb>();
            return _breadC;
        }

        #region Formats


        [ActionName("RequestFormat")]
        public IActionResult MainRequestFormat(string idEmployee,
            [FromServices] IRepository<PermissionType> _permissionTypeRepository,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory,
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] INomipaqEmployeeVacationModelFactory nomipaqEmployeeVacations)
        {

            ViewBag.permissionType = _permissionTypeRepository.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });

            var employees = organigramaModelFactory.GetAllData().Employess;

            var info = organigramaModelFactory.GetEmployeeData(idEmployee);

            FormatRequestModel model = new FormatRequestModel()
            {
                Employee = $"{info.GeneralInfo.Id.TrimStart(new Char[] { '0' })}",
                FulName = $"{info.GeneralInfo.Id}-{info.GeneralInfo.FirstName} {info.GeneralInfo.LastName}",
                Picture = info.GeneralInfo.Picture,
                JobTitle = $"Puesto: {info.GeneralInfo.JobTitle}",
                Area = $"Area: {info.Area}",
                JobCenter = $"Centro de trabajo: {info.JobCenter}",
                EmployeeVacations = nomipaqEmployeeVacations.GetEmployeeVacations(int.Parse(idEmployee).ToString("00000"))
            };

            var formats = formatRepository.SearhItemsFor(y => y.EmployeeId.Equals(idEmployee));

            if (formats.Any())
            {
                model.EmployeeFormats.AddRange(formatRepository.SearhItemsFor(y => y.EmployeeId.Equals(idEmployee)).Select(r => new EmployeeFormatItem()
                {
                    Id = r.Id,
                    EmployeeId = r.EmployeeId,
                    ApprovalDate = r.ApprovalDate.HasValue ? r.ApprovalDate.Value.ToShortDateString() : "",
                    Comments = r.Comments,
                    CreateDate = r.CreateDate.ToShortDateString(),
                    EndDate = r.EndDate.ToShortDateString(),
                    PermissionType = _permissionTypeRepository.GetElement(r.PermissionType).Description,
                    StartDate = $"({r.StartDate.ToShortDateString()})-({r.EndDate.ToShortDateString()})",
                    EmployeeSubstitute = employees.Where(k => k.Id.Equals(r.EmployeeSubstitute.TrimStart('0'))).FirstOrDefault().Name
                }));
            }

            model.TotalPermissionsApproved = model.EmployeeFormats.Where(h => !string.IsNullOrEmpty(h.ApprovalDate)).Count();

            return View(model);
        }

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


        [HttpPost]
        public JsonResult SaveFormat(FormatInputModel format, 
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory,
            [FromServices] IConfigurationManager configurationManager,
            [FromServices] INomipaqEmployeeVacationModelFactory nomipaqEmployeeVacations)
        {

            var vacations = nomipaqEmployeeVacations.GetEmployeeVacations(int.Parse(format.EmployeeId).ToString("00000"));
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
                WithPay = format.WithPay,
                StartTime = format.StartTime,
                EndTime = format.EndTime,
                NomipaqVacations = vacationsConfig
            };

            formatRepository.Create(element);

            if (format.EmployeeSubId == null) 
            {
                format.EmployeeSubId = "99999";
            }

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

        [HttpPost]
        public JsonResult GetFormatData(int id, 
        [FromServices] IRepository<EmployeeFormat> formatRepository, 
        [FromServices] IOrganigramaModelFactory _organigramaModelFactory,
        [FromServices] IRepository<PermissionType> _permissionTypeRepository)
        {
            StringBuilder sb = new StringBuilder();

            var result = formatRepository.GetElement(id);
            string type = _permissionTypeRepository.GetElement(result.PermissionType).Description;

            var allPermission = formatRepository.SearhItemsFor(y => y.EmployeeId.Equals(result.EmployeeId));
            int totpermissions = 0;

            if (allPermission.Any())
            {
                var d = formatRepository.SearhItemsFor(y => y.EmployeeId.Equals(result.EmployeeId)).Select(r => new EmployeeFormatItem()
                {
                    Id = r.Id,
                    EmployeeId = r.EmployeeId,
                    ApprovalDate = r.ApprovalDate.HasValue ? r.ApprovalDate.Value.ToShortDateString() : "",
                    Comments = r.Comments,
                    CreateDate = r.CreateDate.ToShortDateString(),
                    EndDate = r.EndDate.ToShortDateString(),
                    PermissionType = _permissionTypeRepository.GetElement(r.PermissionType).Description,
                    StartDate = $"({r.StartDate.ToShortDateString()})-({r.EndDate.ToShortDateString()})"
                });

                totpermissions = d.Where(t => t.PermissionType.ToLower().Contains("permiso")).Count();
            }

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
                if (totpermissions > 0) 
                {
                    sb.Append($"Permisos registrados : {totpermissions}");
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


        #endregion









    }
}

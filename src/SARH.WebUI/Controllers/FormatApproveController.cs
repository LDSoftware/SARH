using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SARH.Core.PdfCreator;
using SARH.Core.PdfCreator.Interface;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Formats;
using SmartAdmin.WebUI.Areas.Identity.Pages.Account;

namespace SARH.WebUI.Controllers
{
    public class FormatApproveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Pendientes")]
        public IActionResult ViewAll(int idFormat,
            [FromServices] IOrganigramaModelFactory organigramModelFactory,
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] INotificationModelFactory notificationModelFactory,
            [FromServices] IHttpContextAccessor _httpContextAccessor)
        {
            FormatApproverPendingModel model = new FormatApproverPendingModel();
            var formats = notificationModelFactory.NotificaticonsItems;


            var format = formatRepository.GetElement(idFormat);
            if (format != null) 
            {
                var employee = organigramModelFactory.GetEmployeeData(format.EmployeeId);
                if (employee != null) 
                {
                    model.FormatEmployee.Area = employee.Area;
                    model.FormatEmployee.JobCenter = employee.JobCenter;
                    model.FormatEmployee.Departamento = employee.Departamento;
                    model.FormatEmployee.Name = $"{employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName} {employee.GeneralInfo.LastName2}";
                    model.FormatEmployee.PicturePath = employee.GeneralInfo.Picture;
                }

                var formatDetail = formats.Where(u => u.Id.Equals(idFormat));
                if (formatDetail.Any()) 
                {
                    model.FormatDetail = formatDetail.FirstOrDefault();
                }
            }

            return View(model);
        }

        [ActionName("FormatoInfo")]
        public IActionResult ViewDetail(int idFormat,
            [FromServices] IOrganigramaModelFactory organigramModelFactory,
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] INotificationModelFactory notificationModelFactory,
            [FromServices] IRepository<FormatApprover> formatApprover,
            [FromServices] IHttpContextAccessor _httpContextAccessor)
        {
            FormatApproverPendingModel model = new FormatApproverPendingModel();
            var formats = notificationModelFactory.NotificaticonsItems;
            var employees = organigramModelFactory.GetAllData();

            var format = formatRepository.GetElement(idFormat);
            if (format != null)
            {
                var employee = organigramModelFactory.GetEmployeeData(format.EmployeeId);
                if (employee != null)
                {
                    model.FormatEmployee.Area = employee.Area;
                    model.FormatEmployee.JobCenter = employee.JobCenter;
                    model.FormatEmployee.Departamento = employee.Departamento;
                    model.FormatEmployee.JobTitle = employee.GeneralInfo.JobTitle;
                    model.FormatEmployee.Name = $"{employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName} {employee.GeneralInfo.LastName2}";
                    model.FormatEmployee.PicturePath = employee.GeneralInfo.Picture;
                }


                var formatDetail = formats.Where(u => u.Id.Equals(idFormat));
                if (formatDetail.Any())
                {
                    model.FormatDetail = formatDetail.FirstOrDefault();
                    var employeeSust = organigramModelFactory.GetEmployeeData(format.EmployeeSubstitute);
                    model.FormatDetail.EmployeeSustitution = $"{employeeSust.GeneralInfo.FirstName} {employeeSust.GeneralInfo.LastName} {employeeSust.GeneralInfo.LastName2}";

                    List<FormatApprover> apprs = new List<FormatApprover>();

                    //Approbador Global
                    apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(string.Empty)
                        && i.Centro.Equals(string.Empty) && i.Departamento.Equals(string.Empty)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                    //Aprobador Area                    
                    apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                    //Aprobador Centro
                    apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)
                        && i.Centro.Equals(model.FormatEmployee.JobCenter)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                    //Aprobador Depto
                    apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)
                        && i.Centro.Equals(model.FormatEmployee.JobCenter) && i.Departamento.Equals(model.FormatEmployee.Departamento)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());




                    if (apprs.Any())
                    {
                        apprs.ForEach((a) =>
                        {
                            var approName = employees.Employess.Where(d => d.RowId.ToString().ToLower().Equals(a.RowGuid.ToString().ToLower())).FirstOrDefault();
                            if (approName != null) 
                            {
                                model.Approvers.Add(new ForrmatApproverWorkflow()
                                {
                                    Id = a.Id,
                                    RowId = a.RowGuid,
                                    Name = approName.Name
                                });
                            }
                        });
                    }

                }
                else
                {
                    model.FormatDetail = notificationModelFactory.AllNotificaticonsItems.Where(d => d.Id.Equals(format.Id)).FirstOrDefault();
                    var employeeSust = organigramModelFactory.GetEmployeeData(format.EmployeeSubstitute);
                    model.FormatDetail.EmployeeSustitution = $"{employeeSust.GeneralInfo.FirstName} {employeeSust.GeneralInfo.LastName} {employeeSust.GeneralInfo.LastName2}";

                    if (!string.IsNullOrEmpty(format.ApprovalWorkFlow))
                    {

                        List<FormatApprover> apprs = new List<FormatApprover>();

                        //Approbador Global
                        apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(string.Empty)
                            && i.Centro.Equals(string.Empty) && i.Departamento.Equals(string.Empty)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                        //Aprobador Area                    
                        apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                        //Aprobador Centro
                        apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)
                            && i.Centro.Equals(model.FormatEmployee.JobCenter)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                        //Aprobador Depto
                        apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)
                            && i.Centro.Equals(model.FormatEmployee.JobCenter) && i.Departamento.Equals(model.FormatEmployee.Departamento)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


                        var apps = JsonConvert.DeserializeObject<List<ForrmatApproverWorkflow>>(format.ApprovalWorkFlow);

                        apprs.ForEach(h => 
                        {

                            var t = apps.Where(k => k.RowId.Equals(h.RowGuid));
                            if (!t.Any()) 
                            {
                                var approName = employees.Employess.Where(d => d.RowId.ToString().ToLower().Equals(h.RowGuid.ToString().ToLower())).FirstOrDefault();
                                if (approName != null) 
                                {
                                    model.Approvers.Add(new ForrmatApproverWorkflow()
                                    {
                                        Id = h.Id,
                                        RowId = h.RowGuid,
                                        Name = approName.Name
                                    });
                                }
                            }
                        });


                        model.Approvers.AddRange(apps);
                    }
                }
            }

            return View(model);
        }



        public JsonResult Approved(int idFormat,string comments,
            [FromServices] IOrganigramaModelFactory organigramModelFactory,
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] INotificationModelFactory notificationModelFactory,
            [FromServices] IRepository<FormatApprover> formatApprover,
            [FromServices] IHttpContextAccessor _httpContextAccessor) 
        {

            bool alreadyApprove = false;
            var employees = organigramModelFactory.GetAllData();
            var format = formatRepository.GetElement(idFormat);
            List<ForrmatApproverWorkflow> approvers = new List<ForrmatApproverWorkflow>();
            var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");
            var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
            string userName = loginInfo != null ? loginInfo.Email : null;
            var approver = employees.Employess.Where(t => t.UserName.Equals(userName));

            var emp = employees.Employess.Where(j => j.Id.Equals(format.EmployeeId)).FirstOrDefault();

            List<FormatApprover> apprs = new List<FormatApprover>();

            //Approbador Global
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(string.Empty)
                && i.Centro.Equals(string.Empty) && i.Departamento.Equals(string.Empty)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            //Aprobador Area                    
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            //Aprobador Centro
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)
                && i.Centro.Equals(emp.JobCenter)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            //Aprobador Depto
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)
                && i.Centro.Equals(emp.JobCenter) && i.Departamento.Equals(emp.Category)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            int totApp = apprs.Count;

            if (format != null) 
            {
                if (string.IsNullOrEmpty(format.ApprovalWorkFlow))
                {
                    if (approver.Any())
                    {
                        ForrmatApproverWorkflow workflow = new ForrmatApproverWorkflow()
                        {
                            Id = idFormat,
                            Approved = "Si",
                            ApproveDate = DateTime.Now.ToShortDateString(),
                            Comments = comments,
                            Name = approver.FirstOrDefault().Name,
                            RowId = Guid.Parse(approver.FirstOrDefault().RowId)
                        };
                        approvers.Add(workflow);
                    }
                }
                else
                {
                    approvers = JsonConvert.DeserializeObject<List<ForrmatApproverWorkflow>>(format.ApprovalWorkFlow);
                    var app = approvers.Where(g => g.RowId.ToString().ToLower().Equals(approver.FirstOrDefault().RowId.ToLower()));
                    if (!app.Any())
                    {
                        ForrmatApproverWorkflow workflow = new ForrmatApproverWorkflow()
                        {
                            Id = idFormat,
                            Approved = "Si",
                            ApproveDate = DateTime.Now.ToShortDateString(),
                            Comments = comments,
                            Name = approver.FirstOrDefault().Name,
                            RowId = Guid.Parse(approver.FirstOrDefault().RowId)
                        };
                        approvers.Add(workflow);
                    }
                    else
                    {
                        alreadyApprove = true;
                    }
                }

                if (!alreadyApprove)
                {

                    if (approvers.Count.Equals(totApp)) 
                    {
                        format.ApprovalDate = DateTime.Now;
                    }

                    format.ApprovalWorkFlow = JsonConvert.SerializeObject(approvers);
                    formatRepository.Update(format);
                }
            }

            return Json(new { alreadyapprove = alreadyApprove });
        }



        public JsonResult Declined(int idFormat, string comments,
            [FromServices] IOrganigramaModelFactory organigramModelFactory,
            [FromServices] IRepository<EmployeeFormat> formatRepository,
            [FromServices] INotificationModelFactory notificationModelFactory,
            [FromServices] IRepository<FormatApprover> formatApprover,
            [FromServices] IHttpContextAccessor _httpContextAccessor)
                {

            bool alreadyApprove = false;
            var employees = organigramModelFactory.GetAllData();
            var format = formatRepository.GetElement(idFormat);
            List<ForrmatApproverWorkflow> approvers = new List<ForrmatApproverWorkflow>();
            var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");
            var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
            string userName = loginInfo != null ? loginInfo.Email : null;
            var approver = employees.Employess.Where(t => t.UserName.Equals(userName));

            var emp = employees.Employess.Where(j => j.Id.Equals(format.EmployeeId)).FirstOrDefault();

            List<FormatApprover> apprs = new List<FormatApprover>();

            //Approbador Global
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(string.Empty)
                && i.Centro.Equals(string.Empty) && i.Departamento.Equals(string.Empty)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            //Aprobador Area                    
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            //Aprobador Centro
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)
                && i.Centro.Equals(emp.JobCenter)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());


            //Aprobador Depto
            apprs.AddRange(formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)
                && i.Centro.Equals(emp.JobCenter) && i.Departamento.Equals(emp.Category)).Where(p => (apprs).All(p2 => p2.RowGuid != p.RowGuid)).ToList());

            int totApp = apprs.Count;

            if (format != null)
            {
                if (string.IsNullOrEmpty(format.ApprovalWorkFlow))
                {
                    if (approver.Any())
                    {
                        ForrmatApproverWorkflow workflow = new ForrmatApproverWorkflow()
                        {
                            Id = idFormat,
                            Approved = "No",
                            ApproveDate = DateTime.Now.ToShortDateString(),
                            Comments = comments,
                            Name = approver.FirstOrDefault().Name,
                            RowId = Guid.Parse(approver.FirstOrDefault().RowId)
                        };
                        approvers.Add(workflow);
                    }
                }
                else
                {
                    approvers = JsonConvert.DeserializeObject<List<ForrmatApproverWorkflow>>(format.ApprovalWorkFlow);
                    var app = approvers.Where(g => g.RowId.ToString().ToLower().Equals(approver.FirstOrDefault().RowId.ToLower()));
                    if (!app.Any())
                    {
                        ForrmatApproverWorkflow workflow = new ForrmatApproverWorkflow()
                        {
                            Id = idFormat,
                            Approved = "No",
                            ApproveDate = DateTime.Now.ToShortDateString(),
                            Comments = comments,
                            Name = approver.FirstOrDefault().Name,
                            RowId = Guid.Parse(approver.FirstOrDefault().RowId)
                        };
                        approvers.Add(workflow);
                    }
                    else
                    {
                        alreadyApprove = true;
                    }
                }

                if (!alreadyApprove)
                {

                    format.Declined = true;
                    format.ApprovalWorkFlow = JsonConvert.SerializeObject(approvers);
                    formatRepository.Update(format);
                }
            }

            return Json(new { alreadyapprove = alreadyApprove });
        }


        public FileResult PrintCurrentFormat(int idFormat,
            [FromServices] IEmployeeFormatModelFactory employeeFormatModelFactory,
            [FromServices] SARH.Core.Configuration.IConfigurationManager configManager,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory) 
        {

            IConfigPdf config = new ConfigPdf()
            {
                FontPathPdf = configManager.FontPathPdf,
                ImgPathPdf = configManager.ImgPathPdf,
                FontPathBarCode = configManager.FontPathBarCode
            };
            PdfManager manager = new PdfManager(config);

            var format = employeeFormatModelFactory.GetformatInfo(idFormat);
            var formats = employeeFormatModelFactory.GetAllFormatsByEmployee(format.EmployeeId);
            string path = System.IO.Path.GetTempPath();
            string empId = int.Parse(format.EmployeeId).ToString("00000");

            var empInfo = organigramaModelFactory.GetEmployeeData(format.EmployeeId);

            var sustitute = organigramaModelFactory.GetEmployeeData(format.Suplente);

            string fileName = $"{path}FormatNumber{idFormat}.pdf";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            try
            {
                manager.CreatePermissionFormat(new Core.PdfCreator.FormatData.DocumentInfoPdfData()
                {
                    TitleDocumento = "SOLICITUD DE PERMISOS",
                    FormatId = $"Fecha de Solicitud: {format.FechaSolicitud}",
                    Area = empInfo.Area,
                    JobTitle = empInfo.GeneralInfo.JobTitle,
                    EmployeeName = $"{empInfo.GeneralInfo.FirstName} {empInfo.GeneralInfo.LastName} {empInfo.GeneralInfo.LastName2}",
                    EmployeInfo = new Core.PdfCreator.FormatData.EmployeeInfoData()
                    {
                        EmployeeId = empInfo.GeneralInfo.Id,
                        EmployeeName = $"{empInfo.GeneralInfo.FirstName} {empInfo.GeneralInfo.LastName} {empInfo.GeneralInfo.LastName2}",
                        Rfc = empInfo.GeneralInfo.RFC,
                        Curp = empInfo.GeneralInfo.CURP,
                        NSS = empInfo.GeneralInfo.NSS,
                        BrithDate = empInfo.GeneralInfo.FechaNacimiento,
                        HireDate = empInfo.GeneralInfo.HireDate,
                        FireDate = "N/A",
                        PhotoPath = empInfo.GeneralInfo.Picture,
                        PersonalPhone = empInfo.PersonalInfo.Phone,
                        CellNumber = empInfo.PersonalInfo.CellPhone,
                        PersonalEmail = empInfo.PersonalInfo.Email,
                        Address = empInfo.PersonalInfo.Address,
                        City = empInfo.PersonalInfo.City,
                        State = empInfo.PersonalInfo.State,
                        Cp = empInfo.PersonalInfo.Zip,
                        ContactName = empInfo.EmergencyData.Name,
                        ContactRelation = empInfo.EmergencyData.Relacion,
                        ContactPhone = empInfo.EmergencyData.Phone,
                        JobTitle = empInfo.GeneralInfo.JobTitle
                    },
                    EmployeeSustitute = new Core.PdfCreator.FormatData.FormatEmployeeSustitute()
                    {
                        Name = $"{sustitute.GeneralInfo.FirstName} {sustitute.GeneralInfo.LastName} {sustitute.GeneralInfo.LastName2}",
                        JobTitle = sustitute.GeneralInfo.JobTitle
                    },
                    FormatPermission = new Core.PdfCreator.FormatData.FormatPermissionData()
                    {
                        Cause = format.Comments,
                        DateStart = format.StartDate,
                        DateEnd = format.EndDate,
                        TimeStart = format.StartTime,
                        TimeEnd = format.EndTime,
                        Comments = format.AdditionalInfo,
                        TotalPermissions = formats.Count()
                    }
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


        public FileResult PrintVacationFormat(int idFormat,
            [FromServices] IEmployeeFormatModelFactory employeeFormatModelFactory,
            [FromServices] SARH.Core.Configuration.IConfigurationManager configManager,
            [FromServices] IOrganigramaModelFactory organigramaModelFactory,
            [FromServices] INomipaqEmployeeVacationModelFactory nomipaqVacationModelFactory)
        {

            IConfigPdf config = new ConfigPdf()
            {
                FontPathPdf = configManager.FontPathPdf,
                ImgPathPdf = configManager.ImgPathPdf,
                FontPathBarCode = configManager.FontPathBarCode
            };
            PdfManager manager = new PdfManager(config);

            var format = employeeFormatModelFactory.GetformatInfo(idFormat);
            var formats = employeeFormatModelFactory.GetAllFormatsByEmployee(format.EmployeeId);
            string path = System.IO.Path.GetTempPath();
            string empId = int.Parse(format.EmployeeId).ToString("00000");
            var vacations = nomipaqVacationModelFactory.GetEmployeeVacations(int.Parse(format.EmployeeId).ToString("00000"));
            Core.PdfCreator.FormatData.FormatVacationData vacationInfo = new Core.PdfCreator.FormatData.FormatVacationData();

            var empInfo = organigramaModelFactory.GetEmployeeData(format.EmployeeId);

            var sustitute = organigramaModelFactory.GetEmployeeData(format.Suplente);

            if (vacations != null)
            {
                var formatDays = (DateTime.Parse(format.EndDate) - DateTime.Parse(format.StartDate)).TotalDays + 1;
                vacationInfo = new Core.PdfCreator.FormatData.FormatVacationData()
                {
                    AñosCumplidos = vacations.Antiguedad.ToString("00"),
                    DiasGozado = vacations.DiasTomados.ToString(),
                    DiasPendinteAñoAnteior = vacations.TotalDias.ToString(),
                    DiasPorAño = vacations.DiasPorAño.ToString(),
                    DiasPorGozar = vacations.DiasDisponibles.ToString(),
                    FechaIngreso = empInfo.GeneralInfo.HireDate,
                    PorGozar = Math.Round((vacations.DiasDisponibles - formatDays), 0).ToString(),
                    DiasSolicitados = formatDays.ToString("00"),
                    FechaInicial = format.StartDate,
                    FechaFinal = format.EndDate
                };
            }

            string fileName = $"{path}FormatNumber{idFormat}.pdf";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            try
            {
                manager.CreateVacationFormat(new Core.PdfCreator.FormatData.DocumentInfoPdfData()
                {
                    TitleDocumento = "SOLICITUD DE VACACIONES",
                    FormatId = $"Fecha de Solicitud: {format.FechaSolicitud}",
                    Area = empInfo.Area,
                    JobTitle = empInfo.GeneralInfo.JobTitle,
                    EmployeeName = $"{empInfo.GeneralInfo.FirstName} {empInfo.GeneralInfo.LastName} {empInfo.GeneralInfo.LastName2}",
                    EmployeInfo = new Core.PdfCreator.FormatData.EmployeeInfoData()
                    {
                        EmployeeId = empInfo.GeneralInfo.Id,
                        EmployeeName = $"{empInfo.GeneralInfo.FirstName} {empInfo.GeneralInfo.LastName} {empInfo.GeneralInfo.LastName2}",
                        Rfc = empInfo.GeneralInfo.RFC,
                        Curp = empInfo.GeneralInfo.CURP,
                        NSS = empInfo.GeneralInfo.NSS,
                        BrithDate = empInfo.GeneralInfo.FechaNacimiento,
                        HireDate = empInfo.GeneralInfo.HireDate,
                        FireDate = "N/A",
                        PhotoPath = empInfo.GeneralInfo.Picture,
                        PersonalPhone = empInfo.PersonalInfo.Phone,
                        CellNumber = empInfo.PersonalInfo.CellPhone,
                        PersonalEmail = empInfo.PersonalInfo.Email,
                        Address = empInfo.PersonalInfo.Address,
                        City = empInfo.PersonalInfo.City,
                        State = empInfo.PersonalInfo.State,
                        Cp = empInfo.PersonalInfo.Zip,
                        ContactName = empInfo.EmergencyData.Name,
                        ContactRelation = empInfo.EmergencyData.Relacion,
                        ContactPhone = empInfo.EmergencyData.Phone,
                        JobTitle = empInfo.GeneralInfo.JobTitle
                    },
                    EmployeeSustitute = new Core.PdfCreator.FormatData.FormatEmployeeSustitute()
                    {
                        Name = $"{sustitute.GeneralInfo.FirstName} {sustitute.GeneralInfo.LastName} {sustitute.GeneralInfo.LastName2}",
                        JobTitle = sustitute.GeneralInfo.JobTitle
                    },
                    FormatVacationData = vacationInfo
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





    }
}
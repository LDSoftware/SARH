using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

                    var apprs = formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)
                    && i.Centro.Equals(model.FormatEmployee.JobCenter) && i.Departamento.Equals(model.FormatEmployee.Departamento)).ToList();

                    if (apprs.Any())
                    {
                        apprs.ForEach((a) =>
                        {
                            var approName = employees.Employess.Where(d => d.RowId.ToString().ToLower().Equals(a.RowGuid.ToString().ToLower())).FirstOrDefault();
                            model.Approvers.Add(new ForrmatApproverWorkflow()
                            {
                                Id = a.Id,
                                RowId = a.RowGuid,
                                Name = approName.Name
                            });
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

                        var apprs = formatApprover.SearhItemsFor(i => i.Area.Equals(model.FormatEmployee.Area)
                        && i.Centro.Equals(model.FormatEmployee.JobCenter) && i.Departamento.Equals(model.FormatEmployee.Departamento)).ToList();
                        var apps = JsonConvert.DeserializeObject<List<ForrmatApproverWorkflow>>(format.ApprovalWorkFlow);

                        apprs.ForEach(h => 
                        {

                            var t = apps.Where(k => k.RowId.Equals(h.RowGuid));
                            if (!t.Any()) 
                            {
                                var approName = employees.Employess.Where(d => d.RowId.ToString().ToLower().Equals(h.RowGuid.ToString().ToLower())).FirstOrDefault();
                                model.Approvers.Add(new ForrmatApproverWorkflow()
                                {
                                    Id = h.Id,
                                    RowId = h.RowGuid,
                                    Name = approName.Name
                                });
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

            var apprs = formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)
            && i.Centro.Equals(emp.JobCenter) && i.Departamento.Equals(emp.Category)).ToList();

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

            var apprs = formatApprover.SearhItemsFor(i => i.Area.Equals(emp.Area)
            && i.Centro.Equals(emp.JobCenter) && i.Departamento.Equals(emp.Category)).ToList();

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



    }
}
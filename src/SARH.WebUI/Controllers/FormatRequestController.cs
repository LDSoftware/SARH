using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SARH.WebUI.Factories;
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
            [FromServices] IRepository<EmployeeFormat> formatRepository)
        {
            var data = System.Convert.FromBase64String(pinEmployee);
            string base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);

            ViewBag.permissionType = _permissionTypeRepository.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });

            var employees = organigramaModelFactory.GetAllData().Employess;

            var info = organigramaModelFactory.GetEmployeeData("17");

            FormatRequestModel model = new FormatRequestModel()
            {
                Employee = $"{info.GeneralInfo.Id}",
                FulName = $"{info.GeneralInfo.Id}-{info.GeneralInfo.FirstName} {info.GeneralInfo.LastName}",
                Picture = info.GeneralInfo.Picture,
                JobTitle = $"Puesto: {info.GeneralInfo.JobTitle}",
                Area = $"Area: {info.Area}",
                JobCenter = $"Centro de trabajo: {info.JobCenter}"
            };

            if (formatRepository.SearhItemsFor(y => y.EmployeeId.Equals("17")).Any()) 
            {
                model.EmployeeFormats.AddRange(formatRepository.SearhItemsFor(y => y.EmployeeId.Equals("17")).Select(r => new EmployeeFormatItem()
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
        public JsonResult SaveFormat(FormatInputModel format, [FromServices] IRepository<EmployeeFormat> formatRepository)
        {


            var element = new EmployeeFormat() 
            {                
                Comments = format.Comments,
                EmployeeId = format.EmployeeId,
                EmployeeSubstitute = string.IsNullOrEmpty(format.EmployeeSubId) ? "0" : format.EmployeeSubId,
                EndDate = DateTime.Parse(format.EndDate),
                PermissionType = format.PermissionType,
                StartDate = DateTime.Parse(format.StartDate),
                CreateDate = DateTime.Today,
                ApprovalDate = null,
                ApprovalWorkFlow = ""
            };

            formatRepository.Create(element);

            return Json(new { idformat = element.Id });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetFormatData(int id, [FromServices] IRepository<EmployeeFormat> formatRepository, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {

            var result = formatRepository.GetElement(id);
            

            string name = "";
            if (result.EmployeeSubstitute != "0")
            {
                var employee = _organigramaModelFactory.GetEmployeeData(result.EmployeeSubstitute.TrimStart(new Char[] { '0' }));
                name = $"{employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName}";
            }

            return Json(new { comment = result.Comments, dateini = result.StartDate.ToShortDateString(), datefin = result.EndDate.ToShortDateString(), sustitute = name });
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


    }
}
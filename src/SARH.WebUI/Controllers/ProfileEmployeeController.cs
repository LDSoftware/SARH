using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.EmployeeProfile;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SARH.WebUI.Controllers
{
    public class ProfileEmployeeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        #region Perfil del empleado

        [ActionName("PerfilEmpleado")]
        public IActionResult EmployeeProfile(string idEmployee,
            [FromServices]IRepository<EmployeeProfile> employeeProfileRepository,
            [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            EmployeeProfileModel model = new EmployeeProfileModel();
            string employeeFormat = int.Parse(idEmployee).ToString("00000");
            var row = employeeProfileRepository.SearhItemsFor(l => l.EmployeeId.TrimStart('0').Equals(employeeFormat.TrimStart('0')));
            if (row.Any())
            {
                var data = row.FirstOrDefault();
                model = JsonConvert.DeserializeObject<EmployeeProfileModel>(data.ProfileSectionValues);
                var info = organigramaModelFactory.GetEmployeeData(idEmployee);
                model.Area = info.Area;
                model.JobCenter = info.JobCenter;
                model.Puesto = info.GeneralInfo.JobTitle;
                model.EmployeeId = info.GeneralInfo.Id;
                model.Categoria = info.GeneralInfo.JobCenter;
                model.FirstName = info.GeneralInfo.FirstName;
                model.LastName = info.GeneralInfo.LastName;
                model.DocumentPath = data.DocumentPath;
            }
            else 
            {
                var info = organigramaModelFactory.GetEmployeeData(idEmployee);
                model.Area = info.Area;
                model.JobCenter = info.JobCenter;
                model.Puesto = info.GeneralInfo.JobTitle;
                model.EmployeeId = info.GeneralInfo.Id;
                model.Categoria = info.GeneralInfo.JobCenter;
                model.FirstName = info.GeneralInfo.FirstName;
                model.LastName = info.GeneralInfo.LastName;
                model.Edad = (DateTime.Now.Year - DateTime.Parse(info.GeneralInfo.FechaNacimiento).Year).ToString();
                model.Sexo = info.GeneralInfo.Sexo;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult SaveProfileData(EmployeeProfileModel model, 
            [FromServices]IRepository<EmployeeProfile> employeeProfileRepository,
            [FromServices]SARH.Core.Configuration.IConfigurationManager configurationManager) 
        {
            string employeeFormat = int.Parse(model.EmployeeId).ToString("00000");
            string documentPath = string.Empty;
            if (!string.IsNullOrEmpty(model.DocumentPath)) 
            {
                string path = configurationManager.EmployeeProfileDocumentPath.Replace("|EmpNumber|", model.EmployeeId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                System.IO.File.Delete($"{path}PerfilPuesto.pdf");
                System.IO.File.Move(model.DocumentPath, $"{path}PerfilPuesto.pdf");
                documentPath = $"{path}PerfilPuesto.pdf";
            }

            var row = employeeProfileRepository.SearhItemsFor(l => l.EmployeeId.Equals(model.EmployeeId));
            if (!row.Any())
            {
                employeeProfileRepository.Create(new EmployeeProfile()
                {
                    EmployeeId = model.EmployeeId,
                    ProfileSectionValues = JsonConvert.SerializeObject(model),
                    DocumentPath = documentPath
                });
            }
            else 
            {
                var data = row.FirstOrDefault();
                bool overwriteDoc = false;

                if (!string.IsNullOrEmpty(data.DocumentPath) && !string.IsNullOrEmpty(documentPath))
                {
                    overwriteDoc = true;
                }
                if (string.IsNullOrEmpty(data.DocumentPath) && !string.IsNullOrEmpty(documentPath))
                {
                    overwriteDoc = true;
                }


                data.ProfileSectionValues = JsonConvert.SerializeObject(model);
                data.DocumentPath = overwriteDoc ? documentPath : data.DocumentPath;
                employeeProfileRepository.Update(data);
            }

            return Json("Ok");
        }


        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {

            string filePath = Path.GetTempFileName();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
            else
            {
                filePath = string.Empty;
            }

            return Json(new { filetemp = filePath });
        }


        public FileResult ViewAssignedDocument(string filepath)
        {
            byte[] FileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(FileBytes, "application/pdf");
        }


        #endregion 



    }
}

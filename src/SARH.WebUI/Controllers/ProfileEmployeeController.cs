using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
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
            var row = employeeProfileRepository.SearhItemsFor(l => l.EmployeeId.Equals(employeeFormat));
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

        public JsonResult SaveProfileData(EmployeeProfileModel model, 
            [FromServices]IRepository<EmployeeProfile> employeeProfileRepository) 
        {
            string employeeFormat = int.Parse(model.EmployeeId).ToString("00000");
            var row = employeeProfileRepository.SearhItemsFor(l => l.EmployeeId.Equals(model.EmployeeId));
            if (!row.Any())
            {
                employeeProfileRepository.Create(new EmployeeProfile()
                {
                    EmployeeId = model.EmployeeId,
                    ProfileSectionValues = JsonConvert.SerializeObject(model)
                });
            }
            else 
            {
                var data = row.FirstOrDefault();
                data.ProfileSectionValues = JsonConvert.SerializeObject(model);
                employeeProfileRepository.Update(data);
            }

            return Json("Ok");
        }




        #endregion 



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Configuration;
using SARH.WebUI.Models.Organigrama;

namespace SARH.WebUI.Controllers
{
    public class NonWorkingDaysController : Controller
    {

        private readonly IRepository<NonWorkingDay> _repository;
        private readonly IRepository<NonWorkingDayException> _repositoryExceptions;

        public NonWorkingDaysController(IRepository<NonWorkingDay> repository, IRepository<NonWorkingDayException> repositoryExceptions)
        {
            _repository = repository;
            _repositoryExceptions = repositoryExceptions;
        }


        public IActionResult Index()
        {

            NonWorkingDayModel model = new NonWorkingDayModel()
            {
                NonWorkingDays = _repository.GetAll().Select(o => new NonWorkingModelItem() { Id = o.Id, Description = o.Description, Holiday = o.Holiday.ToShortDateString(), Year = o.Holiday.Year }).OrderByDescending(a => DateTime.Parse(a.Holiday)).ToList(),
                HolidayYears = _repository.GetAll().Select(i => new { year = i.Holiday.Year }).GroupBy(u => u.year).Select(r => r.Key).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult SaveHoliday(string holiday, string description, string[] selectedInputs)
        {
            string response = "fail";
            string message = string.Empty;
            if (!_repository.SearhItemsFor(i => i.Holiday.ToShortDateString().Equals(holiday)).Any())
            {
                _repository.Create(new NonWorkingDay()
                {
                    Holiday = DateTime.Parse(holiday),
                    Description = description
                });


                var last = _repository.GetLastAdded;

                if (selectedInputs.Length > 0) 
                {
                    foreach (var item in selectedInputs)
                    {
                        var empsId = item.Split('|');
                        _repositoryExceptions.Create(new NonWorkingDayException()
                        {
                            HolidayId = last.Id,
                            EmployeeId = empsId[1]
                        });
                    }
                }


                response = "ok";
            }
            else
            {
                message = "El régistro ya existe";
            }

            return Json(new { response = response, message = message });
        }


        [HttpPost]
        public JsonResult DeleteHoliday(int id)
        {
            string response = "fail";
            var element = _repository.SearhItemsFor(i => i.Id.Equals(id));
            if (element.Any())
            {
                _repository.Delete(element.FirstOrDefault());
                response = "ok";
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult SearchBy(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            string value = string.Empty;
            string id = string.Empty;
            int row = 0;
            var list = new List<dynamic>();

            switch (typesearch)
            {
                case "1":
                    var areas = _organigramaModelFactory.GetAllData().Employess.Where(e => e.Area.ToLower().Equals(inputtext.ToLower()));
                    if (areas.Any())
                    {
                        id = $"{typesearch}|{areas.FirstOrDefault().Area}";
                        value = areas.FirstOrDefault().Area;
                        list.Add(areas.Select(j => new { id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}", value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }).ToList());
                        row = areas.Count();
                    }
                    break;
                case "2":
                    var jobcenters = _organigramaModelFactory.GetAllData().Employess.Where(e => e.JobCenter.ToLower().Equals(inputtext.ToLower()));
                    if (jobcenters.Any())
                    {
                        id = $"{typesearch}|{jobcenters.FirstOrDefault().Area}";
                        value = jobcenters.FirstOrDefault().Area;
                        list.Add(jobcenters.Select(j =>
                        new {
                            id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}",
                            value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}"
                        }
                        ).ToList());

                        row = jobcenters.Count();
                    }
                    break;
                case "3":
                    var jobtitles = _organigramaModelFactory.GetAllData().Employess.Where(e => e.JobTitle.ToLower().Equals(inputtext.ToLower()));
                    if (jobtitles.Any())
                    {
                        id = $"{typesearch}|{jobtitles.FirstOrDefault().Area}";
                        value = jobtitles.FirstOrDefault().Area;
                        list.Add(jobtitles.Select(j => new { id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}", value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }).ToList());
                        row = jobtitles.Count();
                    }
                    break;
                case "4":
                    var employee = _organigramaModelFactory.GetEmployeeData(inputtext.TrimStart(new Char[] { '0' }));
                    if (employee.Area != null)
                    {
                        if (employee.RowId != null)
                        {
                            id = $"{typesearch}|{employee.GeneralInfo.Id}";
                            value = $"{inputtext}-{ employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName}";
                            list.Add("");
                            row = 1;
                        }
                    }
                    else
                    {
                        id = "-1";
                        value = "";
                        row = -1;
                    }

                    break;
                default:
                    break;
            }

            return Json(new { id = id, value = value, rows = row, details = list.Count != 0 ? list[0] : "" });
        }


    }
}
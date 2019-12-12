using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Models.Configuration;

namespace SARH.WebUI.Controllers
{
    public class NonWorkingDaysController : Controller
    {

        private readonly IRepository<NonWorkingDay> _repository;

        public NonWorkingDaysController(IRepository<NonWorkingDay> repository)
        {
            _repository = repository;
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
        public JsonResult SaveHoliday(string holiday, string description)
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

    }
}
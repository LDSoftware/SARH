using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Configuration;
using SARH.WebUI.Models.Organigrama;

namespace SARH.WebUI.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IRepository<Schedule> _repository;
        private readonly IRepository<EmployeeScheduleAssigned> _repositoryEmployeeSchedule;
        private readonly IRepository<EmployeeScheduleAssignedTemp> _repositoryEmployeeScheduleTemp;

        public ScheduleController(IRepository<Schedule> repository, IRepository<EmployeeScheduleAssigned> repositoryEmployeeSchedule, IRepository<EmployeeScheduleAssignedTemp> repositoryEmployeeScheduleTemp)
        {
            _repository = repository;
            _repositoryEmployeeSchedule = repositoryEmployeeSchedule;
            _repositoryEmployeeScheduleTemp = repositoryEmployeeScheduleTemp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public FileResult SchedulesDownloadCSV([FromServices] IOrganigramaModelFactory _organigramaModelFactory) 
        {
            var scheduleAssigned = _repositoryEmployeeSchedule.GetAll();
            var scheduleAssignedTemp = _repositoryEmployeeScheduleTemp.GetAll();
            var organigramaModel = _organigramaModelFactory.GetAllData().Employess;
            var sb = new StringBuilder();

            var model = new EmployeeScheduleAssignedModel();
            if (scheduleAssigned.Any())
            {
                model.EmployeeScheduleAssignedList = scheduleAssigned.Select(i => new EmployeeScheduleAssignedItem()
                {
                    EmployeeId = int.Parse(i.EmployeeId).ToString("00000"),
                    ScheduleMeal = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) ? $"{GetStartScheduleAssigned(i.IdScheduleMeal, 2)} - {GetEndScheduleAssigned(i.IdScheduleMeal, 2)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2),
                    ScheduleWorkday = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) ? $"{GetStartScheduleAssigned(i.IdScheduleWorkday, 1)} - {GetEndScheduleAssigned(i.IdScheduleWorkday, 1)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1),
                    EmployeeName = $"{organigramaModel.Where(k => k.Id.Equals(i.EmployeeId)).FirstOrDefault().Name }",
                    ToleranceWorkday = $"{i.ToleranceWorkday} Mins",
                    ToleranceMeal = $"{i.ToleranceMeal} Mins",
                    EsTemporal = scheduleAssignedTemp.Where(u => u.EmployeeId.Equals(i.EmployeeId)).Any() ? "Si" : "No"
                }).ToList();
            }

            sb.Append("Id Empleado,Nombre, Horario laboral, Tolerancia horario laboral, Horario comida,Tolerancia horario comida,Tiene Horario Temporal");

            model.EmployeeScheduleAssignedList.ForEach(d => 
            {
                sb.AppendLine();
                sb.Append($"{d.EmployeeId},{d.EmployeeName},{d.ScheduleWorkday},{d.ToleranceWorkday},{d.ScheduleMeal},{d.ToleranceMeal},{d.EsTemporal}");
            });


            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "AsignacionDeHorarios.csv");
        }


        #region Admin Schedule Metodos

        public IActionResult AdminSchedule()
        {
            ScheduleAdminModel model = new ScheduleAdminModel();

            model.ScheduleItems = _repository.GetAll().Select(x => new ScheduleItem()
            {
                Id = x.Id,
                Description = x.Description,
                EffectiveTime = x.EffectiveTimeStart.HasValue ? $"({x.EffectiveTimeStart.Value.ToShortDateString()})-({x.EffectiveTimeEnd.Value.ToShortDateString()})" : "",
                Enabled = x.Enabled == true ? "Si" : "No",
                EndHour = x.EndHour,
                StartHour = x.StartHour,
                TypeSchedule = GetScheduleType(x.TypeSchedule),
                StartHourAnticipated = x.StartHourAnticipated
            }).ToList();


            ViewBag.scheduletype = model.CatalogSchedule.Select(k => new SelectListItem() { Text = k.Descripcion, Value = k.Id.ToString() });


            return View(model);
        }

        [HttpPost]
        public JsonResult SaveScheduleNew(ScheduleItem sheduleInfo)
        {

            DateTime? effectiveTime = null;
            DateTime? efectiveTimeEnd = null;
            if (!string.IsNullOrEmpty(sheduleInfo.EffectiveTime))
            {
                string[] values = sheduleInfo.EffectiveTime.Split('|');
                if (!string.IsNullOrEmpty(values[0]))
                {
                    effectiveTime = DateTime.Parse(values[0]);
                    efectiveTimeEnd = DateTime.Parse(values[1]);
                }
            }

            _repository.Create(new Schedule()
            {
                Description = sheduleInfo.Description,
                EffectiveTimeStart = effectiveTime,
                EffectiveTimeEnd = efectiveTimeEnd,
                Enabled = true,
                EndHour = sheduleInfo.EndHour,
                StartHour = sheduleInfo.StartHour,
                TypeSchedule = int.Parse(sheduleInfo.TypeSchedule),
                StartHourAnticipated = sheduleInfo.StartHourAnticipated
            });

            return Json("Ok");
        }


        [HttpPost]
        public JsonResult DeleteSchedule(int id)
        {
            var row = _repository.GetElement(id);
            row.Enabled = false;
            _repository.Update(row);

            return Json("Ok");
        }


        #endregion


        #region Employee Schedule Assigned Methods

        public IActionResult AssignedSchedules([FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {

            var scheduleAssigned = _repositoryEmployeeSchedule.GetAll();
            var scheduleAssignedTemp = _repositoryEmployeeScheduleTemp.GetAll();
            var organigramaModel = _organigramaModelFactory.GetAllData().Employess;


            var model = new EmployeeScheduleAssignedModel();
            if (scheduleAssigned.Any())
            {
                model.EmployeeScheduleAssignedList = scheduleAssigned.Select(i => new EmployeeScheduleAssignedItem()
                {
                    Id = i.Id,
                    EmployeeId = int.Parse(i.EmployeeId).ToString("00000"),
                    ScheduleMeal = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) ? $"{GetStartScheduleAssigned(i.IdScheduleMeal, 2)} - {GetEndScheduleAssigned(i.IdScheduleMeal, 2)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2),
                    ScheduleWorkday = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) ? $"{GetStartScheduleAssigned(i.IdScheduleWorkday, 1)} - {GetEndScheduleAssigned(i.IdScheduleWorkday, 1)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1),
                    EmployeeName = $"{organigramaModel.Where(k => k.Id.Equals(i.EmployeeId)).FirstOrDefault().Name }",
                    ToleranceWorkday = $"{i.ToleranceWorkday} Mins",
                    ToleranceMeal = $"{i.ToleranceMeal} Mins",
                    EsTemporal = scheduleAssignedTemp.Where(u => u.EmployeeId.Equals(i.EmployeeId)).Any() ? "Si" : "No"
                }).ToList();
            }

            var dropdownSchedule = _repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(1) && !u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            });

            var dropdownScheduleMeal = _repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(2) && !u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            });

            ViewBag.dropdownSchedule = dropdownSchedule;
            ViewBag.dropdownScheduleMeal = dropdownScheduleMeal;


            List<SelectListItem> dropdownScheduleTemp = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Seleccione un horario" } };
            dropdownScheduleTemp.AddRange(_repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(1) && u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            }));


            List<SelectListItem> dropdownScheduleMealTemp = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Seleccione un horario" } };

            dropdownScheduleMealTemp.AddRange(_repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(2) && u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            }));


            ViewBag.dropdownScheduletemp = dropdownScheduleTemp;
            ViewBag.dropdownScheduleMealtemp = dropdownScheduleMealTemp;


            return View(model);
        }

        public JsonResult SearchBy(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            string value = string.Empty;
            string id = string.Empty;
            int row = 0;
            List<OrganigramaEmployeeModel> unassignedEmployee = new List<OrganigramaEmployeeModel>();
            var list = new List<dynamic>();
            int unnasigned = 0;

            switch (typesearch)
            {
                case "1":
                    var areas = _organigramaModelFactory.GetAllData().Employess.Where(e => e.Area.ToLower().Equals(inputtext.ToLower()));
                    if (areas.Any())
                    {
                        id = $"{typesearch}|{areas.FirstOrDefault().Area}";
                        value = areas.FirstOrDefault().Area;
                        areas.ToList().ForEach((f) =>
                        {
                            if (!_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(f.Id)).Any())
                            {
                                unassignedEmployee.Add(f);
                            }
                            else
                            {
                                unnasigned++;
                            }
                        });
                        list.Add(unassignedEmployee.Select(j => new { id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}", value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }).ToList());
                        row = areas.Count().Equals(unnasigned) ? -1 : unassignedEmployee.Count();
                    }
                    break;
                case "2":
                    var jobcenters = _organigramaModelFactory.GetAllData().Employess.Where(e => e.JobCenter.ToLower().Equals(inputtext.ToLower()));
                    if (jobcenters.Any())
                    {
                        id = $"{typesearch}|{jobcenters.FirstOrDefault().Area}";
                        value = jobcenters.FirstOrDefault().Area;
                        jobcenters.ToList().ForEach((f) =>
                        {
                            if (!_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(f.Id)).Any())
                            {
                                unassignedEmployee.Add(f);
                            }
                            else
                            {
                                unnasigned++;
                            }
                        });
                        list.Add(unassignedEmployee.Select(j =>
                        new {
                            id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}",
                            value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }
                        ).ToList());

                        row = jobcenters.Count().Equals(unnasigned) ? -1 : unassignedEmployee.Count();
                    }
                    break;
                case "3":
                    var jobtitles = _organigramaModelFactory.GetAllData().Employess.Where(e => e.JobTitle.ToLower().Equals(inputtext.ToLower()));
                    if (jobtitles.Any())
                    {
                        id = $"{typesearch}|{jobtitles.FirstOrDefault().Area}";
                        value = jobtitles.FirstOrDefault().Area;
                        jobtitles.ToList().ForEach((f) =>
                        {
                            if (!_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(f.Id)).Any())
                            {
                                unassignedEmployee.Add(f);
                            }
                            else
                            {
                                unnasigned++;
                            }
                        });
                        list.Add(unassignedEmployee.Select(j => new { id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}", value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }).ToList());
                        row = jobtitles.Count().Equals(unnasigned) ? -1 : unassignedEmployee.Count();
                    }
                    break;
                case "4":
                    var employee = _organigramaModelFactory.GetEmployeeData(inputtext.TrimStart(new Char[] { '0' }));
                    if (employee.Area != null && !_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(employee.GeneralInfo.Id.TrimStart(new Char[] { '0' }))).Any())
                    {
                        if (employee.RowId != null && !_repositoryEmployeeSchedule.GetAll().Where(d => d.EmployeeId.Equals(inputtext.TrimStart(new Char[] { '0' }))).Any())
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

        public JsonResult AssignedSchedule(string[] selectedInputs, string scheduleSelected, string scheduleMealSelected, string toleranceWD, string toleranceML, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            selectedInputs.ToList().ForEach((y) =>
            {
                var data = y.Split('|');
                var detail = _repositoryEmployeeSchedule.SearhItemsFor(i => i.EmployeeId.Equals(data[1].TrimStart(new Char[] { '0' })));
                if (!detail.Any())
                {
                    _repositoryEmployeeSchedule.Create(new EmployeeScheduleAssigned()
                    {
                        EmployeeId = data[1].TrimStart(new Char[] { '0' }),
                        IdScheduleMeal = int.Parse(scheduleMealSelected),
                        IdScheduleWorkday = int.Parse(scheduleSelected),
                        ToleranceMeal = int.Parse(toleranceML),
                        ToleranceWorkday = int.Parse(toleranceWD)
                    });
                }
            });

            return Json("ok");
        }


        public JsonResult SearchByTemp(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            string value = string.Empty;
            string id = string.Empty;
            int row = 0;
            List<OrganigramaEmployeeModel> unassignedEmployee = new List<OrganigramaEmployeeModel>();
            var list = new List<dynamic>();
            int unnasigned = 0;

            switch (typesearch)
            {
                case "1":
                    var areas = _organigramaModelFactory.GetAllData().Employess.Where(e => e.Area.ToLower().Equals(inputtext.ToLower()));
                    if (areas.Any())
                    {
                        id = $"{typesearch}|{areas.FirstOrDefault().Area}";
                        value = areas.FirstOrDefault().Area;
                        areas.ToList().ForEach((f) =>
                        {
                            if (!_repositoryEmployeeScheduleTemp.SearhItemsFor(e => e.EmployeeId.Equals(f.Id)).Any())
                            {
                                unassignedEmployee.Add(f);
                            }
                            else
                            {
                                unnasigned++;
                            }
                        });
                        list.Add(unassignedEmployee.Select(j => new { id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}", value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }).ToList());
                        row = areas.Count().Equals(unnasigned) ? -1 : unassignedEmployee.Count();
                    }
                    break;
                case "2":
                    var jobcenters = _organigramaModelFactory.GetAllData().Employess.Where(e => e.JobCenter.ToLower().Equals(inputtext.ToLower()));
                    if (jobcenters.Any())
                    {
                        id = $"{typesearch}|{jobcenters.FirstOrDefault().Area}";
                        value = jobcenters.FirstOrDefault().Area;
                        jobcenters.ToList().ForEach((f) =>
                        {
                            if (!_repositoryEmployeeScheduleTemp.SearhItemsFor(e => e.EmployeeId.Equals(f.Id)).Any())
                            {
                                unassignedEmployee.Add(f);
                            }
                            else
                            {
                                unnasigned++;
                            }
                        });
                        list.Add(unassignedEmployee.Select(j =>
                        new {
                            id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}",
                            value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}"
                        }
                        ).ToList());

                        row = jobcenters.Count().Equals(unnasigned) ? -1 : unassignedEmployee.Count();
                    }
                    break;
                case "3":
                    var jobtitles = _organigramaModelFactory.GetAllData().Employess.Where(e => e.JobTitle.ToLower().Equals(inputtext.ToLower()));
                    if (jobtitles.Any())
                    {
                        id = $"{typesearch}|{jobtitles.FirstOrDefault().Area}";
                        value = jobtitles.FirstOrDefault().Area;
                        jobtitles.ToList().ForEach((f) =>
                        {
                            if (!_repositoryEmployeeScheduleTemp.SearhItemsFor(e => e.EmployeeId.Equals(f.Id)).Any())
                            {
                                unassignedEmployee.Add(f);
                            }
                            else
                            {
                                unnasigned++;
                            }
                        });
                        list.Add(unassignedEmployee.Select(j => new { id = $"{typesearch}|{int.Parse(j.Id).ToString("00000")}", value = $"{int.Parse(j.Id).ToString("00000")}-{j.Name}" }).ToList());
                        row = jobtitles.Count().Equals(unnasigned) ? -1 : unassignedEmployee.Count();
                    }
                    break;
                case "4":
                    var employee = _organigramaModelFactory.GetEmployeeData(inputtext.TrimStart(new Char[] { '0' }));
                    if (employee.Area != null && !_repositoryEmployeeScheduleTemp.SearhItemsFor(e => e.EmployeeId.Equals(employee.GeneralInfo.Id.TrimStart(new Char[] { '0' }))).Any())
                    {
                        if (employee.RowId != null && !_repositoryEmployeeScheduleTemp.GetAll().Where(d => d.EmployeeId.Equals(inputtext.TrimStart(new Char[] { '0' }))).Any())
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

        public JsonResult AssignedScheduleTemp(string[] selectedInputs, string scheduleSelected, string scheduleMealSelected, string toleranceWD, string toleranceML, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            selectedInputs.ToList().ForEach((y) =>
            {
                var data = y.Split('|');
                var detail = _repositoryEmployeeScheduleTemp.SearhItemsFor(i => i.EmployeeId.Equals(data[1].TrimStart(new Char[] { '0' })));
                if (!detail.Any())
                {
                    _repositoryEmployeeScheduleTemp.Create(new EmployeeScheduleAssignedTemp()
                    {
                        EmployeeId = data[1].TrimStart(new Char[] { '0' }),
                        IdScheduleMeal = int.Parse(scheduleMealSelected),
                        IdScheduleWorkday = int.Parse(scheduleSelected),
                        ToleranceMeal = int.Parse(toleranceML),
                        ToleranceWorkday = int.Parse(toleranceWD)
                    });
                }
            });

            return Json("ok");
        }

        public JsonResult HaveTempSchedule(string employeeId)
        {
            string response = "fail";
            int idSchedule = 0;
            var row = _repositoryEmployeeScheduleTemp.SearhItemsFor(d => d.EmployeeId.Equals(employeeId.TrimStart(new Char[] { '0' })));

            if (row.Any())
            {
                response = "success";
                idSchedule = row.First().Id; 
            }

            return Json(new { response = response, idschedule = idSchedule });
        }

        public JsonResult DeleteTempSchedule(int idSchedule)
        {
            string response = "fail";

            var row = _repositoryEmployeeScheduleTemp.GetElement(idSchedule);

            if (row != null) 
            {
                _repositoryEmployeeScheduleTemp.Delete(row);
                response = "success";
            }

            return Json(new { response = response });
        }

        public JsonResult GetScheduleInfo(string employeeId,[FromServices] IOrganigramaModelFactory _organigramaModelFactory) 
        {

            var employee = _organigramaModelFactory.GetEmployeeData(employeeId.TrimStart(new Char[] { '0' }));
            var data = _repositoryEmployeeSchedule.SearhItemsFor(j => j.EmployeeId.Equals(employeeId.TrimStart(new Char[] { '0' })));
            int idswd = 0;
            int tswd = 0;
            int idsmd = 0;
            int tsmd = 0;
            string name = $"{employeeId} - {employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName}";


            if(data.Any())
            {
                idswd = data.First().IdScheduleWorkday;
                tswd = data.First().ToleranceWorkday;
                idsmd = data.First().IdScheduleMeal;
                tsmd = data.First().ToleranceMeal;
            }

            return Json(new { idswd = idswd, idsmd = idsmd, tswd = tswd, tsmd = tsmd, name = name });
        }


        public JsonResult UpdateScheduleInfo(string employeeId, int scheduleSelected, int scheduleMealSelected, int toleranceWD, int toleranceML)
        {

            var data = _repositoryEmployeeSchedule.SearhItemsFor(j => j.EmployeeId.Equals(employeeId.TrimStart(new Char[] { '0' })));

            if (data.Any())
            {
                data.First().IdScheduleWorkday = scheduleSelected;
                data.First().IdScheduleMeal = scheduleMealSelected;
                data.First().ToleranceWorkday = toleranceWD;
                data.First().ToleranceMeal = toleranceML;
                _repositoryEmployeeSchedule.Update(data.First());
            }

            return Json("Ok");
        }



        #endregion


        #region Utilities

        private string GetScheduleType(int typeSchedule)
        {
            string type = string.Empty;
            switch (typeSchedule)
            {
                case 1:
                    type = "Horario Laboral";
                    break;
                case 2:
                    type = "Horario de Comida";
                    break;
            }

            return type;
        }

        private string GetStartScheduleAssigned(int Id, int TypeSchedule) 
        {
            string response = string.Empty;
            var result = _repository.SearhItemsFor(i => i.Id.Equals(Id) && i.TypeSchedule.Equals(TypeSchedule));
            if (result.Any()) 
            {
                response = result.FirstOrDefault().StartHour;
            }
            return response;
        }

        private string GetEndScheduleAssigned(int Id, int TypeSchedule)
        {
            string response = string.Empty;
            var result = _repository.SearhItemsFor(i => i.Id.Equals(Id) && i.TypeSchedule.Equals(TypeSchedule));
            if (result.Any())
            {
                response = result.FirstOrDefault().EndHour;
            }
            return response;
        }

        private string GetScheduleAssigedTemp(string Employee, List<EmployeeScheduleAssignedTemp> listScheduleTemp, int TypeSchedule) 
        {
            string response = string.Empty;
            var result = listScheduleTemp.Where(j => j.EmployeeId.Equals(Employee)).FirstOrDefault();
            if (result != null) 
            {
                string startDate = GetStartScheduleAssigned(TypeSchedule == 1 ? result.IdScheduleWorkday : result.IdScheduleMeal, TypeSchedule);
                string endDate = GetEndScheduleAssigned(TypeSchedule == 1 ? result.IdScheduleWorkday : result.IdScheduleMeal, TypeSchedule);

                response = $"{startDate}-{endDate}";
            }

            return response;
        }

        private DateTime? GetScheduleEffectiveTimeStartTemp(int Id, int TypeSchedule)
        {
            DateTime? response = null;
            var result = _repository.SearhItemsFor(i => i.Id.Equals(Id) && i.TypeSchedule.Equals(TypeSchedule));
            if (result.Any())
            {
                response = result.FirstOrDefault().EffectiveTimeStart;
            }

            return response;
        }

        private DateTime? GetScheduleEffectiveTimeEndTemp(int Id, int TypeSchedule)
        {
            DateTime? response = null;
            var result = _repository.SearhItemsFor(i => i.Id.Equals(Id) && i.TypeSchedule.Equals(TypeSchedule));
            if (result.Any())
            {
                response = result.FirstOrDefault().EffectiveTimeEnd;
            }

            return response;
        }

        private bool IsValidcheduleAssigedTemp(string Employee, List<EmployeeScheduleAssignedTemp> listScheduleTemp, int TypeSchedule) 
        {
            bool response = false;
            var result = listScheduleTemp.Where(j => j.EmployeeId.Equals(Employee)).FirstOrDefault();

            if (result != null)
            {
                DateTime? startDate = GetScheduleEffectiveTimeStartTemp(TypeSchedule == 1 ? result.IdScheduleWorkday : result.IdScheduleMeal, TypeSchedule);
                DateTime? endDate = GetScheduleEffectiveTimeEndTemp(TypeSchedule == 1 ? result.IdScheduleWorkday : result.IdScheduleMeal, TypeSchedule);

                if (startDate.HasValue && endDate.HasValue)
                {
                    if (DateTime.Today >= startDate && DateTime.Today <= endDate)
                    {
                        response = true;
                    }
                }
            }

            return response;
        }

   
        #endregion


    }
}
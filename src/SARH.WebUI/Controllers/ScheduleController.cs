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
                    Id = i.Id,
                    EmployeeId = int.Parse(i.EmployeeId).ToString("00000"),
                    ScheduleMeal = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) ? $"L-V({GetStartScheduleAssigned(i.IdScheduleMeal, 2)} - {GetEndScheduleAssigned(i.IdScheduleMeal, 2)}) {ScheduleWeekendConfig(i.IdScheduleMealWeekEnd, 4)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2),
                    ScheduleWorkday = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) ? $"L-V({GetStartScheduleAssigned(i.IdScheduleWorkday, 1)} - {GetEndScheduleAssigned(i.IdScheduleWorkday, 1)}) {ScheduleWeekendConfig(i.IdScheduleWeekEnd, 3)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1),
                    EmployeeName = organigramaModel.Where(k => k.Id.Equals(i.EmployeeId)).Any() ? $"{organigramaModel.Where(k => k.Id.Equals(i.EmployeeId)).FirstOrDefault().Name }" : "",
                    ToleranceWorkday = IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) ? ScheduleTempTolerance(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) : $"{i.ToleranceWorkday} Mins",
                    ToleranceMeal = IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) ? ScheduleTempTolerance(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) : $"{i.ToleranceMeal} Mins",
                    EsTemporal = (!IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) && !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1)) ? "No" : "Si"
                }).ToList().Where(u => !u.EmployeeName.Equals(string.Empty)).ToList();
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
                TypeSchedule = GetScheduleType(x.TypeSchedule),
                StartHourAnticipated = x.StartHourAnticipated,
                StartHour = x.StartHour,
                EndHour = x.EndHour                
            }).ToList();


            ViewBag.scheduletype = model.CatalogSchedule.Select(k => new SelectListItem() { Text = k.Descripcion, Value = k.Id.ToString() });


            return View(model);
        }

        private string CombineScheduleInfo(string hour1, string hour2, bool weekend) 
        {
            string response = $"L-V({hour1}) ";

            if (weekend) 
            {
                response += $"S({hour2})";
            }


            return response;
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
                StartHourAnticipated = sheduleInfo.StartHourAnticipated,
                Weekend = int.Parse(sheduleInfo.TypeSchedule) > 2 ? true : false
            });

            return Json("Ok");
        }


        [HttpPost]
        public JsonResult DeleteSchedule(int id)
        {
            bool success = true;
            bool isvalidty = false;
            string message = string.Empty;
            var sch = _repositoryEmployeeSchedule.SearhItemsFor(d => d.IdScheduleWorkday.Equals(id) || d.IdScheduleMeal.Equals(id));
            var schtemp = _repositoryEmployeeScheduleTemp.SearhItemsFor(d => d.IdScheduleWorkday.Equals(id) || d.IdScheduleMeal.Equals(id));
            if (schtemp.Any()) 
            {
                var row = _repository.GetElement(id);
                if (row != null) 
                {
                    var now = DateTime.Now;
                    isvalidty = now >= row.EffectiveTimeStart && now <= row.EffectiveTimeEnd;
                }
            }

            if (!sch.Any() && !isvalidty)
            {
                var row = _repository.GetElement(id);
                row.Enabled = false;
                _repository.Update(row);
            }
            else
            {
                success = false;
                message = "El horario se encuentra asignado, no se puede deshabilitar, reasigne el horario e intente nuevamente";
            }



            return Json(new { success = success, message = message });
        }


        #endregion


        #region Employee Schedule Assigned Methods

        public IActionResult AssignedSchedules([FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {

            var scheduleAssigned = _repositoryEmployeeSchedule.GetAll();
            var scheduleAssignedTemp = _repositoryEmployeeScheduleTemp.GetAll().Where(i => i.Disabled == false);
            var organigramaModel = _organigramaModelFactory.GetAllData().Employess;


            var model = new EmployeeScheduleAssignedModel();
            if (scheduleAssigned.Any())
            {
                model.EmployeeScheduleAssignedList = scheduleAssigned.Select(i => new EmployeeScheduleAssignedItem()
                {
                    Id = i.Id,
                    EmployeeId = int.Parse(i.EmployeeId).ToString("00000"),
                    ScheduleMeal = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) ? $"L-V({GetStartScheduleAssigned(i.IdScheduleMeal, 2)} - {GetEndScheduleAssigned(i.IdScheduleMeal, 2)}) {ScheduleWeekendConfig(i.IdScheduleMealWeekEnd, 4)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2),
                    ScheduleWorkday = !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) ? $"L-V({GetStartScheduleAssigned(i.IdScheduleWorkday, 1)} - {GetEndScheduleAssigned(i.IdScheduleWorkday, 1)}) {ScheduleWeekendConfig(i.IdScheduleWeekEnd,3)}" : GetScheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1),
                    EmployeeName = organigramaModel.Where(k => k.Id.Equals(i.EmployeeId)).Any() ? $"{organigramaModel.Where(k => k.Id.Equals(i.EmployeeId)).FirstOrDefault().Name }" : "",
                    ToleranceWorkday = IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) ? ScheduleTempTolerance(i.EmployeeId, scheduleAssignedTemp.ToList(), 1) : $"{i.ToleranceWorkday} Mins",
                    ToleranceMeal = IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) ? ScheduleTempTolerance(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) : $"{i.ToleranceMeal} Mins",
                    EsTemporal = (!IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 2) && !IsValidcheduleAssigedTemp(i.EmployeeId, scheduleAssignedTemp.ToList(), 1)) ? "No" : "Si"
                }).ToList().Where(u => !u.EmployeeName.Equals(string.Empty)).ToList();
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


            var dropdownSchedulewkn = _repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(3) && !u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            });

            var dropdownScheduleMealwkn = _repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(4) && !u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            });

            ViewBag.dropdownSchedulewkn = dropdownSchedulewkn;
            ViewBag.dropdownScheduleMealwkn = dropdownScheduleMealwkn;



            List<SelectListItem> dropdownScheduleTempwkn = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Seleccione un horario" } };
            dropdownScheduleTempwkn.AddRange(_repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(3) && u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            }));


            List<SelectListItem> dropdownScheduleMealTempwkn = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Seleccione un horario" } };

            dropdownScheduleMealTempwkn.AddRange(_repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(4) && u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            }));


            ViewBag.dropdownScheduletempwkn = dropdownScheduleTempwkn;
            ViewBag.dropdownScheduleMealtempwkn = dropdownScheduleMealTempwkn;

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

        #region Employee Schedule Assigned Temp Methods

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
                    if (employee.Area != null && !_repositoryEmployeeScheduleTemp.SearhItemsFor(e => e.EmployeeId.Equals(employee.GeneralInfo.Id.TrimStart(new Char[] { '0' })) && e.Disabled == false).Any())
                    {
                        if (employee.RowId != null && !_repositoryEmployeeScheduleTemp.GetAll().Where(d => d.EmployeeId.Equals(inputtext.TrimStart(new Char[] { '0' })) && d.Disabled == false).Any())
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
                var detail = _repositoryEmployeeScheduleTemp.SearhItemsFor(i => i.EmployeeId.Equals(data[1].TrimStart(new Char[] { '0' })) && i.Disabled == false);
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
                row.Disabled = true;
                row.DisabledDate = DateTime.Now;
                _repositoryEmployeeScheduleTemp.Update(row);
                response = "success";
            }

            return Json(new { response = response });
        }


        #endregion

        #region Employee Schedule Assigned Weekend Methods

        public JsonResult SearchByWkn(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
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
                            if (!_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(f.Id) && !e.IdScheduleWeekEnd.Equals(0)).Any())
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
                            if (!_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(f.Id) && !e.IdScheduleWeekEnd.Equals(0)).Any())
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
                            if (!_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(f.Id) && !e.IdScheduleWeekEnd.Equals(0)).Any())
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
                    if (employee.Area != null && !_repositoryEmployeeSchedule.SearhItemsFor(e => e.EmployeeId.Equals(employee.GeneralInfo.Id.TrimStart(new Char[] { '0' })) && !e.IdScheduleWeekEnd.Equals(0)).Any())
                    {
                        if (employee.RowId != null && !_repositoryEmployeeSchedule.GetAll().Where(d => d.EmployeeId.Equals(inputtext.TrimStart(new Char[] { '0' })) && !d.IdScheduleWeekEnd.Equals(0)).Any())
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

        public JsonResult AssignedScheduleWkn(string[] selectedInputs, string scheduleSelected, string scheduleMealSelected, string toleranceWD, string toleranceML, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
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
                        IdScheduleWeekEnd = int.Parse(scheduleSelected),
                        ToleranceWeekEnd = int.Parse(toleranceWD),
                        IdScheduleMealWeekEnd = int.Parse(scheduleMealSelected),
                        ToleranceMealWeekEnd = int.Parse(toleranceML)
                    });
                }
                else 
                {
                    var row = detail.FirstOrDefault();
                    row.IdScheduleWeekEnd = int.Parse(scheduleSelected);
                    row.ToleranceWeekEnd = int.Parse(toleranceWD);
                    row.IdScheduleMealWeekEnd = int.Parse(scheduleMealSelected);
                    row.ToleranceMealWeekEnd = int.Parse(toleranceML);
                    _repositoryEmployeeSchedule.Update(row);
                }
            });

            return Json("ok");
        }

        #endregion

        #region Employee Schedule Assigned Temp Weekend Methods

        public JsonResult SearchByTempWkn(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
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
                    if (employee.Area != null && !_repositoryEmployeeScheduleTemp.SearhItemsFor(e => e.EmployeeId.Equals(employee.GeneralInfo.Id.TrimStart(new Char[] { '0' })) && e.Disabled == false).Any())
                    {
                        if (employee.RowId != null && !_repositoryEmployeeScheduleTemp.GetAll().Where(d => d.EmployeeId.Equals(inputtext.TrimStart(new Char[] { '0' })) && d.Disabled == false).Any())
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

        public JsonResult AssignedScheduleTempWkn(string[] selectedInputs, string scheduleSelected, string scheduleMealSelected, string toleranceWD, string toleranceML, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            selectedInputs.ToList().ForEach((y) =>
            {
                var data = y.Split('|');
                var detail = _repositoryEmployeeScheduleTemp.SearhItemsFor(i => i.EmployeeId.Equals(data[1].TrimStart(new Char[] { '0' })) && i.Disabled == false);
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

        public JsonResult HaveTempScheduleWkn(string employeeId)
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

        public JsonResult DeleteTempScheduleWkn(int idSchedule)
        {
            string response = "fail";

            var row = _repositoryEmployeeScheduleTemp.GetElement(idSchedule);

            if (row != null)
            {
                row.Disabled = true;
                row.DisabledDate = DateTime.Now;
                _repositoryEmployeeScheduleTemp.Update(row);
                response = "success";
            }

            return Json(new { response = response });
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
                case 3:
                    type = "Horario Laboral Sabatino";
                    break;
                case 4:
                    type = "Horario de Comida Sabatino";
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

        private string ScheduleWeekendConfig(int idScheduleWD, int scheduleType) 
        {
            string response = string.Empty;
            if (!idScheduleWD.Equals(0)) 
            {
                string startWD = GetStartScheduleAssigned(idScheduleWD, scheduleType);
                string endWD = GetEndScheduleAssigned(idScheduleWD, scheduleType);
                response = $"S({startWD}-{endWD})";
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

                response = $"L-V({startDate}-{endDate})";
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
                    DateTime now = DateTime.Today;
                    if (now >= startDate.Value && now <= endDate.Value)
                    {
                        response = true;
                    }
                }
            }

            return response;
        }

        private string ScheduleTempTolerance(string Employee, List<EmployeeScheduleAssignedTemp> listScheduleTemp, int TypeSchedule) 
        {
            string response = string.Empty;
            var result = listScheduleTemp.Where(j => j.EmployeeId.Equals(Employee)).FirstOrDefault();
            if (result != null)
            {
                if (TypeSchedule.Equals(1))
                {
                    response = $"{result.ToleranceWorkday} Mins";
                }
                else
                {
                    response = $"{result.ToleranceMeal} Mins";
                }
            }
            return response;
        }



        #endregion

    }
}
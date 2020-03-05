using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Dashboard;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using SARH.WebUI.Models.Dashboard;
using SARH.WebUI.Models.Report;

namespace SARH.WebUI.Factories
{
    public class DashboardModelFactory : IDashboardModelFactory
    {

        private readonly IRepository<DashboardData> _dashboardRepository;
        private readonly IRepository<PersonalDboardData> _personalDashboardRepository;
        private readonly IRepository<EmployeeFormat> _employeeFormatRepository;
        private readonly IRepository<EmployeeAditionalInfo> _employeeAdditionalInfo;
        private readonly IRepository<NonWorkingDay> _nonworkingDays;
        private readonly IRepository<NonWorkingDayException> _nonworkingDaysExeption;
        private readonly IRepository<EmployeeScheduleAssigned> _employeeScheduleAssigned;
        private readonly IRepository<Schedule> _scheduleRepository;
        private readonly IRepository<EmployeeScheduleDate> _employeeScheduleDate;
        private readonly IOrganigramaModelFactory _organigramaModelFactory;
        private const string dashboard = "CreateDashboardInfo";
        private const string dashboardweekend = "CreateDashboardInfoWeekEnd";


        public DashboardModelFactory(IRepository<DashboardData> dashboardRepository,
        IRepository<EmployeeFormat> employeeFormatRepository,
        IRepository<EmployeeAditionalInfo> employeeAdditionalInfo,
        IRepository<NonWorkingDay> nonworkingDays,
        IRepository<NonWorkingDayException> nonworkingDaysExeption,
        IRepository<EmployeeScheduleAssigned> employeeScheduleAssigned,
        IRepository<Schedule> scheduleRepository,
        IRepository<PersonalDboardData> personalDashboardRepository,
        IRepository<EmployeeScheduleDate> employeeScheduleDate,
        IOrganigramaModelFactory organigramaModelFactory)
        {
            this._dashboardRepository = dashboardRepository;
            this._employeeFormatRepository = employeeFormatRepository;
            this._employeeAdditionalInfo = employeeAdditionalInfo;
            this._nonworkingDays = nonworkingDays;
            this._nonworkingDaysExeption = nonworkingDaysExeption;
            this._employeeScheduleAssigned = employeeScheduleAssigned;
            this._scheduleRepository = scheduleRepository;
            this._personalDashboardRepository = personalDashboardRepository;
            this._employeeScheduleDate = employeeScheduleDate;
            this._organigramaModelFactory = organigramaModelFactory;
        }

        public DashboardModel GetDay(string date, DashboardFilters filters)
        {

            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToShortDateString();
            }

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", string.IsNullOrEmpty(date) ? DateTime.Now.ToShortDateString() : date));

            string spName = dashboard;
            if (DateTime.Parse(date).DayOfWeek == DayOfWeek.Saturday)
            {
                spName = dashboardweekend;
            }

            var t = this._dashboardRepository.GetStoredProcData(spName, param);
            List<DashboardEmployeeDetailModel> empsDetail = new List<DashboardEmployeeDetailModel>();

            if (DateTime.Parse(date).DayOfWeek == DayOfWeek.Saturday)
            {
                var wkworkers = this.WeekEndWorkers();

                DashboardData[] ops = new DashboardData[t.Count()];
                t.ToList().CopyTo(ops, 0);

                var wkemployees = (from wk in wkworkers
                                   join dd in ops on wk equals dd.EmployeeId
                                   select dd);

                t = null;
                t = new List<DashboardData>(wkemployees);
            }



            if (filters.StartJobDelay)
            {
                var a = t.Where(d => d.RetardoEntrada.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.EndJobAnticipate)
            {
                var a = t.Where(d => d.SalidaAnticipada.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.NoEntryCheckShow)
            {
                var a = t.Where(d => d.RetardoEntrada.Equals(2)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.FoodStartDelayShow)
            {
                var a = t.Where(d => d.SalidaAnticipadaComida.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.FoodEndDelayShow)
            {
                var a = t.Where(d => d.RetardoEntradaComida.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.NoFoodEntryCheckShow)
            {
                var a = t.Where(d => d.RetardoEntradaComida.Equals(2) && !d.RetardoEntrada.Equals(2)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }

            DashboardModel model = new DashboardModel()
            {
                EntryDelay = t.Where(d => d.RetardoEntrada.Equals(1)).Count(),
                EndDayDelay = t.Where(d => d.SalidaAnticipada.Equals(1)).Count(),
                FoodStartDelay = t.Where(d => d.SalidaAnticipadaComida.Equals(1)).Count(),
                FoodEndDelay = t.Where(d => d.RetardoEntradaComida.Equals(1)).Count(),
                NoFoodEntryCheck = t.Where(d => d.RetardoEntradaComida.Equals(2)).Count(),
                NoEntryCheck = t.Where(d => d.RetardoEntrada.Equals(2)).Count(),
                EmployeeDetail = empsDetail.GroupBy(o => o.ID).Select(o => o.FirstOrDefault()).ToList()
            };

            if (IsHoliday(date))
            {
                var exeptions = HaveHolidayExepcions(date);
                if (exeptions.Any())
                {
                    exeptions.ToList().ForEach(v =>
                    {
                        var emp = model.EmployeeDetail.Where(f => f.ID.Equals(v)).FirstOrDefault();
                        if (emp != null)
                        {
                            model.EmployeeDetail.Remove(emp);
                        }
                    });
                }
                else
                {
                    model.EmployeeDetail.Clear();
                }
            }

            return model;
        }

        public DashboardModel GetToday(DashboardFilters filters)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", DateTime.Now.ToShortDateString()));

            string spName = dashboard;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                spName = dashboardweekend;
            }
            var t = this._dashboardRepository.GetStoredProcData(spName, param);
            List<DashboardEmployeeDetailModel> empsDetail = new List<DashboardEmployeeDetailModel>();


            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                var wkworkers = this.WeekEndWorkers();

                DashboardData[] ops = new DashboardData[t.Count()];
                t.ToList().CopyTo(ops, 0);

                var wkemployees = (from wk in wkworkers
                                   join dd in ops on wk equals dd.EmployeeId
                                   select dd);

                t = null;
                t = new List<DashboardData>(wkemployees);
            }

            if (filters.StartJobDelay)
            {
                var a = t.Where(d => d.RetardoEntrada.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.EndJobAnticipate)
            {
                var a = t.Where(d => d.SalidaAnticipada.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.NoEntryCheckShow)
            {
                var a = t.Where(d => d.RetardoEntrada.Equals(2)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.FoodStartDelayShow)
            {
                var a = t.Where(d => d.SalidaAnticipadaComida.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.FoodEndDelayShow)
            {
                var a = t.Where(d => d.RetardoEntradaComida.Equals(1)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }
            if (filters.NoFoodEntryCheckShow)
            {
                var a = t.Where(d => d.RetardoEntradaComida.Equals(2) && !d.RetardoEntrada.Equals(2)).Select(r => new DashboardEmployeeDetailModel()
                {
                    Area = r.Area,
                    Name = r.EmployeeName,
                    ID = r.EmployeeId,
                    JobCenter = r.Centro,
                    JobTitle = r.Puesto,
                    DetailType = createEmployeeIncidencia(r)
                }).ToList();
                empsDetail.AddRange(a);
            }

            DashboardModel model = new DashboardModel()
            {
                EntryDelay = t.Where(d => d.RetardoEntrada.Equals(1)).Count(),
                EndDayDelay = t.Where(d => d.SalidaAnticipada.Equals(1)).Count(),
                FoodStartDelay = t.Where(d => d.SalidaAnticipadaComida.Equals(1)).Count(),
                FoodEndDelay = t.Where(d => d.RetardoEntradaComida.Equals(1)).Count(),
                NoFoodEntryCheck = t.Where(d => d.RetardoEntradaComida.Equals(2)).Count(),
                NoEntryCheck = t.Where(d => d.RetardoEntrada.Equals(2)).Count(),
                EmployeeDetail = empsDetail.GroupBy(o => o.ID).Select(o => o.FirstOrDefault()).ToList()
            };

            if (IsHoliday(DateTime.Now.ToShortDateString()))
            {
                var exeptions = HaveHolidayExepcions(DateTime.Now.ToShortDateString());
                if (exeptions.Any())
                {
                    exeptions.ToList().ForEach(v =>
                    {
                        var emp = model.EmployeeDetail.Where(f => f.ID.Equals(v)).FirstOrDefault();
                        if (emp != null)
                        {
                            model.EmployeeDetail.Remove(emp);
                        }
                    });
                }
                else
                {
                    model.EmployeeDetail.Clear();
                }
            }


            return model;
        }

        private string createEmployeeIncidencia(DashboardData data)
        {
            StringBuilder sb = new StringBuilder();

            switch (data.RetardoEntrada)
            {
                case 1:
                    sb.Append("Retardo horario entrada");
                    sb.AppendLine();
                    break;
                case 2:
                    sb.Append("SR horario entrada");
                    sb.AppendLine();
                    break;
                default:
                    break;
            }

            switch (data.SalidaAnticipadaComida)
            {
                case 1:
                    sb.Append("Salida anticipada horario de comida");
                    sb.AppendLine();
                    break;
                case 2:
                    sb.Append("SR salida horario de comida");
                    sb.AppendLine();
                    break;
                default:
                    break;
            }

            switch (data.RetardoEntradaComida)
            {
                case 1:
                    sb.Append("Retardo horario entrada comida");
                    sb.AppendLine();
                    break;
                case 2:
                    sb.Append("SR horario entrada comida");
                    sb.AppendLine();
                    break;
                default:
                    break;
            }

            switch (data.SalidaAnticipada)
            {
                case 1:
                    sb.Append("Salida anticipada horario salida");
                    break;
                case 2:
                    sb.Append("SR horario salida");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        public List<DashboardDataDetail> GetDashboardDetail(string employee, string date, DashboardFilters filters)
        {

            string cdate = string.IsNullOrEmpty(date) ? DateTime.Now.ToShortDateString() : date;

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", cdate));

            string spName = dashboard;
            if (DateTime.Parse(cdate).DayOfWeek == DayOfWeek.Saturday)
            {
                spName = dashboardweekend;
            }

            var t = this._dashboardRepository.GetStoredProcData(spName, param).Where(y => y.EmployeeId.Equals(employee));

            List<DashboardDataDetail> detail = new List<DashboardDataDetail>()
            {
                new DashboardDataDetail()
                {
                    horario = "Entrada",
                    hora = t.First().StartWorkDate,
                    registro = t.First().StartJobDay
                },
                new DashboardDataDetail()
                {
                    horario = "Salida comida",
                    hora = t.First().StartMealDate,
                    registro = t.First().StartMealDay
                },
                new DashboardDataDetail()
                {
                    horario = "Entrada Comida",
                    hora = t.First().EndMealDate,
                    registro = t.First().EndMealDay
                },
                new DashboardDataDetail()
                {
                    horario = "Salida",
                    hora = t.First().EndWorkDate,
                    registro = t.First().EndJobDay
                }
            };

            return detail;
        }

        public FormatApproved GetFormats(string date)
        {
            FormatApproved result = new FormatApproved();
            DateTime dashboardDate = DateTime.Parse(date);
            var formats = this._employeeFormatRepository.SearhItemsFor(u => u.StartDate >= dashboardDate && u.EndDate <= dashboardDate);
            var employees = this._employeeAdditionalInfo.GetAll();
            if (formats.Any())
            {
                var fmts = (from format in formats
                            join emp in employees on format.EmployeeId equals emp.EMP_EmployeeID
                            select new FormatApprovedItem()
                            {
                                EmployeeId = emp.EMP_EmployeeID,
                                EmployeeName = $"{emp.EMP_FirstName} {emp.EMP_LastName}",
                                Id = format.Id,
                                PeriodDates = $"{format.StartDate.ToShortDateString()}-{format.EndDate.ToShortDateString()}",
                                Type = ""
                            }).ToList();

                result.Date = date;
                result.TotalFormats = fmts.Count;
                result.FormaApprovedtItems.AddRange(fmts);
            }

            return result;
        }

        private bool IsHoliday(string date)
        {
            bool isholiday = false;
            var result = _nonworkingDays.SearhItemsFor(t => t.Holiday.Equals(DateTime.Parse(date)));
            if (result.Any())
            {
                isholiday = true;
            }
            return isholiday;
        }

        private List<string> HaveHolidayExepcions(string date)
        {
            List<string> exeptionList = new List<string>();
            var result = _nonworkingDays.SearhItemsFor(t => t.Holiday.Equals(DateTime.Parse(date)));
            if (result.Any())
            {
                var exeptions = this._nonworkingDaysExeption.SearhItemsFor(r => r.Id.Equals(result.FirstOrDefault().Id));
                if (exeptions.Any())
                {
                    exeptions.ToList().ForEach(x =>
                    {
                        exeptionList.Add(x.EmployeeId);
                    });
                }
            }

            return exeptionList;
        }

        private List<string> WeekEndWorkers()
        {
            List<string> result = new List<string>();

            var schedules = this._scheduleRepository.SearhItemsFor(u => u.Weekend == true);
            var employees = this._employeeScheduleAssigned.GetAll();

            if (schedules.Any() && employees.Any())
            {
                var wkworkers = (from emps in employees
                                 join sche in schedules on emps.IdScheduleWeekEnd equals sche.Id
                                 select new { emps.EmployeeId }).ToList();
                result.AddRange(wkworkers.Select(g => g.EmployeeId));
            }

            return result;
        }

        public PersonalDashboardData GetPersonalDashboardData(string employee, DateTime startDate, DateTime endDate)
        {
            PersonalDashboardData result = new PersonalDashboardData();
            List<DashboardData> t = new List<DashboardData>();

            var rows = this._employeeScheduleDate.SearhItemsFor(f => f.Employee.Equals(employee) && f.RegisterDate >= startDate && f.RegisterDate <= endDate);
            var empData = this._organigramaModelFactory.GetEmployeeData(employee.TrimStart(new Char[] { '0' }));
            var empAssignedSchedule = this._employeeScheduleAssigned.SearhItemsFor(g => g.EmployeeId.Equals(employee.TrimStart(new Char[] { '0' })));

            if (empAssignedSchedule.Any()) 
            {
                var scheduleA = this._scheduleRepository.GetElement(empAssignedSchedule.FirstOrDefault().IdScheduleWorkday);
                var scheduleB = this._scheduleRepository.GetElement(empAssignedSchedule.FirstOrDefault().IdScheduleMeal);
                var scheduleC = this._scheduleRepository.GetElement(empAssignedSchedule.FirstOrDefault().IdScheduleWeekEnd);
                var scheduleD = this._scheduleRepository.GetElement(empAssignedSchedule.FirstOrDefault().IdScheduleMealWeekEnd);

                var data = new PersonalDashboardData()
                {
                    Area = empData.Area,
                    Centro = empData.JobCenter,
                    EmployeeId = empData.GeneralInfo.Id,
                    Name = $"{empData.GeneralInfo.FirstName} {empData.GeneralInfo.LastName}",
                    Puesto = empData.GeneralInfo.JobTitle,
                    Picture = empData.GeneralInfo.Picture,
                    Days = rows.Where(d => d.RegisterDate.DayOfWeek != DayOfWeek.Sunday && scheduleC == null ? d.RegisterDate.DayOfWeek != DayOfWeek.Saturday : false).Select(u => new PersonalDashboardDataItem()
                    {
                        StartJobDay = u.StartJobDay,
                        StartMealDay = u.StartMealDay,
                        EndMealDay = u.EndMealDay,
                        EndJobDay = u.EndJobDay,
                        StartWorkDate = scheduleA.StartHour,
                        StartMealDate = scheduleB.StartHour,
                        EndMealDate = scheduleB.EndHour,
                        EndWorkDate = scheduleA.EndHour,
                        RegisterDate = u.RegisterDate.ToShortDateString(),
                        RetardoEntrada = !ScheduleNotCompliment(u.RegisterDate.ToShortDateString(), u.StartJobDay, scheduleA.StartHour) ? 1 : 0,
                        SalidaAnticipadaComida = !ScheduleNotComplimentRev(u.RegisterDate.ToShortDateString(), u.StartMealDay, scheduleB.StartHour) ? 1 : 0,
                        RetardoEntradaComida = !ScheduleNotCompliment(u.RegisterDate.ToShortDateString(), u.EndMealDay, scheduleB.EndHour) ? 1 : 0,
                        SalidaAnticipada = !ScheduleNotComplimentRev(u.RegisterDate.ToShortDateString(), u.EndJobDay, scheduleA.EndHour) ? 1 : 0
                    }).ToList()
                };

                result = data;

                var TotalDays = data.Days.Count();
                var retardos = data.Days.Sum(l => l.RetardoEntrada);
                var salidasanticipadacomida = data.Days.Sum(l => l.SalidaAnticipadaComida);
                var retardocomida = data.Days.Sum(l => l.RetardoEntradaComida);
                var salidaanticipada = data.Days.Sum(l => l.SalidaAnticipada);
                int PorcentajeRetardos = (int)Math.Round((double)(100 * retardos) / TotalDays);
                int PorcentajeSalidasAnticipadasComida = (int)Math.Round((double)(100 * salidasanticipadacomida) / TotalDays);
                int PorcentajeRetardosRegresoComida = (int)Math.Round((double)(100 * retardocomida) / TotalDays);
                int PorcentajeSalidasAnticipadas = (int)Math.Round((double)(100 * salidaanticipada) / TotalDays);

                result.TotalDays = TotalDays;
                result.PorcentajeRetardos = PorcentajeRetardos;
                result.PorcentajeSalidasAnticipadasComida = PorcentajeSalidasAnticipadasComida;
                result.PorcentajeRetardosRegresoComida = PorcentajeRetardosRegresoComida;
                result.PorcentajeSalidasAnticipadas = PorcentajeSalidasAnticipadas;
            }


            return result;
        }

        private bool ScheduleNotCompliment(string datereg, string hourreg, string hourschedule) 
        {
            if (DateTime.Parse(datereg) > DateTime.Now) 
            {
                return true;
            }

            bool isCompliment = false;

            DateTime dateA = DateTime.Parse($"{datereg} {hourreg}");
            DateTime dateB = DateTime.Parse($"{datereg} {hourschedule}");

            var diff = (dateB - dateA).TotalMinutes;
            if (diff > 0) 
            {
                isCompliment = true;
            }

            return isCompliment;
        }


        private bool ScheduleNotComplimentRev(string datereg, string hourreg, string hourschedule)
        {
            if (DateTime.Parse(datereg) > DateTime.Now || hourreg.Equals("00:00:00"))
            {
                return true;
            }

            bool isCompliment = false;

            DateTime dateA = DateTime.Parse($"{datereg} {hourreg}");
            DateTime dateB = DateTime.Parse($"{datereg} {hourschedule}");

            var diff = (dateA - dateB).TotalMinutes;
            if (diff > 0)
            {
                isCompliment = true;
            }

            return isCompliment;
        }



        public List<ReportEmployeeDetailModel> GetNoRegistry(string date) 
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", string.IsNullOrEmpty(date) ? DateTime.Now.ToShortDateString() : date));

            string spName = dashboard;
            if (DateTime.Parse(date).DayOfWeek == DayOfWeek.Saturday)
            {
                spName = dashboardweekend;
            }

            var t = this._dashboardRepository.GetStoredProcData(spName, param);
            List<ReportEmployeeDetailModel> empsDetail = new List<ReportEmployeeDetailModel>();

            if (DateTime.Parse(date).DayOfWeek == DayOfWeek.Saturday)
            {
                var wkworkers = this.WeekEndWorkers();

                DashboardData[] ops = new DashboardData[t.Count()];
                t.ToList().CopyTo(ops, 0);

                var wkemployees = (from wk in wkworkers
                                   join dd in ops on wk equals dd.EmployeeId
                                   where dd.RetardoEntrada == 2
                                   where dd.SalidaAnticipada ==2
                                   where dd.SalidaAnticipadaComida ==2
                                   where dd.RetardoEntradaComida == 2
                                   where DateTime.Parse($"{dd.RegisterDate} {dd.StartWorkDate}") < DateTime.Now
                                   select dd);

                t = null;
                t = new List<DashboardData>(wkemployees);
            }


            var a = t.Where(d => d.RetardoEntrada.Equals(2) && d.SalidaAnticipada.Equals(2) && d.SalidaAnticipadaComida.Equals(2) && d.RetardoEntradaComida.Equals(2)).Select(r => new ReportEmployeeDetailModel()
            {
                Area = r.Area,
                Name = r.EmployeeName,
                ID = r.EmployeeId,
                JobCenter = r.Centro,
                JobTitle = r.Puesto,
                DetailType = "Falta",
                Fecha = date,
                Type = 1
            }).ToList();
            empsDetail.AddRange(a);



            if (IsHoliday(date))
            {
                var exeptions = HaveHolidayExepcions(date);
                if (exeptions.Any())
                {
                    exeptions.ToList().ForEach(v =>
                    {
                        var emp = empsDetail.Where(f => f.ID.Equals(v)).FirstOrDefault();
                        if (emp != null)
                        {
                            empsDetail.Remove(emp);
                        }
                    });
                }
                else
                {
                    empsDetail.Clear();
                }
            }

            return empsDetail;
        }


    }
}

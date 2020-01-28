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

namespace SARH.WebUI.Factories
{
    public class DashboardModelFactory : IDashboardModelFactory
    {

        private readonly IRepository<DashboardData> _dashboardRepository;
        private readonly IRepository<EmployeeFormat> _employeeFormatRepository;
        private readonly IRepository<EmployeeAditionalInfo> _employeeAdditionalInfo;
        private readonly IRepository<NonWorkingDay> _nonworkingDays;
        private readonly IRepository<NonWorkingDayException> _nonworkingDaysExeption;
        private readonly IRepository<EmployeeScheduleAssigned> _employeeScheduleAssigned;
        private readonly IRepository<Schedule> _scheduleRepository;

        public DashboardModelFactory(IRepository<DashboardData> dashboardRepository,
        IRepository<EmployeeFormat> employeeFormatRepository,
        IRepository<EmployeeAditionalInfo> employeeAdditionalInfo,
        IRepository<NonWorkingDay> nonworkingDays,
        IRepository<NonWorkingDayException> nonworkingDaysExeption,
        IRepository<EmployeeScheduleAssigned> employeeScheduleAssigned,
        IRepository<Schedule> scheduleRepository)
        {
            this._dashboardRepository = dashboardRepository;
            this._employeeFormatRepository = employeeFormatRepository;
            this._employeeAdditionalInfo = employeeAdditionalInfo;
            this._nonworkingDays = nonworkingDays;
            this._nonworkingDaysExeption = nonworkingDaysExeption;
            this._employeeScheduleAssigned = employeeScheduleAssigned;
            this._scheduleRepository = scheduleRepository;
        }

        public DashboardModel GetDay(string date, DashboardFilters filters)
        {

            if (string.IsNullOrEmpty(date)) 
            {
                date = DateTime.Now.ToShortDateString();
            }

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", string.IsNullOrEmpty(date) ? DateTime.Now.ToShortDateString() : date));
            var t = this._dashboardRepository.GetStoredProcData("CreateDashboardInfo", param);
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
                AverageEntryDelay = 0,
                AverageJobPermission = 0,
                EntryDelay = t.Where(d => d.RetardoEntrada.Equals(1)).Count(),
                EndDayDelay = t.Where(d => d.SalidaAnticipada.Equals(1)).Count(),
                FoodStartDelay = t.Where(d => d.SalidaAnticipadaComida.Equals(1)).Count(),
                FoodEndDelay = t.Where(d => d.RetardoEntradaComida.Equals(1)).Count(),
                NoFoodEntryCheck = t.Where(d => d.RetardoEntradaComida.Equals(2)).Count(),
                NoEntryCheck = t.Where(d => d.RetardoEntrada.Equals(2)).Count(),
                EmployeeDetail = empsDetail.GroupBy(o=>o.ID).Select(o=>o.FirstOrDefault()).ToList()
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
            var t = this._dashboardRepository.GetStoredProcData("CreateDashboardInfo", param);
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
                AverageEntryDelay = 0,
                AverageJobPermission = 0,
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
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", string.IsNullOrEmpty(date) ? DateTime.Now.ToShortDateString() : date));
            var t = this._dashboardRepository.GetStoredProcData("CreateDashboardInfo", param).Where(y => y.EmployeeId.Equals(employee));

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



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Dashboard;
using ISOSA.SARH.Data.Repository;
using SARH.WebUI.Models.Dashboard;

namespace SARH.WebUI.Factories
{
    public class DashboardModelFactory : IDashboardModelFactory
    {

        private readonly IRepository<DashboardData> _dashboardRepository;


        public DashboardModelFactory(IRepository<DashboardData> dashboardRepository)
        {
            this._dashboardRepository = dashboardRepository;
        }

        public DashboardModel GetDay(string date, DashboardFilters filters)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", string.IsNullOrEmpty(date) ? DateTime.Now.ToShortDateString() : date));
            var t = this._dashboardRepository.GetStoredProcData("CreateDashboardInfo", param);
            List<DashboardEmployeeDetailModel> empsDetail = new List<DashboardEmployeeDetailModel>();

            //var emps = t.Select(r => new DashboardEmployeeDetailModel()
            //{
            //    Area = r.Area,
            //    Name = r.EmployeeName,
            //    ID = r.EmployeeId,
            //    JobCenter = r.Centro,
            //    JobTitle = r.Puesto,
            //    DetailType = createEmployeeIncidencia(r)
            //}).ToList();

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
                EmployeeDetail = empsDetail
            };

            return model;
        }

        public DashboardModel GetToday(DashboardFilters filters)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("@CurrentDate", DateTime.Now.ToShortDateString()));
            var t = this._dashboardRepository.GetStoredProcData("CreateDashboardInfo", param);
            List<DashboardEmployeeDetailModel> empsDetail = new List<DashboardEmployeeDetailModel>();

            //var emps = t.Select(r => new DashboardEmployeeDetailModel()
            //{
            //    Area = r.Area,
            //    Name = r.EmployeeName,
            //    ID = r.EmployeeId,
            //    JobCenter = r.Centro,
            //    JobTitle = r.Puesto,
            //    DetailType = createEmployeeIncidencia(r)
            //}).ToList();

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
                EmployeeDetail = empsDetail
            };

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


    }
}

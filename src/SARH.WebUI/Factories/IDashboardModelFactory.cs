using ISOSA.SARH.Data.Domain.Dashboard;
using SARH.WebUI.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IDashboardModelFactory
    {
        DashboardModel GetToday(DashboardFilters filters);
        DashboardModel GetDay(string date, DashboardFilters filters);
        List<DashboardDataDetail> GetDashboardDetail(string employee, string date, DashboardFilters filters);
        FormatApproved GetFormats(string date);
    }
}

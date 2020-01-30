using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Models.Organigrama;
using SARH.WebUI.Models.OrganizationChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IOrganigramaModelFactory
    {
        OrganigramaModel GetData(string organigramaPath);
        OrganigramaModel GetAllData();
        OrganigramaModel GetAllDataNoActive();
        OrganigramaEmployeeDetailModel GetEmployeeData(string employeeid);
        OrganigramaEmployeeDetailModel GetEmployeeDataNoActive(string employeeid);
        string GetNextIdEmployeeRepository(string optionSelected);
        List<OrganizationChartItem> GetAreas(string area = "");
        List<OrganizationChartItem> GetCentros(string area, string centro = "");
        List<OrganizationChartItem> GetDeptos(string area, string centro, string depto = "");
        List<EmployeeNotifyModel> GetEmployeesSchedulerTempNotify(string employeeId, string JobsTitle, bool GetRHManager);
    }
}

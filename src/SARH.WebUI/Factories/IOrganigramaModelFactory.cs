using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Models.Organigrama;
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
    }
}

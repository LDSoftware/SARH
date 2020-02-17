using SARH.WebUI.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IEmployeeFormatModelFactory
    {
        EmployeeFormatInfo GetformatInfo(int Id);
        List<EmployeeFormatInfo> GetAllFormats(DateTime StartDate, DateTime EndDate);
        List<EmployeeFormatInfo> GetAllApprovedFormats(DateTime StartDate);
        List<EmployeeFormatInfo> GetAllFormatsByEmployee(string EmployeeId);
    }
}

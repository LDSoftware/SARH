﻿using SARH.WebUI.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface INomipaqEmployeeVacationModelFactory
    {
        EmployeeVacation GetEmployeeVacations(string employeeID);
    }
}

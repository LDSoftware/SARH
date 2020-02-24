using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using SARH.WebUI.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public class NomipaqEmployeeVacationModelFactory : INomipaqEmployeeVacationModelFactory
    {

        private readonly IRepository<Nomipaq_nom10001> _nomipaqEmployee;
        private readonly IRepository<Nomipaq_nom10014> _nomipaqVacations;
        private List<EmployeeAntiguedad> _antiguedad = new List<EmployeeAntiguedad>();

        public NomipaqEmployeeVacationModelFactory(IRepository<Nomipaq_nom10001> nomipaqEmployee,IRepository<Nomipaq_nom10014> nomipaqVacations)
        {
            this._nomipaqEmployee = nomipaqEmployee;
            this._nomipaqVacations = nomipaqVacations;
            this.CreateAntiguedadTable();
        }

        public EmployeeVacation GetEmployeeVacations(string employeeID)
        {
            EmployeeVacation result = new EmployeeVacation();

            var employee = this._nomipaqEmployee.SearhItemsFor(g => g.codigoempleado.Equals(employeeID));
            if (employee.Any()) 
            {
                var emp = employee.FirstOrDefault();
                var vacations = this._nomipaqVacations.SearhItemsFor(j => j.idempleado.Equals(emp.idempleado));
                if (vacations.Any()) 
                {
                    var totaltomadas = vacations.Sum(t => t.diasvacaciones);
                    var antiguedad = Math.Abs(Math.Round((emp.fechaalta.Value - DateTime.Today).TotalDays / 365.0, 0));

                    var totalantiguedad = this._antiguedad.Where(s => s.Antiguedad.Equals(int.Parse(antiguedad.ToString())) || s.Antiguedad < int.Parse(antiguedad.ToString()));
                    var diasporantiguedad = this._antiguedad.Where(s => s.Antiguedad.Equals(int.Parse(antiguedad.ToString()))).FirstOrDefault();
                    if (totalantiguedad.Any()) 
                    {
                        var days = totalantiguedad.Sum(v => v.Dias);
                        result.Employee = employeeID;
                        result.Antiguedad = int.Parse(antiguedad.ToString());
                        result.DiasTomados = totaltomadas.Value;
                        result.TotalDias = days;
                        result.DiasDisponibles = days - totaltomadas.Value;
                        result.DiasPorAño = diasporantiguedad != null ? diasporantiguedad.Dias : 0;
                    }

                }
            }

            return result;
        }

        private void CreateAntiguedadTable()
        {
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 1, Dias = 6 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 2, Dias = 8 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 3, Dias = 10 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 4, Dias = 12 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 5, Dias = 14 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 6, Dias = 14 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 7, Dias = 14 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 8, Dias = 14 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 9, Dias = 14 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 10, Dias = 16 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 11, Dias = 16 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 12, Dias = 16 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 13, Dias = 16 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 14, Dias = 16 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 15, Dias = 18 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 16, Dias = 18 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 17, Dias = 18 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 18, Dias = 18 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 19, Dias = 18 });
            this._antiguedad.Add(new EmployeeAntiguedad() { Antiguedad = 20, Dias = 20 });

        }


    }
}

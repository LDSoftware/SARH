using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISOSADataMigrationTools
{
    public class Employee
    {
        public string EMP_EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string NSS { get; set; }
        public string BirthDate { get; set; }
        public string Status { get; set; }
        public string Deparment { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string EmergencyContactRelation { get; set; }
        public string DateOfHire { get; set; }
        public string EMailAddress { get; set; }
        public string Photo { get; set; }
        public string RecordId { get; set; }
        public string UserType { get; set; }
        public string Asignation { get; set; }
        public string RolType { get; set; }
        public string SDI { get; set; }
        public string StartJob { get; set; }
        public string EndJob { get; set; }
        public string StartFood { get; set; }
        public string EndFood { get; set; }
        public string Centro { get; set; }
        public string Area { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Perfil { get; set; }
        public string RowGuid { get; set; }
    }

    public class EmployeeManager
    {
        private const string employeeJson = "employees.json";

        public List<Employee> Employees { get; set; }

        public void Create() => LoadFromJson();

        private void LoadFromJson()
        {
            string jsonInfo = File.ReadAllText(employeeJson);
            var result = JsonConvert.DeserializeObject<List<Employee>>(jsonInfo);
            Employees = result;
        }
    }

}

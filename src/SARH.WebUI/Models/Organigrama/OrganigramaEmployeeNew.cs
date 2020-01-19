using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Organigrama.New
{
    public class OrganigramaEmployeeNew
    {
        public OrganigramaEmployeeNew()
        {
            GeneralInfo = new EmployeeDetailGeneralInfoNew();
            PersonalInfo = new EmployeeDetailPersonalInfoNew();
            EmergencyData = new EmployeeDetailEmergencyDataNew();
        }

        public string Area { get; set; }

        public string JobCenter { get; set; }

        public string RowId { get; set; }

        public string ContractType { get; set; }


        public EmployeeDetailGeneralInfoNew GeneralInfo { get; set; }
        public EmployeeDetailPersonalInfoNew PersonalInfo { get; set; }
        public EmployeeDetailEmergencyDataNew EmergencyData { get; set; }

    }

    public class EmployeeDetailGeneralInfoNew
    {
        public string ContratcType { get; set; }
        public string SelectionValue { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string NSS { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public string HireDate { get; set; }
        public string StartJob { get; set; }
        public string EndJob { get; set; }
        public string StartFood { get; set; }
        public string EndFood { get; set; }
        public string Observations { get; set; }
        public string Picture { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }

    }

    public class EmployeeDetailPersonalInfoNew
    {
        public string EmployeeId { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Suburb { get; set; }
        public string Zip { get; set; }
        public string Sick { get; set; }
        public string BloodType { get; set; }

    }

    public class EmployeeDetailEmergencyDataNew
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Relacion { get; set; }
        public string Phone { get; set; }
    }

}

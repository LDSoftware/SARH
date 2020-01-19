using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Organigrama
{
    public class OrganigramaEmployeeDetailModel
    {

        public OrganigramaEmployeeDetailModel()
        {
            GeneralInfo = new EmployeeDetailGeneralInfo();
            PersonalInfo = new EmployeeDetailPersonalInfo();
            EmergencyData = new EmployeeDetailEmergencyData();
            HardwareAssined = new List<HardwareAssignedModel>();
            SecurityEquipmentAssigned = new List<SafeEquimentAssignedModel>();
            Documents = new List<EmployeeDetailDocuments>();
        }

        public string Area { get; set; }

        public string JobCenter { get; set; }

        public string Departamento { get; set; }

        public string RowId { get; set; }

        public string HierarchyGuid { get; set; }

        public EmployeeDetailGeneralInfo GeneralInfo { get; set; }
        public EmployeeDetailPersonalInfo PersonalInfo { get; set; }
        public EmployeeDetailEmergencyData EmergencyData { get; set; }
        public List<HardwareAssignedModel> HardwareAssined { get; set; }
        public List<SafeEquimentAssignedModel> SecurityEquipmentAssigned { get; set; }
        public List<EmployeeDetailDocuments> Documents { get; set; }
    }

    public class EmployeeDetailGeneralInfo
    {
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

    public class EmployeeDetailPersonalInfo
    {
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Sick { get; set; }
        public string BloodType { get; set; }
        public string Suburb { get; set; }

    }

    public class EmployeeDetailEmergencyData
    {
        public string Name { get; set; }
        public string Relacion { get; set; }
        public string Phone { get; set; }
    }

    public class EmployeeDetailDocuments
    {
        public int Id { get; set; }
        public string DocumentPathInfo { get; set; }
        public string EmployeeID { get; set; }
        public string DocumentType { get; set; }
        public string IsValid { get; set; }
        public string Checked { get; set; }
    }

}

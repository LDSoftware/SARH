using System;
using System.Collections.Generic;
using System.Text;

namespace SARH.Core.PdfCreator.FormatData
{
    public class DocumentInfoPdfData
    {
        public DocumentInfoPdfData()
        {
            DocumentDetailInfo = new List<string>();
            EmployeInfo = new EmployeeInfoData();
        }

        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string Area { get; set; }
        public List<string> DocumentDetailInfo { get; set; }
        public string Comments { get; set; }
        public string TitleDocumento { get; set; }
        public string FormatId { get; set; }
        public string DetailDocument { get; set; }
        public string EmployeeInfoTitle { get; set; }
        public string DocumentObservationsTitle { get; set; }
        public string SingEmployeeTitle { get; set; }
        public string IdDocument { get; set; }
        public EmployeeInfoData EmployeInfo { get; set; }
    }

    public class EmployeeInfoData 
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Rfc { get; set; }
        public string Curp { get; set; }
        public string NSS { get; set; }
        public string BrithDate { get; set; }
        public string HireDate { get; set; }
        public string FireDate { get; set; }
        public string PhotoPath { get; set; }
        public string PersonalPhone { get; set; }
        public string CellNumber { get; set; }
        public string PersonalEmail { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cp { get; set; }
        public string ContactName { get; set; }
        public string ContactRelation { get; set; }
        public string ContactPhone { get; set; }
        public string JobTitle { get; set; }
        public string Salary { get; set; }
        public string StarWorkDay { get; set; }
        public string EndWorkDay { get; set; }
        public string StartMealDay { get; set; }
        public string EndMealDay { get; set; }
        public string AssignedEquipment { get; set; }
        public string Permissions { get; set; }
        public string DelayStartWork { get; set; }
        public string NoWorkDayRegistry { get; set; }
        public string Documents { get; set; }
    }
}

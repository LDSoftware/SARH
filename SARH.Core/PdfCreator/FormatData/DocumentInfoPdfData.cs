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
    }
}

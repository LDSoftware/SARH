using System;
using System.Collections.Generic;
using System.Text;

namespace SARH.Core.Configuration
{
    public interface IConfigurationManager
    {
        string AssignedHardwarePath { get; set; }
        string AssignedEquimentPath { get; set; }
        string EmployeeDocumentsPath { get; set; }
        string EmployeeVacationsPath { get; set; }
        string EmployeePermissionPath { get; set; }
        string EmployeePhotoPath { get; set; }
        string EmployeeProfileDocumentPath { get; set; }
        string ImgPathPdf { get; set; }
        string FontPathBarCode { get; set; }
        string FontPathPdf { get; set; }
        string MailUsername { get; set; }
        string MailUserpassword { get; set; }
        string MailServer { get; set; }
        string MailPort { get; set; }
        string ServerIP { get; set; }

    }
}

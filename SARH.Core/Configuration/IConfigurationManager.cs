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
        string ImgPathPdf { get; set; }
        string FontPathBarCode { get; set; }
        string FontPathPdf { get; set; }
    }
}

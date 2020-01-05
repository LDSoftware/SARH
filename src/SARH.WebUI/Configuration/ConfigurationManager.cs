using Microsoft.Extensions.Configuration;
using SARH.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {

        private IConfigurationBuilder _confgBuilder;
        private IConfigurationRoot _config;


        public ConfigurationManager(string jsonFile, string sectionName)
        {
            _confgBuilder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile(jsonFile);
            _config = _confgBuilder.Build();
        }


        public string AssignedHardwarePath
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:AssignedHardwarePath"];
                return result;
            }
            set { }
        }
        public string AssignedEquimentPath 
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:AssignedEquimentPath"];
                return result;
            }

            set { }
        }
        public string EmployeeDocumentsPath
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:EmployeeDocumentsPath"];
                return result;
            }
            set { }
        }
        public string EmployeeVacationsPath 
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:EmployeeVacationsPath"];
                return result;
            }

            set { }
        }
        public string EmployeePermissionPath 
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:EmployeePermissionPath"];
                return result;
            }

            set { }
        }
        public string EmployeePhotoPath 
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:EmployeePhotoPath"];
                return result;
            }

            set { }
        }
        public string EmployeeProfileDocumentPath
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:EmployeeProfileDocumentPath"];
                return result;
            }

            set { }
        }
        public string ImgPathPdf
        {
            get
            {
                var result = _config[$"ConfigPDFCreator:ImgPathPdf"];
                return result;
            }

            set { }
        }
        public string FontPathBarCode
        {
            get
            {
                var result = _config[$"ConfigPDFCreator:FontPathBarCode"];
                return result;
            }

            set { }
        }
        public string FontPathPdf
        {
            get
            {
                var result = _config[$"ConfigPDFCreator:FontPathPdf"];
                return result;
            }

            set { }
        }

    }
}

﻿using Microsoft.Extensions.Configuration;
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

        public string EmployeeDataInfo 
        {
            get
            {
                var result = _config[$"ApplicationFormatPath:EmployeeDataInfo"];
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
        public string MailUsername
        {
            get
            {
                var result = _config[$"ServerConfig:MailUsername"];
                return result;
            }

            set { }
        }
        public string MailUserpassword
        {
            get
            {
                var result = _config[$"ServerConfig:MailUserpassword"];
                return result;
            }

            set { }
        }
        public string MailServer
        {
            get
            {
                var result = _config[$"ServerConfig:MailServer"];
                return result;
            }

            set { }
        }
        public string MailPort
        {
            get
            {
                var result = _config[$"ServerConfig:MailPort"];
                return result;
            }

            set { }
        }
        public string ServerIP
        {
            get
            {
                var result = _config[$"ServerConfig:ServerIP"];
                return result;
            }

            set { }
        }
        public string NotifyToEmployee
        {
            get
            {
                var result = _config[$"SchedulerTempNotifications:NotifyToEmployee"];
                return result;
            }

            set { }
        }
        public bool NotifyToRHManager
        {
            get
            {
                var result = _config[$"SchedulerTempNotifications:NotifyToRHManager"];
                return bool.Parse(result);
            }

            set { }
        }



    }
}

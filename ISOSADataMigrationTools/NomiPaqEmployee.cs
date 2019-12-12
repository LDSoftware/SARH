﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISOSADataMigrationTools
{
    public class NomiPaqEmployee
    {
        public string idempleado { get; set; }
        public string codigoempleado { get; set; }
        public string nombre { get; set; }
        public string apellidopaterno { get; set; }
        public string apellidomaterno { get; set; }
        public string nombrelargo { get; set; }
        public string fechanacimiento { get; set; }
        public string lugarnacimiento { get; set; }
        public string estadocivil { get; set; }
        public string sexo { get; set; }
        public string Curp { get; set; }
        public string numerosegurosocial { get; set; }
        public string umf { get; set; }
        public string Rfc { get; set; }
        public string estadoempleado { get; set; }
        public string sueldodiario { get; set; }
        public string sueldointegrado { get; set; }
        public string fechaalta { get; set; }
        public string telefono { get; set; }
        public string codigopostal { get; set; }
        public string direccion { get; set; }
        public string poblacion { get; set; }
        public string estado { get; set; }
    }


    public class NomiPaqEmployeeManager
    {
        private const string organigramaJson = "nom10001.json";

        public List<NomiPaqEmployee> Employees { get; set; }

        public void Create() => LoadFromJson();

        private void LoadFromJson()
        {
            string jsonInfo = File.ReadAllText(organigramaJson);
            var result = JsonConvert.DeserializeObject<List<NomiPaqEmployee>>(jsonInfo);
            Employees = result;
        }

    }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISOSADataMigrationTools
{
    public class Organigrama
    {
        public string Area { get; set; }
        public string Centrodetrabajo { get; set; }
        public string Puesto { get; set; }
        public string Categoria { get; set; }
    }

    public class OrganigramaManager
    {
        private const string organigramaJson = "organigrama.json";

        public List<Organigrama> Organigrama { get; set; }

        public void Create() => LoadFromJson();

        private void LoadFromJson()
        {
            string jsonInfo = File.ReadAllText(organigramaJson);
            var result = JsonConvert.DeserializeObject<OrganigramaManager>(jsonInfo);
            Organigrama = result.Organigrama;
        }

    }
}

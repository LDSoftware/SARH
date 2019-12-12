using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISOSADataMigrationTools
{
    public class Hierarchy
    {

        public Hierarchy()
        {
            SubAreas = new List<Hierarchy>();
        }

        public string Area { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public string RowGuid { get; set; }
        public string IdentPuesto { get; set; }
        public string Centro { get; set; }
        public bool Available { get; set; }
        public List<Hierarchy> SubAreas { get; set; }
    }

    public class HierarchyManager
    {
        private const string hierarchyJson = "hierarchy.json";

        public List<Hierarchy> Hierarchies { get; set; }

        public void Create() => LoadFromJson();

        private void LoadFromJson()
        {
            string jsonInfo = File.ReadAllText(hierarchyJson);
            var result = JsonConvert.DeserializeObject<List<Hierarchy>>(jsonInfo);
            Hierarchies = result;
        }

    }


}

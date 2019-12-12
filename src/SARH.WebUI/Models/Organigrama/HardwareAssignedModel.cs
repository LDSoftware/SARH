﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Organigrama
{
    public class HardwareAssignedModel
    {
        public int Id { get; set; }
        public int IdHardwareDevice { get; set; }
        public string HadwareName { get; set; }
        public string AssignationDate { get; set; }
        public string Marca { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }
    }
}

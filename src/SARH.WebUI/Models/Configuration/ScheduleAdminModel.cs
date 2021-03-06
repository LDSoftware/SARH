﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Configuration
{
    public class ScheduleAdminModel
    {
        public ScheduleAdminModel()
        {
            ScheduleItems = new List<ScheduleItem>();
            CatalogSchedule = new List<ScheduleCatalogItem>() 
            {
                new ScheduleCatalogItem(){ Id = 1, Descripcion ="Horario Laboral" },
                new ScheduleCatalogItem(){ Id = 2, Descripcion ="Horario Comida"  },
                new ScheduleCatalogItem(){ Id = 3, Descripcion ="Horario Laboral Sabatino" },
                new ScheduleCatalogItem(){ Id = 4, Descripcion ="Horario Comida Sabatino"  }
            };
        }

        public List<ScheduleItem> ScheduleItems { get; set; }
        public List<ScheduleCatalogItem> CatalogSchedule { get; set; }
    }

    public class ScheduleItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Enabled { get; set; }
        public string EffectiveTime { get; set; }
        public string TypeSchedule { get; set; }
        public int StartHourAnticipated { get; set; }
        public string StartHourWke { get; set; }
        public string EndHourWke { get; set; }
    }

    public class ScheduleCatalogItem
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

}

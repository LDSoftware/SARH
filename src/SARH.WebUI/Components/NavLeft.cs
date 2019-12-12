using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SARH.WebUI.Models;
using System.Collections.Generic;
using SARH.WebUI.Models.Hierarchy;
using ISOSADataMigrationTools;
using Newtonsoft.Json;

namespace SARH.WebUI.Components
{
    public class NavLeftViewComponent : ViewComponent
    {
        List<HierarchyModel> list = new List<HierarchyModel>();
        HierarchyManager manager = new HierarchyManager();

        public IViewComponentResult Invoke()
        {
            List<ListItem> items = new List<ListItem>();
            manager.Create();

            var dta = manager.Hierarchies;


            var pp = CreateNav("a9abae18-c608-45f2-9061-6dc035803fd7", dta);

            var tt = CreateHierarchyOriginal("a9abae18-c608-45f2-9061-6dc035803fd7", dta);

            string kk = JsonConvert.SerializeObject(tt);


            //var organigramaManager = new ISOSADataMigrationTools.OrganigramaManager();
            //organigramaManager.Create();
            //var areas = organigramaManager.Organigrama.GroupBy(g => g.Area).Select(i => new { name = i.Key.ToLower() }).ToList();
            //areas.ForEach((area) => 
            //{
            //    var jobcenter = organigramaManager.Organigrama.Where(j => j.Centrodetrabajo != null).Where(o => o.Area.ToLower().Equals(area.name)).GroupBy(g => g.Centrodetrabajo).Select(y => new { name = y.Key.ToLower() }).ToList();
            //    ListItem litem = new ListItem()
            //    {
            //        Text = char.ToUpper(area.name[0]) + area.name.Substring(1),
            //        I18n = "nav.organigrama." + area.name,
            //        Title = char.ToUpper(area.name[0]) + area.name.Substring(1),
            //        Tags = area.name,
            //    };
            //    jobcenter.ForEach((jcenter) => 
            //    {
            //        litem.Items.Add(new ListItem()
            //        {
            //            Text = char.ToUpper(jcenter.name[0]) + jcenter.name.Substring(1),
            //            I18n = "nav.organigrama." + area.name + "." + jcenter.name,
            //            Title = char.ToUpper(jcenter.name[0]) + jcenter.name.Substring(1),
            //            Tags = jcenter.name
            //        });
            //    });
            //    items.Add(litem);
            //});

            return View(pp);
        }

        private List<ListItem> CreateNav(string rootId, List<Hierarchy> data)
        {
            List<ListItem> result = new List<ListItem>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).ToList();

            ListItem m = new ListItem()
            {
                Text = char.ToUpper(element.Puesto[0]) + element.Puesto.Substring(1).ToLower(),
                I18n = "nav.hierarchy." + element.Puesto.ToLower(),
                Title = char.ToUpper(element.Puesto[0]) + element.Puesto.Substring(1).ToLower(),
                Tags = element.Puesto.ToLower()
            };

            elements.ForEach(g => 
            {
                var t = CreateNav(g.RowGuid, data);
                m.Items.AddRange(t);
            });

            result.Add(m);

            return result;
        }


        private List<HierarchyModel> CreateHierarchy(string rootId, List<Hierarchy> data)
        {
            List<HierarchyModel> result = new List<HierarchyModel>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).ToList();

            HierarchyModel m = new HierarchyModel()
            {
                Area = element.Area
            };
            elements.ForEach(g =>
            {
                var t = CreateHierarchy(g.RowGuid, data);
                m.SubArea.AddRange(t);
            });

            result.Add(m);

            return result;
        }



        private List<Hierarchy> CreateHierarchyOriginal(string rootId, List<Hierarchy> data)
        {
            List<Hierarchy> result = new List<Hierarchy>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).ToList();

            Hierarchy m = element;
            elements.ForEach(g =>
            {
                var t = CreateHierarchyOriginal(g.RowGuid, data);
                m.SubAreas.AddRange(t);
            });

            result.Add(m);

            return result;
        }


    }
}

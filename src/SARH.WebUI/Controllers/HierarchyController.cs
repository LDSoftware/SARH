using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using ISOSADataMigrationTools;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Models;

namespace SARH.WebUI.Controllers
{
    public class HierarchyController : Controller
    {
        private List<Breadcrumb> _breadcrum;
        private List<Hierarchy> _hierarchies;
        private List<EmployeeAditionalInfo> _employeeManager;

        public HierarchyController(IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository,
            IRepository<EmployeeAditionalInfo> isosaemployeesRepository)
        {
            _breadcrum = new List<Breadcrumb>();


            _hierarchies = isosaemployeesOrganigramaRepository.GetAll().Select(o => new Hierarchy()
            {
                Area = o.Area,
                Departamento = o.Departamento,
                IdentPuesto = o.IdentPuesto.HasValue ? o.IdentPuesto.Value.ToString().ToLower() : "NULL",
                Puesto = o.Puesto,
                RowGuid = o.RowGuid.ToString().ToLower()
            }).ToList();

            _employeeManager = isosaemployeesRepository.SearhItemsFor(u => u.EMP_StatusCode.Equals("Active")).ToList();


        }

        public IActionResult Index()
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav("a9abae18-c608-45f2-9061-6dc035803fd7", _hierarchies));
            return View(model);
        }

        private List<ListItem> CreateNav(string rootId, List<Hierarchy> data)
        {
            List<ListItem> result = new List<ListItem>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).OrderBy(k => k.Puesto).ToList();

            var empName = _employeeManager.Where(h => h.HrowGuid.Value.ToString().ToLower().Equals(rootId));
            string empN = string.Empty;
            string empR = string.Empty;
            if (empName.Any())
            {
                empN = empName.FirstOrDefault().EMP_FirstName + " " + empName.FirstOrDefault().EMP_LastName;
                empR = empName.FirstOrDefault().EMP_EmployeeID;
            }

            ListItem m = new ListItem()
            {
                Text = char.ToUpper(element.Puesto[0]) + element.Puesto.Substring(1).ToLower(),
                I18n = element.RowGuid,
                Title = empN,
                Tags = element.Puesto.ToLower(),
                Route = Url.Action("EmployeeDetail", "Organigrama", new { employeeid = empR })
            };

            elements.ForEach(g =>
            {
                var t = CreateNav(g.RowGuid, data);
                m.Items.AddRange(t);
            });

            result.Add(m);

            return result;
        }

        public IActionResult HierarchyPartial(string rowid)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav(rowid, _hierarchies));

            var pp = _hierarchies.Where(l => l.RowGuid.Equals(rowid));

            ViewBag.RowId = pp.FirstOrDefault().IdentPuesto;

            return View(model);
        }

        public List<Breadcrumb> CreateBreadcrum()
        {
            List<Breadcrumb> _breadC = new List<Breadcrumb>();
            return _breadC;
        }



    }
}
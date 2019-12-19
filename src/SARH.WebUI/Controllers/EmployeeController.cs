using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using ISOSADataMigrationTools;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SARH.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private List<Breadcrumb> _breadcrum;
        private List<Hierarchy> _hierarchies;
        private List<EmployeeAditionalInfo> _employeeManager;

        public EmployeeController(IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository,
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

        [ActionName("HierarchyAssigned")]
        public IActionResult Index(string assignedHGuid, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav("a9abae18-c608-45f2-9061-6dc035803fd7", _hierarchies));

            ViewBag.AssignedHGuid = assignedHGuid;
            ViewBag.EmployeeID = employeeID.TrimStart(new Char[] { '0' });

            return View(model);
        }

        [HttpPost]
        public JsonResult SetNewJob(string jobGuid, string employeeGuid, [FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository) 
        {

            isosaemployeesOrganigramaRepository.CreateTableBackup("Horarios");

            var source = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.RowGuid.ToString().Equals(employeeGuid));
            var destiny = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.RowGuid.ToString().Equals(jobGuid));



            Guid nGuid = Guid.NewGuid();
            var s = source.First();
            s.RowGuid = nGuid;
            var subs = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.IdentPuesto.ToString().Equals(employeeGuid)).ToList();
            isosaemployeesOrganigramaRepository.Update(s);
            if (subs.Any()) 
            {
                subs.ForEach((y) =>
                {
                    y.IdentPuesto = nGuid;
                    isosaemployeesOrganigramaRepository.Update(y);
                });
            }
            var d = destiny.First();
            d.RowGuid = Guid.Parse(employeeGuid);
            isosaemployeesOrganigramaRepository.Update(d);
            var dest = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.IdentPuesto.ToString().Equals(jobGuid)).ToList();
            if (dest.Any())
            {
                dest.ForEach((y) =>
                {
                    y.IdentPuesto = Guid.Parse(employeeGuid);
                    isosaemployeesOrganigramaRepository.Update(y);
                });
            }

            return Json("ok");
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

        [ActionName("HierarchyAssignedSub")]
        public IActionResult HierarchyPartial(string rowid, string empRowId, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav(rowid, _hierarchies));

            var pp = _hierarchies.Where(l => l.RowGuid.Equals(rowid));

            ViewBag.AssignedHGuid = empRowId;
            ViewBag.EmployeeID = employeeID;

            return View(model);
        }


        [ActionName("HierarchyAssignedReactive")]
        public IActionResult IndexReactive(string assignedHGuid, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav("a9abae18-c608-45f2-9061-6dc035803fd7", _hierarchies));

            ViewBag.AssignedHGuid = assignedHGuid;
            ViewBag.EmployeeID = employeeID.TrimStart(new Char[] { '0' });

            return View(model);
        }

        [ActionName("HierarchyAssignedSubReactive")]
        public IActionResult HierarchyPartialReactive(string rowid, string empRowId, string employeeID)
        {
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNav(rowid, _hierarchies));

            var pp = _hierarchies.Where(l => l.RowGuid.Equals(rowid));

            ViewBag.AssignedHGuid = empRowId;
            ViewBag.EmployeeID = employeeID;

            return View(model);
        }

        [HttpPost]
        public JsonResult SetNewJobReactive(string jobGuid, string employeeId, 
            [FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository,
            [FromServices] IRepository<EmployeeAditionalInfo> employeAdditionalInfoRepository)
        {

            isosaemployeesOrganigramaRepository.CreateTableBackup("Horarios");
            var destiny = isosaemployeesOrganigramaRepository.SearhItemsFor(j => j.RowGuid.ToString().Equals(jobGuid));
            var employee = employeAdditionalInfoRepository.SearhItemsFor(f => f.EMP_EmployeeID.Equals(int.Parse(employeeId).ToString("00000")));

            bool success = true;

            try
            {
                if (employee.Any())
                {
                    var emp = employee.First();
                    emp.HrowGuid = Guid.Parse(jobGuid);
                    employeAdditionalInfoRepository.Update(emp);
                }
            }
            catch
            {
                success = false;
            }


            return Json(new { success = success, guidresponse = jobGuid });
        }



        public List<Breadcrumb> CreateBreadcrum()
        {
            List<Breadcrumb> _breadC = new List<Breadcrumb>();
            return _breadC;
        }
    }
}

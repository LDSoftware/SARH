using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using ISOSADataMigrationTools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SARH.WebUI.Factories;
using SARH.WebUI.Models;
using SARH.WebUI.Models.Catalog;
using SARH.WebUI.Models.OrganizationChart;

namespace SARH.WebUI.Controllers
{
    public class CatalogController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        #region Organigrama

        [ActionName("Organigrama")]
        public IActionResult IndexOrganigrama([FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {
            var hierarchies = isosaemployeesOrganigramaRepositor.GetAll().Select(o => new Hierarchy()
            {
                Area = o.Area,
                Departamento = o.Departamento,
                IdentPuesto = o.IdentPuesto.HasValue ? o.IdentPuesto.Value.ToString().ToLower() : "NULL",
                Centro = o.Centro,
                Puesto = o.Puesto,
                RowGuid = o.RowGuid.ToString().ToLower()
            }).ToList();
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNavArea("a9abae18-c608-45f2-9061-6dc035803fd7", hierarchies));


            return View(model);
        }

        [ActionName("OrganigramaElemento")]
        public IActionResult ViewDeparment(string rowGuid, [FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {
            var hierarchies = isosaemployeesOrganigramaRepositor.GetAll().Select(o => new Hierarchy()
            {
                Area = o.Area,
                Departamento = o.Departamento,
                Centro = o.Centro,
                IdentPuesto = o.IdentPuesto.HasValue ? o.IdentPuesto.Value.ToString().ToLower() : "NULL",
                Puesto = o.Puesto,
                RowGuid = o.RowGuid.ToString().ToLower()
            }).ToList();
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNavDepto(rowGuid, hierarchies));

            return View(model);
        }

        [ActionName("Puesto")]
        public IActionResult ViewJob(string rowGuid, [FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {


            var hierarchies = isosaemployeesOrganigramaRepositor.GetAll().Select(o => new Hierarchy()
            {
                Area = o.Area,
                Departamento = o.Departamento,
                Centro = o.Centro,
                IdentPuesto = o.IdentPuesto.HasValue ? o.IdentPuesto.Value.ToString().ToLower() : "NULL",
                Puesto = o.Puesto,
                RowGuid = o.RowGuid.ToString().ToLower()
            }).ToList();
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNavJob(rowGuid, hierarchies));


            ViewBag.RowGuid = isosaemployeesOrganigramaRepositor.SearhItemsFor(y => y.RowGuid.ToString().Equals(rowGuid)).FirstOrDefault().IdentPuesto.ToString();

            return View(model);
        }

        private List<ListItem> CreateNavArea(string rootId, List<Hierarchy> data)
        {
            List<ListItem> result = new List<ListItem>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).OrderBy(k => k.Puesto).ToList();

            ListItem m = new ListItem()
            {
                Text = char.ToUpper(element.Area[0]) + element.Area.Substring(1).ToLower(),
                I18n = element.RowGuid,
                Title = element.Centro,
                Route = element.Puesto,
                Tags = element.Area.ToLower()
            };

            elements.ForEach(g =>
            {
                var t = CreateNavArea(g.RowGuid, data);
                m.Items.AddRange(t);
            });

            result.Add(m);

            return result;
        }

        private List<ListItem> CreateNavDepto(string rootId, List<Hierarchy> data)
        {
            List<ListItem> result = new List<ListItem>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).OrderBy(k => k.Puesto).ToList();

            ListItem m = new ListItem()
            {
                Text = element.Centro.Length != 0 ? char.ToUpper(element.Centro[0]) + element.Centro.Substring(1).ToLower() : "",
                I18n = element.RowGuid,
                Title = element.Centro,
                Route = element.Puesto,
                Tags = element.Area.ToLower()
            };

            elements.ForEach(g =>
            {
                var t = CreateNavDepto(g.RowGuid, data);
                m.Items.AddRange(t);
            });

            result.Add(m);

            return result;
        }

        private List<ListItem> CreateNavJob(string rootId, List<Hierarchy> data)
        {
            List<ListItem> result = new List<ListItem>();

            var element = data.Where(p => p.RowGuid.Equals(rootId)).FirstOrDefault();
            var elements = data.Where(d => d.IdentPuesto.Equals(rootId)).OrderBy(k => k.Puesto).ToList();

            ListItem m = new ListItem()
            {
                Text = char.ToUpper(element.Puesto[0]) + element.Puesto.Substring(1).ToLower(),
                I18n = element.RowGuid,
                Title = "",
                Tags = element.Puesto.ToLower()
            };

            elements.ForEach(g =>
            {
                var t = CreateNavJob(g.RowGuid, data);
                m.Items.AddRange(t);
            });

            result.Add(m);

            return result;
        }

        #endregion

        #region Documentos

        [ActionName("Documentos")]
        public IActionResult IndexDocumentType([FromServices] IRepository<DocumentType> docTypeRepository)
        {
            List<CatalogItemModel> model = new List<CatalogItemModel>();
            model = docTypeRepository.GetAll().Select(i => new CatalogItemModel()
            { Id = i.Id, Descripcion = i.Description }).ToList();
            return View(model);
        }


        [HttpPost]
        public JsonResult SaveEditDocumentType(int Id, string Description, [FromServices] IRepository<DocumentType> docTypeRepository)
        {
            docTypeRepository.Update(new DocumentType() { Id = Id, Description = Description });
            return Json("Ok");
        }

        [HttpPost]
        public JsonResult SaveNewDocumentType(string Description, [FromServices] IRepository<DocumentType> docTypeRepository)
        {
            docTypeRepository.Create(new DocumentType() { Description = Description });
            return Json("Ok");
        }

        #endregion

        #region Equipo de Computo Asignado

        [ActionName("EquipoComputoAsignado")]
        public IActionResult HardwareAssigned([FromServices] IRepository<HardwareAssigned> hardwareAssigned)
        {
            List<CatalogItemModel> model = new List<CatalogItemModel>();
            model = hardwareAssigned.GetAll().Select(i => new CatalogItemModel()
            { Id = i.Id, Descripcion = i.Description }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveEditHardwareAssigned(int Id, string Description, [FromServices] IRepository<HardwareAssigned> hardwareAssigned)
        {
            hardwareAssigned.Update(new HardwareAssigned() { Id = Id, Description = Description });
            return Json("Ok");
        }

        [HttpPost]
        public JsonResult SaveNewHardwareAssigned(string Description, [FromServices] IRepository<HardwareAssigned> hardwareAssigned)
        {
            hardwareAssigned.Create(new HardwareAssigned() { Description = Description });
            return Json("Ok");
        }

        #endregion

        #region Equipo Seguridad Asignado

        [ActionName("EquipoSeguridad")]
        public IActionResult SafeEquipmentAssigned([FromServices] IRepository<SafeEquipmentAssigned> safeEquipmentAssigned)
        {
            List<CatalogItemModel> model = new List<CatalogItemModel>();
            model = safeEquipmentAssigned.GetAll().Select(i => new CatalogItemModel()
            { Id = i.Id, Descripcion = i.Description }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveEditSafeEquipmentAssigned(int Id, string Description, [FromServices] IRepository<SafeEquipmentAssigned> safeEquipmentAssigned)
        {
            safeEquipmentAssigned.Update(new SafeEquipmentAssigned() { Id = Id, Description = Description });
            return Json("Ok");
        }

        [HttpPost]
        public JsonResult SaveNewSafeEquipmentAssigned(string Description, [FromServices] IRepository<SafeEquipmentAssigned> safeEquipmentAssigned)
        {
            safeEquipmentAssigned.Create(new SafeEquipmentAssigned() { Description = Description });
            return Json("Ok");
        }

        #endregion

        #region Tipos de Permisos

        [ActionName("PermisosIncidencias")]
        public IActionResult PermissionType([FromServices] IRepository<PermissionType> permissionRepository)
        {

            List<CatalogItemModel> model = new List<CatalogItemModel>();
            model = permissionRepository.GetAll().Select(i => new CatalogItemModel()
            { Id = i.Id, Descripcion = i.Description }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult SaveEditPermissionType(int Id, string Description, [FromServices] IRepository<PermissionType> permissionRepository)
        {
            permissionRepository.Update(new PermissionType() { Id = Id, Description = Description });
            return Json("Ok");
        }

        [HttpPost]
        public JsonResult SaveNewSafePermissionType(string Description, [FromServices] IRepository<PermissionType> permissionRepository)
        {
            permissionRepository.Create(new PermissionType() { Description = Description });
            return Json("Ok");
        }




        #endregion

        #region Organigrama

        [HttpPost]
        public JsonResult GetElementDetail(string rowGuid, [FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {
            var detail = isosaemployeesOrganigramaRepositor.SearhItemsFor(d => d.RowGuid.ToString().Equals(rowGuid));
            string area = string.Empty;
            string centro = string.Empty;
            string depto = string.Empty;
            string puesto = string.Empty;

            if (detail.Any())
            {
                area = detail.First().Area;
                centro = detail.First().Centro;
                depto = detail.First().Departamento;
                puesto = detail.First().Puesto;
            }

            return Json(new { area = area, centro = centro, depto = depto, puesto = puesto });
        }

        [HttpPost]
        public JsonResult AddOrganigramaElement(OrganigramaItemModel model,[FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor) 
        {

            isosaemployeesOrganigramaRepositor.Create(new EmployeeOrganigrama()
            {
                Area = model.Area,
                Centro = model.Centro,
                Departamento = model.Departamento,
                Puesto = model.Puesto,
                IdentPuesto = Guid.Parse(model.IdentPuesto),
                RowGuid = Guid.NewGuid()
            });

            return Json("ok");
        }

        [HttpPost]
        public JsonResult EditOrganigramaElement(OrganigramaItemModel model, [FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {

            var row = isosaemployeesOrganigramaRepositor.SearhItemsFor(d => d.RowGuid.ToString().Equals(model.IdentPuesto));

            if (row.Any()) 
            {
                row.First().Area = model.Area;
                row.First().Centro = model.Centro;
                row.First().Departamento = model.Departamento;
                row.First().Puesto = model.Puesto;
                isosaemployeesOrganigramaRepositor.Update(row.First());
            }
         
            return Json("ok");
        }

        [HttpPost]
        public JsonResult AcceptMove(string source, string destiny, [FromServices] IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {

            var sourceelement = isosaemployeesOrganigramaRepositor.SearhItemsFor(d => d.RowGuid.ToString().Equals(source));
            var destinyelement = isosaemployeesOrganigramaRepositor.SearhItemsFor(d => d.RowGuid.ToString().Equals(destiny));

            //Guid? identpuestosource = sourceelement.FirstOrDefault().IdentPuesto;
            Guid? identpuestodestiny = destinyelement.FirstOrDefault().RowGuid;

            sourceelement.FirstOrDefault().IdentPuesto = identpuestodestiny.Value;
            isosaemployeesOrganigramaRepositor.Update(sourceelement.FirstOrDefault());
            //destinyelement.FirstOrDefault().IdentPuesto = identpuestosource.Value;
            //isosaemployeesOrganigramaRepositor.Update(destinyelement.FirstOrDefault());

            return Json("ok");
        }

        #endregion

        #region Job Assigned

        [ActionName("AsignacionPuesto")]
        public IActionResult JobTitleAssigned([FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor,[FromServices]IRepository<EmployeeAditionalInfo> empAdditionalInfo )
        {
            var isosaOrg = isosaemployeesOrganigramaRepositor.GetAll();
            var isosaEmp = empAdditionalInfo.GetAll();

            var emps = (from emp in isosaEmp
                        join org in isosaOrg on emp.HrowGuid.Value equals org.RowGuid
                        select new Hierarchy()
                        {
                            Area = org.Area,
                            Centro = org.Centro,
                            Departamento = org.Departamento,
                            Puesto = org.Puesto,
                            IdentPuesto = org.IdentPuesto.Value.ToString(),
                            RowGuid = org.RowGuid.ToString(),
                            Available = string.IsNullOrEmpty(emp.EMP_EmployeeID) ? false : true
                        }).ToList();


            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNavArea("a9abae18-c608-45f2-9061-6dc035803fd7", emps));


            return View(model);
        }

        [ActionName("AsignacionPuestoElemento")]
        public IActionResult JobTitleAssignedElement(string rowGuid, [FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor)
        {
            var hierarchies = isosaemployeesOrganigramaRepositor.GetAll().Select(o => new Hierarchy()
            {
                Area = o.Area,
                Departamento = o.Departamento,
                Centro = o.Centro,
                IdentPuesto = o.IdentPuesto.HasValue ? o.IdentPuesto.Value.ToString().ToLower() : "NULL",
                Puesto = o.Puesto,
                RowGuid = o.RowGuid.ToString().ToLower()
            }).ToList();
            List<ListItem> model = new List<ListItem>();
            model.AddRange(CreateNavDepto(rowGuid, hierarchies));

            return View(model);
        }


        #endregion



    }
}
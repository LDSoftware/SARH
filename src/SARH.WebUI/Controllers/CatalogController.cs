using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Domain.Process;
using ISOSA.SARH.Data.Repository;
using ISOSADataMigrationTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SARH.WebUI.Factories;
using SARH.WebUI.Models;
using SARH.WebUI.Models.Catalog;
using SARH.WebUI.Models.Formats;
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
        public IActionResult PermissionType([FromServices] IRepository<PermissionType> permissionRepository,
            [FromServices] IRepository<Nomipaq_nom10022> nomipaqMnemonicos)
        {

            List<CatalogItemModel> model = new List<CatalogItemModel>();
            model = permissionRepository.GetAll().Select(i => new CatalogItemModel()
            { 
                Id = i.Id, 
                Descripcion = i.Description,
                CustomValues = GetMnemonicoInfo(i.CustomValues)
            }).ToList();


            List<SelectListItem> mnemonicos = new List<SelectListItem>();

            mnemonicos.Add(new SelectListItem() 
            {
                Text = "Seleccione un Mnemonico",
                Value = "0"
            });

            mnemonicos.AddRange(nomipaqMnemonicos.GetAll().Select(o => new SelectListItem()
            {
                Text = $"{o.mnemonico}-{o.descripcion}",
                Value = o.idtipoincidencia.ToString()
            }).ToList());


            var pp = mnemonicos[0];

            var pp2 = JsonConvert.SerializeObject(pp);

            ViewBag.Mnemonicos = mnemonicos;

            return View(model);
        }

        private string GetMnemonicoInfo(string values) 
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(values)) 
            {
                var data = JsonConvert.DeserializeObject<PermissionTypeMneminicoInfoModel>(values);
                if (data != null) 
                {
                    result = data.Text;
                }
            }

            return result;
        }



        [HttpPost]
        public JsonResult SaveEditPermissionType(int Id, string Description, string Mnemonico,
            [FromServices] IRepository<PermissionType> permissionRepository,
            [FromServices] IRepository<Nomipaq_nom10022> nomipaqMnemonicos)
        {

            string jsonValues = string.Empty;
            if (!string.IsNullOrEmpty(Mnemonico)) 
            {
                var nomipaqmnemonico = nomipaqMnemonicos.GetElement(int.Parse(Mnemonico));
                if (nomipaqmnemonico != null) 
                {
                    SelectListItem itm = new SelectListItem() 
                    {
                        Text = $"{nomipaqmnemonico.mnemonico}-{nomipaqmnemonico.descripcion}",
                        Value = nomipaqmnemonico.idtipoincidencia.ToString()
                    };
                    jsonValues = JsonConvert.SerializeObject(itm);
                }
            }

            var selected = permissionRepository.GetElement(Id);

            if (selected != null) 
            {
                selected.Description = Description;
                if (!string.IsNullOrEmpty(jsonValues)) 
                {
                    selected.CustomValues = jsonValues;
                }
                permissionRepository.Update(selected);
            }

            return Json("Ok");
        }

        [HttpPost]
        public JsonResult SaveNewSafePermissionType(string Description, string Mnemonico,
            [FromServices] IRepository<PermissionType> permissionRepository,
            [FromServices] IRepository<Nomipaq_nom10022> nomipaqMnemonicos)
        {
            string jsonValues = string.Empty;
            if (!string.IsNullOrEmpty(Mnemonico))
            {
                var nomipaqmnemonico = nomipaqMnemonicos.GetElement(int.Parse(Mnemonico));
                if (nomipaqmnemonico != null)
                {
                    SelectListItem itm = new SelectListItem()
                    {
                        Text = $"{nomipaqmnemonico.mnemonico}-{nomipaqmnemonico.descripcion}",
                        Value = nomipaqmnemonico.idtipoincidencia.ToString()
                    };
                    jsonValues = JsonConvert.SerializeObject(itm);
                }
            }

            permissionRepository.Create(new PermissionType() { Description = Description, CustomValues = jsonValues });
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


        public JsonResult RemoveJobTitle(string rowGuid, [FromServices]IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepositor) 
        {

            var row = isosaemployeesOrganigramaRepositor.SearhItemsFor(g => g.RowGuid.ToString().ToLower().Equals(rowGuid.ToLower())).FirstOrDefault();
            if (row != null) 
            {
                isosaemployeesOrganigramaRepositor.Delete(row);
            }

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

        #region Format Approver

        [ActionName("Aprobadores")]
        public IActionResult ApproverIndex(
            [FromServices] IRepository<FormatApprover> formatApproversRepository,
            [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {

            FormatApproverModel model = new FormatApproverModel();
            var approvers = formatApproversRepository.GetAll();
            var organigrama = organigramaModelFactory.GetAllData().Employess;

            var emps = (from apv in approvers
                        join org in organigrama on apv.RowGuid.ToString().ToLower() equals org.RowId.ToLower()
                        select new FormatApproverItem()
                        {
                            Id = apv.Id,
                            Area = apv.Area,
                            Centro = apv.Centro,
                            Puesto = org.JobTitle,
                            RowGuid = apv.RowGuid.ToString(),
                            Approver = $"{org.Name}",
                            Order = apv.Orden
                        }).ToList();

            model.Approvers = emps;

            List<SelectListItem> areas = new List<SelectListItem>() 
            {
                new SelectListItem()
                {
                    Text = "Seleccione una opción",
                    Value = "0"
                }
            };

            areas.AddRange(organigramaModelFactory.GetAreas().Select(o => new SelectListItem()
            {
                Text = o.Descripcion,
                Value = o.Id.ToString()
            }).ToList());

            ViewBag.Areas = areas;

            return View(model);
        }

        [HttpPost]
        public JsonResult GetAreas([FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            var response = organigramaModelFactory.GetAreas();
            return Json(new { collection = response });        
        }

        [HttpPost]
        public JsonResult GetCentros(string area,[FromServices]IOrganigramaModelFactory organigramaModelFactory) 
        {
            var response = organigramaModelFactory.GetCentros(area);
            return Json(new { collection = response });
        }

        [HttpPost]
        public JsonResult GetDeptos(string area, string centros, [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            var response = organigramaModelFactory.GetDeptos(area, centros);
            return Json(new { collection = response });
        }

        [HttpPost]
        public JsonResult GetEmployeeApprover(string employeeId, string area, string centro, string depto,
        [FromServices] IRepository<FormatApprover> formatApproversRepository,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            string name = string.Empty;
            int row = 0;

            if (area.ToLower().Contains("seleccione"))
            {
                area = string.Empty;
            }


            if (centro.ToLower().Contains("seleccione"))
            {
                centro = string.Empty;
            }

            if (depto.ToLower().Contains("seleccione"))
            {
                depto = string.Empty;
            }

            if (!string.IsNullOrEmpty(employeeId))
            {
                var employeeInfo = organigramaModelFactory.GetEmployeeData(employeeId);
                if (employeeInfo != null && employeeInfo.HierarchyGuid != null)
                {
                    List<FormatApprover> empApprover = new List<FormatApprover>();

                    empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower())
                    && r.Area.Equals(area) && r.Centro.Equals(centro) 
                    &&  r.Departamento.Equals(depto)).ToList();

                    if (empApprover != null && !empApprover.Any())
                    {
                        if (string.IsNullOrEmpty(area))
                        {
                            empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower())).ToList();
                        }

                        if (string.IsNullOrEmpty(centro) && !string.IsNullOrEmpty(area))
                        {
                            empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower())
                            && r.Area.ToLower().Equals(area.ToLower())).ToList();
                        }

                        if (string.IsNullOrEmpty(depto) && !string.IsNullOrEmpty(centro))
                        {
                            empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower())
                            && r.Area.ToLower().Equals(area.ToLower()) && r.Centro.ToLower().Equals(centro.ToLower())).ToList();
                        }

                        if (!string.IsNullOrEmpty(centro) && !string.IsNullOrEmpty(depto) && !string.IsNullOrEmpty(area))
                        {
                            empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower())
                            && r.Area.ToLower().Equals(area.ToLower()) && r.Centro.ToLower().Equals(centro.ToLower()) && r.Departamento.ToLower().Equals(depto.ToLower())).ToList();
                        }

                        if (empApprover != null && empApprover.Any())
                        {
                            name = "El empleado ya está asignado para aprobación";

                        }
                        else
                        {
                            name = $"{employeeInfo.GeneralInfo.FirstName} {employeeInfo.GeneralInfo.LastName} {employeeInfo.GeneralInfo.LastName2}";
                            row = 1;
                        }
                    }
                    else
                    {
                        name = "El empleado ya está asignado para aprobación";
                    }
                }
                else
                {
                    name = "No se encontrarón resultados";
                }
            }
            else
            {
                name = "No se encontrarón resultados";
            }

            return Json(new { id = employeeId, value = name, rows = row });
        }


        [HttpPost]
        public JsonResult GetEmployeeApproverPP(string employeeId,
        [FromServices] IRepository<FormatApprover> formatApproversRepository,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            string name = string.Empty;
            int row = 0;

            if (!string.IsNullOrEmpty(employeeId))
            {
                var employeeInfo = organigramaModelFactory.GetEmployeeData(employeeId);
                if (employeeInfo != null && employeeInfo.HierarchyGuid != null)
                {
                    List<FormatApprover> empApprover = new List<FormatApprover>();

                    empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower())).ToList();

                    if (empApprover != null && !empApprover.Any())
                    {
                        name = $"{employeeInfo.GeneralInfo.FirstName} {employeeInfo.GeneralInfo.LastName} {employeeInfo.GeneralInfo.LastName2}";
                        row = 1;
                    }
                    else
                    {
                        name = "El empleado ya está asignado para aprobación";
                    }
                }
                else
                {
                    name = "No se encontrarón resultados";
                }
            }
            else
            {
                name = "No se encontrarón resultados";
            }

            return Json(new { id = employeeId, value = name, rows = row });
        }


        [HttpPost]
        public JsonResult GetEmployeeForApprover(string employeeId,
        [FromServices] IRepository<FormatApprover> formatApproversRepository,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            string name = string.Empty;
            int row = 0;

            if (!string.IsNullOrEmpty(employeeId))
            {
                var employeeInfo = organigramaModelFactory.GetEmployeeData(employeeId);
                name = $"{employeeInfo.GeneralInfo.FirstName} {employeeInfo.GeneralInfo.LastName} {employeeInfo.GeneralInfo.LastName2}";
                row = 1;
            }
            else
            {
                name = "No se encontrarón resultados";
            }

            return Json(new { id = employeeId, value = name, rows = row });
        }

        [HttpPost]
        public JsonResult SaveApproverPPA(string approver, string[] employees,
            [FromServices] IRepository<FormatApprover> formatApproversRepository,
            [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {

            var emp = organigramaModelFactory.GetEmployeeData(approver);

            string values = JsonConvert.SerializeObject(employees.ToList());

            var formatapprover = new FormatApprover()
            {
                Area = string.Empty,
                Centro = string.Empty,
                Departamento = string.Empty,
                Orden = 1,
                Puesto = emp.GeneralInfo.JobTitle,
                RowGuid = Guid.Parse(emp.HierarchyGuid),
                ApproverListEmployees = values
            };

            formatApproversRepository.Create(formatapprover);

            return Json("ok");
        }


        [HttpPost]
        public JsonResult SaveApprover(string area, string centro, string depto, string approver,
            [FromServices] IRepository<FormatApprover> formatApproversRepository,
            [FromServices]IOrganigramaModelFactory organigramaModelFactory) 
        {
            int order = 1;

            if (area.ToLower().Contains("seleccione"))
            {
                area = string.Empty;
            }

            if (centro.ToLower().Contains("seleccione")) 
            {
                centro = string.Empty;
            }

            if (depto.ToLower().Contains("seleccione")) 
            {
                depto = string.Empty;
            }

            var aprovOrder = formatApproversRepository.SearhItemsFor(f => f.Area.Equals(area) && f.Centro.Equals(centro) && f.Departamento.Equals(depto));

            if (aprovOrder.Any()) 
            {
                order = aprovOrder.Count() + 1;
            }

            var emp = organigramaModelFactory.GetEmployeeData(approver);

            var formatapprover = new FormatApprover()
            {
                Area = area,
                Centro = centro,
                Departamento = depto,
                Orden = order,
                Puesto = emp.GeneralInfo.JobTitle,
                RowGuid = Guid.Parse(emp.HierarchyGuid)
            };

            formatApproversRepository.Create(formatapprover);



            return Json("ok");
        }


        [HttpPost]
        public JsonResult GetApprovers(int id,
        [FromServices] IRepository<FormatApprover> formatApproversRepository,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory) 
        {
            var organigrama = organigramaModelFactory.GetAllData().Employess;
            var row = formatApproversRepository.GetElement(id);
            var rows = formatApproversRepository.SearhItemsFor(u => u.Area.Equals(row.Area) && u.Centro.Equals(row.Centro) && u.Departamento.Equals(row.Departamento));
            FormatApproverModel model = new FormatApproverModel();

            if (rows.Any()) 
            {
                var emps = (from apv in rows
                            join org in organigrama on apv.RowGuid.ToString().ToLower() equals org.RowId.ToLower()
                            select new FormatApproverItem()
                            {
                                Id = apv.Id,
                                Area = apv.Area,
                                Centro = apv.Centro,
                                Puesto = org.JobTitle,
                                RowGuid = apv.RowGuid.ToString(),
                                Approver = $"{org.Name}",
                                Order = apv.Orden
                            }).ToList();

                model.Approvers = emps;
            }


            return Json(new { area = row.Area, centro = row.Centro, depto = row.Departamento, collection = model.Approvers });
        }

        [HttpPost]
        public JsonResult DeleteApprovers(string[] items,
        [FromServices] IRepository<FormatApprover> formatApproversRepository)
        {

            items.ToList().ForEach(d => 
            {
                var code = d.Split("-");
                int codeint = int.Parse(code[1].ToString());
                var row = formatApproversRepository.GetElement(codeint);
                if (row != null)
                {
                    formatApproversRepository.Delete(row);
                }

            });

            return Json("Ok");
        }


        #endregion

        #region Notification

        public IActionResult NotifyIndex([FromServices] IRepository<EmployeeNotify> employeeNotifyRepository,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            EmployeeNotificationModel model = new EmployeeNotificationModel();
            var notifiers = employeeNotifyRepository.GetAll();
            var organigrama = organigramaModelFactory.GetAllData().Employess;

            var emps = (from apv in notifiers
                        join org in organigrama on apv.RowGuid.ToString().ToLower() equals org.RowId.ToLower()
                        select new EmployeeNotificationItem()
                        {
                            Id = apv.Id,
                            Area = apv.Area,
                            Centro = apv.Centro,
                            Puesto = org.JobTitle,
                            RowGuid = apv.RowGuid.ToString(),
                            Notifier = $"{org.Name}",
                            Email = org.UserName
                        }).ToList();

            model.Notifications = emps;

            List<SelectListItem> areas = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Seleccione una opción",
                    Value = "0"
                }
            };

            areas.AddRange(organigramaModelFactory.GetAreas().Select(o => new SelectListItem()
            {
                Text = o.Descripcion,
                Value = o.Id.ToString()
            }).ToList());

            ViewBag.Areas = areas;

            return View(model);
        }


        [HttpPost]
        public JsonResult GetEmployeeNotifier(string employeeId,
        [FromServices] IRepository<EmployeeNotify> formatApproversRepository,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            string name = string.Empty;
            int row = 0;

            if (!string.IsNullOrEmpty(employeeId))
            {
                var employeeInfo = organigramaModelFactory.GetEmployeeData(employeeId);
                if (employeeInfo != null && employeeInfo.HierarchyGuid != null)
                {
                    var empApprover = formatApproversRepository.SearhItemsFor(r => r.RowGuid.ToString().ToLower().Equals(employeeInfo.HierarchyGuid.ToLower()));
                    if (empApprover != null && empApprover.Any())
                    {
                        name = "El empleado ya está asignado para notificaciones";

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(employeeInfo.GeneralInfo.Email))
                        {
                            name = $"{employeeInfo.GeneralInfo.FirstName} {employeeInfo.GeneralInfo.LastName} - {employeeInfo.GeneralInfo.Email}";
                            row = 1;
                        }
                        else
                        {
                            name = "El empleado seleccionado no tiene cuenta de correo eléctronico";
                            row = 0;
                        }
                    }
                }
                else
                {
                    name = "No se encontrarón resultados";
                }
            }
            else
            {
                name = "No se encontrarón resultados";
            }

            return Json(new { id = employeeId, value = name, rows = row });
        }

        [HttpPost]
        public JsonResult SaveNotifier(string area, string centro, string depto, string approver,
            [FromServices] IRepository<EmployeeNotify> employeeNotify,
            [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            int order = 1;
            var aprovOrder = employeeNotify.SearhItemsFor(f => f.Area.Equals(area) && f.Centro.Equals(centro) && f.Departamento.Equals(depto));
            if (aprovOrder.Any())
            {
                order = aprovOrder.Count() + 1;
            }

            var emp = organigramaModelFactory.GetEmployeeData(approver);

            var empNotify = new EmployeeNotify()
            {
                Area = area,
                Centro = centro,
                Departamento = depto,
                Orden = order,
                Puesto = emp.GeneralInfo.JobTitle,
                RowGuid = Guid.Parse(emp.HierarchyGuid)
            };

            employeeNotify.Create(empNotify);



            return Json("ok");
        }


        [HttpPost]
        public JsonResult GetNotifiers(int id,
        [FromServices] IRepository<EmployeeNotify> employeeNotify,
        [FromServices]IOrganigramaModelFactory organigramaModelFactory)
        {
            var organigrama = organigramaModelFactory.GetAllData().Employess;
            var row = employeeNotify.GetElement(id);
            var rows = employeeNotify.SearhItemsFor(u => u.Area.Equals(row.Area) && u.Centro.Equals(row.Centro) && u.Departamento.Equals(row.Departamento));
            EmployeeNotificationModel model = new EmployeeNotificationModel();

            if (rows.Any())
            {
                var emps = (from apv in rows
                            join org in organigrama on apv.RowGuid.ToString().ToLower() equals org.RowId.ToLower()
                            select new EmployeeNotificationItem()
                            {
                                Id = apv.Id,
                                Area = apv.Area,
                                Centro = apv.Centro,
                                Puesto = org.JobTitle,
                                RowGuid = apv.RowGuid.ToString(),
                                Notifier = $"{org.Name}",
                                Email = org.UserName
                            }).ToList();

                model.Notifications = emps;
            }


            return Json(new { area = row.Area, centro = row.Centro, depto = row.Departamento, collection = model.Notifications });
        }

        [HttpPost]
        public JsonResult DeleteNotifier(string[] items,
        [FromServices] IRepository<EmployeeNotify> employeeNotify)
        {

            items.ToList().ForEach(d =>
            {
                var code = d.Split("-");
                int codeint = int.Parse(code[1].ToString());
                var row = employeeNotify.GetElement(codeint);
                if (row != null)
                {
                    employeeNotify.Delete(row);
                }

            });

            return Json("Ok");
        }

        #endregion

    }
}
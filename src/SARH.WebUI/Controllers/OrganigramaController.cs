using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Assignation;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SARH.Core.Configuration;
using SARH.Core.PdfCreator;
using SARH.Core.PdfCreator.Interface;
using SARH.WebUI.Factories;
using SARH.WebUI.Models.Employee;
using SARH.WebUI.Models.Formats;
using SARH.WebUI.Models.Organigrama;
using SARH.WebUI.Models.Organigrama.New;

namespace SARH.WebUI.Controllers
{
    public class OrganigramaController : Controller
    {

        private readonly IOrganigramaModelFactory _organigramaModelFactory;
        private readonly IHostingEnvironment _host;

        #region Organigrana

        public OrganigramaController(IOrganigramaModelFactory organigramaModelFactory, IHostingEnvironment host, [FromServices]IRepository<EmployeeScheduleAssigned> repoSchedule)
        {
            this._organigramaModelFactory = organigramaModelFactory;
            this._host = host;
        }

        [ActionName("Organigrama")]
        public IActionResult Index(string organigramaitem)
        {

            var model = this._organigramaModelFactory.GetAllData();

            return View(model);
        }

        #endregion

        #region Organigrama Employee Detail

        [ActionName("EmployeeDetail")]
        public IActionResult Detail(string employeeid,
            [FromServices] IRepository<HardwareAssigned> hardwareAssigned,
            [FromServices] IRepository<DocumentType> documentType,
            [FromServices] IRepository<SafeEquipmentAssigned> safeEquipmentAssigned,
            [FromServices] IRepository<EmployeeObjectAsignation> employeeHardwareAssigned,
            [FromServices] IRepository<PermissionType> permissionType,
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository
            )
        {

            var docType = documentType.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });
            ViewBag.hardwareCatalog = hardwareAssigned.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });
            ViewBag.documentType = docType;
            ViewBag.safeequipment = safeEquipmentAssigned.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });
            ViewBag.permissionType = permissionType.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });


            var model = this._organigramaModelFactory.GetEmployeeData(employeeid);

            var assignedData = employeeHardwareAssigned.SearhItemsFor(p => p.EmployeeId.Equals(model.GeneralInfo.Id) && p.AssignationType.Equals(1));
            if (assignedData.Any())
            {
                var list = JsonConvert.DeserializeObject<List<HardwareAssignedModel>>(assignedData.FirstOrDefault().AssignationDetail);
                model.HardwareAssined.AddRange(list);
            }

            var assignedDataSafe = employeeHardwareAssigned.SearhItemsFor(p => p.EmployeeId.Equals(model.GeneralInfo.Id) && p.AssignationType.Equals(2));
            if (assignedDataSafe.Any())
            {
                var list = JsonConvert.DeserializeObject<List<SafeEquimentAssignedModel>>(assignedDataSafe.FirstOrDefault().AssignationDetail);
                model.SecurityEquipmentAssigned.AddRange(list);
            }


            var personalDocs = personalDocumentRepository.SearhItemsFor(p => p.EmployeeID.Equals(model.GeneralInfo.Id));
            if (personalDocs.Any())
            {
                model.Documents.AddRange(personalDocs.Select(y => new EmployeeDetailDocuments()
                {
                    Id = y.Id,
                    EmployeeID = y.EmployeeID,
                    DocumentPathInfo = Path.GetFileName(y.DocumentPathInfo),
                    DocumentType = docType.Where(d => d.Value.Equals(y.DocumentType.ToString())).FirstOrDefault().Text,
                    IsValid = y.IsValid ? "Si" : "No",
                    Checked = y.Checked ? "Si" : "No"
                }));
            }

            return View(model);
        }

        #endregion

        #region Hardware Assigned

        [HttpPost]
        public JsonResult SaveHardwareAssigned(string EmployeeId, HardwareAssignedModel model, [FromServices] IRepository<EmployeeObjectAsignation> repository, [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
        {
            List<HardwareAssignedModel> list = new List<HardwareAssignedModel>();


            string pathPDF = configManager.AssignedHardwarePath.Replace("|EmpNumber|", EmployeeId);

            if (repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(1)).Any())
            {
                var jsonData = repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(1)).FirstOrDefault();
                list.AddRange(JsonConvert.DeserializeObject<List<HardwareAssignedModel>>(jsonData.AssignationDetail));

                int idNext = 1;
                if (list.Any())
                {
                    idNext += list.Max(p => p.Id);
                }

                list.Add(new HardwareAssignedModel()
                {
                    Id = idNext,
                    Marca = string.IsNullOrEmpty(model.Marca) ? "" : model.Marca,
                    Serie = string.IsNullOrEmpty(model.Serie) ? "" : model.Serie,
                    Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description,
                    IdHardwareDevice = model.IdHardwareDevice,
                    HadwareName = model.HadwareName,
                    AssignationDate = DateTime.Now.ToShortDateString()                   
                });

                jsonData.AssignationDetail = JsonConvert.SerializeObject(list);

                repository.Update(jsonData);
            }
            else
            {
                list.Add(new HardwareAssignedModel()
                {
                    Id = 1,
                    Marca = string.IsNullOrEmpty(model.Marca) ? "" : model.Marca,
                    Serie = string.IsNullOrEmpty(model.Serie) ? "" : model.Serie,
                    Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description,
                    IdHardwareDevice = model.IdHardwareDevice,
                    HadwareName = model.HadwareName,
                    AssignationDate = DateTime.Now.ToShortDateString()
                });
                repository.Create(new EmployeeObjectAsignation()
                {
                    EmployeeId = EmployeeId,
                    AssignationType = 1,
                    AssignationDetail = JsonConvert.SerializeObject(list),
                    PathPDF = pathPDF,
                    DocumentId = string.Empty
                });
            }

            return Json(list);
        }

        public JsonResult UpdateHardwareAssigned(string EmployeeId, int[] assignedId, [FromServices] IRepository<EmployeeObjectAsignation> repository, [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
        {
            List<HardwareAssignedModel> list = new List<HardwareAssignedModel>();


            string pathPDF = configManager.AssignedHardwarePath.Replace("|EmpNumber|", EmployeeId);

            if (repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(1)).Any())
            {
                var jsonData = repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(1)).FirstOrDefault();
                list.AddRange(JsonConvert.DeserializeObject<List<HardwareAssignedModel>>(jsonData.AssignationDetail));


                assignedId.ToList().ForEach(y => 
                {
                    var element = list.Where(o => o.Id.Equals(y));
                    if (element.Any())
                    {
                        list.Remove(element.FirstOrDefault());
                    }
                });

                jsonData.PathPDF = pathPDF;
                jsonData.AssignationDetail = JsonConvert.SerializeObject(list);

                repository.Update(jsonData);
            }

            return Json(list);
        }

        public FileResult PrintHardwareAssigned(string EmployeeId, [FromServices] IRepository<EmployeeObjectAsignation> repository, [FromServices] IOrganigramaModelFactory organigramaModelFactory, [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
        {
            List<HardwareAssignedModel> list = new List<HardwareAssignedModel>();

            string fileName = "";

            if (repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(1)).Any())
            {
                var jsonData = repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(1)).FirstOrDefault();

                IConfigPdf config = new ConfigPdf()
                {
                    FontPathPdf = configManager.FontPathPdf,
                    ImgPathPdf = configManager.ImgPathPdf,
                    FontPathBarCode = configManager.FontPathBarCode
                };
                PdfManager manager = new PdfManager(config);


                if (!Directory.Exists(jsonData.PathPDF))
                {
                    Directory.CreateDirectory(jsonData.PathPDF);
                }

                fileName = $"{jsonData.PathPDF}{EmployeeId}-F002-{DateTime.Now.ToShortDateString().Replace("/", string.Empty)}-001.pdf";


                var empInfo = organigramaModelFactory.GetEmployeeData(EmployeeId.TrimStart('0'));

                var detail = JsonConvert.DeserializeObject<List<HardwareAssignedModel>>(jsonData.AssignationDetail);

                List<string> _list = new List<string>();
                StringBuilder strb = new StringBuilder();
                detail.ForEach((det) =>
                {
                    var _tt = "\t\t\t";
                    var _ii = $"Carateristicas \t\t Marca:{det.Marca}  \t\t Serie:{det.Serie}";
                    var _rr = $"\t\t Fecha de entrega {det.AssignationDate}";
                    _list.Add($"{det.HadwareName} {_tt} {_ii} {_rr}");
                    strb.Append(det.Description);
                    strb.AppendLine();
                });

                jsonData.DocumentId = $"{EmployeeId}-F002-{DateTime.Now.ToShortDateString().Replace("/", string.Empty)}-001";
                repository.Update(jsonData);

                manager.CreateAssigedHardwareFormat(new Core.PdfCreator.FormatData.DocumentInfoPdfData()
                {
                    EmployeeNumber = EmployeeId,
                    EmployeeName = $"{empInfo.GeneralInfo.FirstName} {empInfo.GeneralInfo.LastName}",
                    JobTitle = empInfo.GeneralInfo.JobTitle,
                    Area = empInfo.GeneralInfo.Area,
                    DocumentDetailInfo = _list,
                    Comments = strb.ToString(),
                    TitleDocumento = "Equipo en Resguardo",
                    FormatId = "F002",
                    EmployeeInfoTitle = "DATOS PERSONALES",
                    DetailDocument = "TECNOLOGíA DE LA INFORMACIÓN",
                    DocumentObservationsTitle = "OBSERVACIONES",
                    SingEmployeeTitle = "NOMBRE Y FIRMA DE RECIBIDO",
                    IdDocument = $"{EmployeeId}-F002-{DateTime.Now.ToShortDateString().Replace("/", string.Empty)}-001"
                }, fileName);
            }

            byte[] FileBytes = System.IO.File.ReadAllBytes(fileName);
            return File(FileBytes, "application/pdf");
        }

        #endregion

        #region Security Equipment Asigned

        [HttpPost]
        public JsonResult SaveSecurityEquipmentAssigned(string EmployeeId, SafeEquimentAssignedModel model, [FromServices] IRepository<EmployeeObjectAsignation> repository, [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
        {
            List<SafeEquimentAssignedModel> list = new List<SafeEquimentAssignedModel>();


            string pathPDF = configManager.AssignedEquimentPath.Replace("|EmpNumber|", EmployeeId);

            if (repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(2)).Any())
            {
                var jsonData = repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(2)).FirstOrDefault();
                list.AddRange(JsonConvert.DeserializeObject<List<SafeEquimentAssignedModel>>(jsonData.AssignationDetail));

                int idNext = 1;
                if (list.Any())
                {
                    idNext += list.Max(p => p.Id);
                }

                list.Add(new SafeEquimentAssignedModel()
                {
                    Id = idNext,
                    Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description,
                    IdSecurityEquiment = model.IdSecurityEquiment,
                    SecurityEquimentName = model.SecurityEquimentName,
                    AssignationDate = DateTime.Now.ToShortDateString()
                });

                jsonData.AssignationDetail = JsonConvert.SerializeObject(list);

                repository.Update(jsonData);
            }
            else
            {
                list.Add(new SafeEquimentAssignedModel()
                {
                    Id = 1,
                    Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description,
                    IdSecurityEquiment = model.IdSecurityEquiment,
                    SecurityEquimentName = model.SecurityEquimentName,
                    AssignationDate = DateTime.Now.ToShortDateString()
                });
                repository.Create(new EmployeeObjectAsignation()
                {
                    EmployeeId = EmployeeId,
                    AssignationType = 2,
                    AssignationDetail = JsonConvert.SerializeObject(list),
                    PathPDF = pathPDF,
                    DocumentId = string.Empty
                });
            }

            return Json(list);
        }

        [HttpPost]
        public JsonResult UpdateSecurityEquipmentAssigned(string EmployeeId, int[] assignedId, [FromServices] IRepository<EmployeeObjectAsignation> repository, [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
        {
            List<SafeEquimentAssignedModel> list = new List<SafeEquimentAssignedModel>();


            string pathPDF = configManager.AssignedHardwarePath.Replace("|EmpNumber|", EmployeeId);

            if (repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(2)).Any())
            {
                var jsonData = repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(2)).FirstOrDefault();
                list.AddRange(JsonConvert.DeserializeObject<List<SafeEquimentAssignedModel>>(jsonData.AssignationDetail));


                assignedId.ToList().ForEach(y =>
                {
                    var element = list.Where(o => o.Id.Equals(y));
                    if (element.Any())
                    {
                        list.Remove(element.FirstOrDefault());
                    }
                });

                jsonData.PathPDF = pathPDF;
                jsonData.AssignationDetail = JsonConvert.SerializeObject(list);

                repository.Update(jsonData);
            }

            return Json(list);
        }


        public FileResult PrintSecurityEquipmentAssigned(string EmployeeId, [FromServices] IRepository<EmployeeObjectAsignation> repository, [FromServices] IOrganigramaModelFactory organigramaModelFactory, [FromServices] SARH.Core.Configuration.IConfigurationManager configManager)
        {
            List<HardwareAssignedModel> list = new List<HardwareAssignedModel>();

            string fileName = "";

            if (repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(2)).Any())
            {
                var jsonData = repository.SearhItemsFor(p => p.EmployeeId.Equals(EmployeeId) && p.AssignationType.Equals(2)).FirstOrDefault();

                IConfigPdf config = new ConfigPdf()
                {
                    FontPathPdf = configManager.FontPathPdf,
                    ImgPathPdf = configManager.ImgPathPdf,
                    FontPathBarCode = configManager.FontPathBarCode
                };
                PdfManager manager = new PdfManager(config);


                if (!Directory.Exists(jsonData.PathPDF))
                {
                    Directory.CreateDirectory(jsonData.PathPDF);
                }

                fileName = $"{jsonData.PathPDF}{EmployeeId}-F003-{DateTime.Now.ToShortDateString().Replace("/", string.Empty)}-001.pdf";


                var empInfo = organigramaModelFactory.GetEmployeeData(EmployeeId.TrimStart('0'));

                var detail = JsonConvert.DeserializeObject<List<SafeEquimentAssignedModel>>(jsonData.AssignationDetail);

                List<string> _list = new List<string>();
                StringBuilder strb = new StringBuilder();
                detail.ForEach((det) =>
                {
                    var _tt = "\t\t\t";
                    var _ii = $"Carateristicas \t\t {det.Description} ";
                    var _rr = $"\t\t Fecha de entrega {det.AssignationDate}";
                    _list.Add($"{det.Description} {_tt} {_ii} {_rr}");
                    strb.Append(det.Description);
                    strb.AppendLine();
                });

                jsonData.DocumentId = $"{EmployeeId}-F003-{DateTime.Now.ToShortDateString().Replace("/", string.Empty)}-001";
                repository.Update(jsonData);

                manager.CreateAssigedHardwareFormat(new Core.PdfCreator.FormatData.DocumentInfoPdfData()
                {
                    EmployeeNumber = EmployeeId,
                    EmployeeName = $"{empInfo.GeneralInfo.FirstName} {empInfo.GeneralInfo.LastName}",
                    JobTitle = empInfo.GeneralInfo.JobTitle,
                    Area = empInfo.GeneralInfo.Area,
                    DocumentDetailInfo = _list,
                    Comments = strb.ToString(),
                    TitleDocumento = "Entrega Equipo de protección",
                    FormatId = "F003",
                    EmployeeInfoTitle = "DATOS PERSONALES",
                    DetailDocument = "EQUIPO DE PROTECCIÓN",
                    DocumentObservationsTitle = "OBSERVACIONES",
                    SingEmployeeTitle = "NOMBRE Y FIRMA DE RECIBIDO",
                    IdDocument = $"{EmployeeId}-F003-{DateTime.Now.ToShortDateString().Replace("/", string.Empty)}-001"
                }, fileName);
            }

            byte[] FileBytes = System.IO.File.ReadAllBytes(fileName);
            return File(FileBytes, "application/pdf");
        }

        #endregion

        #region Assigned Documents

        [HttpPost]
        public JsonResult SaveDocument(EmployeeDocumentModel model,
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository,
            [FromServices]SARH.Core.Configuration.IConfigurationManager configurationManager,
            [FromServices]IRepository<DocumentType> documentTypeRepository)
        {
            string result = "fail";
            string message = "Ya existe este documento";


            if (!personalDocumentRepository.SearhItemsFor(h => h.EmployeeID.Equals(model.EmployeeId) && h.DocumentType.Equals(model.DocumentType)).Any())
            {
                string path = configurationManager.EmployeeDocumentsPath.Replace("|EmpNumber|", model.EmployeeId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var documentType = documentTypeRepository.GetElement(model.DocumentType).Description.Replace(" ", string.Empty);
                System.IO.File.Move(model.DocumentPath, $"{path}{ documentType}.pdf");
                personalDocumentRepository.Create(new PersonalDocument()
                {
                    Checked = false,
                    IsValid = false,
                    DocumentPathInfo = $"{path}{ documentType}.pdf",
                    DocumentType = model.DocumentType,
                    EmployeeID = model.EmployeeId
                });
                result = "success";
            }

            return Json(new { employeeid = model.EmployeeId.TrimStart(new Char[] { '0' }), result = result, message = message });
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {

            string filePath = Path.GetTempFileName();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                }
            }

            return Json(new { filetemp = filePath });
        }

        [HttpPost]
        public JsonResult GetDocumentoToValidate(string[] inputDocs, 
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository) 
        {

            List<PersonalDocument> list = new List<PersonalDocument>();

           
            inputDocs.ToList().ForEach((l) => 
            {
                var doc = personalDocumentRepository.SearhItemsFor(d => d.Id.Equals(int.Parse(l))).FirstOrDefault();
                list.Add(doc);
            });

            return Json(list.Select(g => new { id = $"docvalidate-{g.Id}", document = Path.GetFileName(g.DocumentPathInfo) }));
        }

        public FileResult ViewAssignedDocument(string filepath, 
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository) 
        {
            string[] docPath = filepath.Split("-");
            var element = personalDocumentRepository.GetElement(int.Parse(docPath[1]));
            byte[] FileBytes = System.IO.File.ReadAllBytes(element.DocumentPathInfo);
            return File(FileBytes, "application/pdf");
        }


        public JsonResult updateDocumentValidation(string[] inputDocs,
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository)
        {
            string idEmployee = string.Empty;
            inputDocs.ToList().ForEach((l) =>
            {
                var doc = personalDocumentRepository.SearhItemsFor(d => d.Id.Equals(int.Parse(l))).FirstOrDefault();
                idEmployee = doc.EmployeeID;
                doc.IsValid = true;
                personalDocumentRepository.Update(doc);
            });

            return Json(new { employeeid = idEmployee.TrimStart(new Char[] { '0' }) });
        }


        [HttpPost]
        public JsonResult UpdateDocument(EmployeeDocumentModel model,
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository,
            [FromServices]SARH.Core.Configuration.IConfigurationManager configurationManager,
            [FromServices]IRepository<DocumentType> documentTypeRepository)
        {
            string result = string.Empty;
            string message = string.Empty;

            var element = personalDocumentRepository.SearhItemsFor(h => h.EmployeeID.Equals(model.EmployeeId) && h.DocumentType.Equals(model.DocumentType));
            if (element.Any())
            {
                var row = element.FirstOrDefault();
                if (System.IO.File.Exists(row.DocumentPathInfo))
                {
                    System.IO.File.Delete(row.DocumentPathInfo);
                }
                string path = configurationManager.EmployeeDocumentsPath.Replace("|EmpNumber|", model.EmployeeId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var documentType = documentTypeRepository.GetElement(model.DocumentType).Description.Replace(" ", string.Empty);
                System.IO.File.Move(model.DocumentPath, $"{path}{ documentType}.pdf");
                row.IsValid = false;
                personalDocumentRepository.Update(row);
                result = "success";
            }
            else 
            {
                result = "fail";
                message = "No se puede actualizar el documento no ha sido cargado previamente";
            }

            return Json(new { employeeid = model.EmployeeId.TrimStart(new Char[] { '0' }), message = message, result = result });
        }


        public FileResult PrintDocument(int idDocument,
            [FromServices]IRepository<PersonalDocument> personalDocumentRepository)
        {
            var element = personalDocumentRepository.SearhItemsFor(h => h.Id.Equals(idDocument));
            string documentPath = string.Empty;
            if (element.Any())
            {
                documentPath = element.FirstOrDefault().DocumentPathInfo;
            }

            byte[] FileBytes = System.IO.File.ReadAllBytes(documentPath);
            return File(FileBytes, "application/pdf");
        }


        #endregion

        #region Formats

        public JsonResult SearchBy(string inputtext, string typesearch, [FromServices] IOrganigramaModelFactory _organigramaModelFactory)
        {
            string value = string.Empty;
            string id = string.Empty;
            int row = 0;

            var employee = _organigramaModelFactory.GetEmployeeData(inputtext.TrimStart(new Char[] { '0' }));
            if (employee.Area != null)
            {
                if (employee.RowId != null)
                {
                    id = $"{typesearch}|{employee.GeneralInfo.Id}";
                    value = $"{inputtext}-{ employee.GeneralInfo.FirstName} {employee.GeneralInfo.LastName}";
                    row = 1;
                }
            }

            return Json(new { id = id, value = value, rows = row });
        }

        public JsonResult SaveFormat(FormatInputModel format, 
            [FromServices] IRepository<EmployeeFormat> formarRepository, 
            [FromServices] IRepository<EmployeeScheduleAssigned> scheduleAssignedRepository,
            [FromServices] IRepository<EmployeeScheduleAssignedTemp> scheduleAssignedTempRepository)
        {

            formarRepository.Create(new EmployeeFormat()
            {
                Comments = format.Comments,
                EmployeeId = format.EmployeeId,
                EmployeeSubstitute = format.EmployeeSubId,
                EndDate = DateTime.Parse(format.EndDate),
                PermissionType = format.PermissionType,
                StartDate = DateTime.Parse(format.StartDate)
            });

            var schedule = scheduleAssignedRepository.SearhItemsFor(s => s.EmployeeId.Equals(format.EmployeeId)).FirstOrDefault();
            if (schedule != null) 
            {
                var scheduleTemp = scheduleAssignedTempRepository.SearhItemsFor(s => s.EmployeeId.Equals(format.EmployeeId)).FirstOrDefault();
                if (scheduleTemp != null) 
                {
                    if (!format.EmployeeSubId.Equals(0))
                    {

                    }
                }





            }






            return Json("Ok");
        }


        #endregion

        #region Organigrama Employee New

        [ActionName("EmployeeNew")]
        public IActionResult NewEmployee(
            [FromServices] IRepository<HardwareAssigned> hardwareAssigned,
            [FromServices] IRepository<DocumentType> documentType,
            [FromServices] IRepository<SafeEquipmentAssigned> safeEquipmentAssigned,
            [FromServices] IRepository<EmployeeObjectAsignation> employeeHardwareAssigned,
            [FromServices] IRepository<PermissionType> permissionType,
            [FromServices] IRepository<PersonalDocument> personalDocumentRepository,
            [FromServices] IRepository<Schedule> repository)
        {
            var docType = documentType.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });
            ViewBag.hardwareCatalog = hardwareAssigned.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });
            ViewBag.documentType = docType;
            ViewBag.safeequipment = safeEquipmentAssigned.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });
            ViewBag.permissionType = permissionType.GetAll().Select(k => new SelectListItem() { Text = k.Description, Value = k.Id.ToString() });

            var dropdownSchedule = repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(1) && !u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            });

            var dropdownScheduleMeal = repository.GetAll().Where(u => u.Enabled.Equals(true) && u.TypeSchedule.Equals(2) && !u.EffectiveTimeStart.HasValue).Select(o => new SelectListItem()
            {
                Value = o.Id.ToString(),
                Text = $"({o.StartHour} - {o.EndHour})"
            });

            ViewBag.dropdownSchedule = dropdownSchedule;
            ViewBag.dropdownScheduleMeal = dropdownScheduleMeal;


            string value = _organigramaModelFactory.GetNextIdEmployeeRepository("1");

            var lastEmployee = _organigramaModelFactory.GetAllData().Employess.Max(u => int.Parse(u.Id));
            OrganigramaEmployeeDetailModel model = new OrganigramaEmployeeDetailModel();
            ViewBag.NextEmployeeId = lastEmployee + 1;
            model.GeneralInfo.HireDate = DateTime.Today.ToShortDateString();
            model.GeneralInfo.Id = value;

            return View(model);
        }

        [HttpPost]
        public JsonResult SaveNewEmployee(EmployeeDetailGeneralInfoNew model,
            [FromServices] IRepository<Nomipaq_nom10001> nomipaqRepo,
            [FromServices] IRepository<EmployeeAditionalInfo> isosaRepo,
            [FromServices] IConfigurationManager configManager)
        {
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(model.Picture))
            {
                var imgb64 = model.Picture.Split(',')[1];
                filePath = $"{configManager.EmployeePhotoPath}{model.Id.TrimStart(new Char[] { '0' })}.jpg";
                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(imgb64));
            }

            if (model.ContratcType.Equals("1"))
            {
                var row = nomipaqRepo.SearhItemsFor(d => d.codigoempleado.Equals(model.Id));

                if (row.Any())
                {
                    var element = row.First();


                    nomipaqRepo.Update(element);
                }
                else
                {
                    nomipaqRepo.Create(new Nomipaq_nom10001()
                    {
                        codigoempleado = model.Id,
                        nombre = model.FirstName,
                        apellidopaterno = model.LastName,
                        apellidomaterno = model.LastName2,
                        EntidadFederativa = "",
                        fechaalta = string.IsNullOrEmpty(model.HireDate) == true ? DateTime.Today : DateTime.Parse(model.HireDate),
                        fechanacimiento = string.IsNullOrEmpty(model.FechaNacimiento) == true ? DateTime.Today : DateTime.Parse(model.FechaNacimiento),
                        sexo = model.Sexo,
                        rfc = model.RFC.Substring(0, 4),
                        homoclave = model.RFC.Substring(10, 3),
                        curpi = model.CURP.Substring(0, 4),
                        curpf = model.CURP.Substring(10, 8)
                    });

                    isosaRepo.Create(new EmployeeAditionalInfo()
                    {
                        EMP_EmployeeID = model.Id,
                        EMP_FirstName = model.FirstName,
                        EMP_LastName = $"{model.LastName} {model.LastName2}",
                        EMP_RFC = model.RFC.Substring(0, 4),
                        EMP_homoclave = model.RFC.Substring(10, 3),
                        EMP_curpi = model.CURP.Substring(0, 4),
                        EMP_curpF = model.CURP.Substring(10, 8),
                        EMP_BirthDate = string.IsNullOrEmpty(model.FechaNacimiento) == true ? DateTime.Today : DateTime.Parse(model.FechaNacimiento),
                        EMP_DateOfHire = string.IsNullOrEmpty(model.HireDate) == true ? DateTime.Today : DateTime.Parse(model.HireDate),
                        HrowGuid = Guid.NewGuid(),
                        EMP_UserDef4 = filePath,
                        EMP_EmployeeID_Edit = "-",
                        EMP_StatusCode = "Active",
                        EMP_UserDef1 = model.Observations
                    });


                }

            }


            return Json("ok");
        }


        [HttpPost]
        public JsonResult GetLastNumber(string newOption)
        {
            string value = _organigramaModelFactory.GetNextIdEmployeeRepository(newOption);

            return Json(new { nextid = value });
        }

        [HttpPost]
        public JsonResult SavePersonalInfo(EmployeeDetailPersonalInfoNew model,
            [FromServices] IRepository<EmployeeAditionalInfo> isosaRepo) 
        {

            var row = isosaRepo.SearhItemsFor(d => d.EMP_EmployeeID.Equals(model.EmployeeId));
            if (row.Any()) 
            {
                var data = row.First();
                data.EMP_City = model.City;
                data.EMP_State = model.State;
                data.EMP_Zip = model.Zip;
                data.EMP_Phone = model.Phone;
                data.EMP_CellPhone = model.CellPhone;
                data.EMP_Address = model.Address;
                data.EMP_BloodType = model.BloodType;
                data.EMP_Sick = model.Sick;
                data.EMP_PersonalEmail = model.Email;
                data.EMP_Phone = model.Phone;
                isosaRepo.Update(data);
            }
            return Json("ok");
        }

        [HttpPost]
        public JsonResult SaveContactEmergency(EmployeeDetailEmergencyDataNew model,
            [FromServices] IRepository<EmployeeAditionalInfo> isosaRepo)
        {
            var row = isosaRepo.SearhItemsFor(d => d.EMP_EmployeeID.Equals(model.EmployeeId));
            if (row.Any())
            {
                var data = row.First();
                data.EMP_EmergencyContactName = model.Name;
                data.EMP_EmergencyContactPhone = model.Phone;
                data.EMP_EmergencyContactRelation = model.Relacion;
                isosaRepo.Update(data);
            }
            return Json("ok");
        }

        [HttpPost]
        public ActionResult UploadPhoto(IFormFile file)
        {
            string filename = GenerateFileName("jpg");
            string localTempPath = $"{_host.WebRootPath}/Document/Temp/{filename}/";

            string filePath = $"{localTempPath}{filename}";
            if (file != null)
            {
                if (file.Length > 0)
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                }
            }

            return Json(new { filetemp = filePath });
        }

        public string GenerateFileName(string extension = "")
        {
            return string.Concat(Path.GetRandomFileName().Replace(".", ""),
                (!string.IsNullOrEmpty(extension)) ? (extension.StartsWith(".") ? extension : string.Concat(".", extension)) : "");
        }


        [HttpPost]
        public JsonResult SaveEditEmployee(EmployeeDetailGeneralInfoNew model,
        [FromServices] IRepository<Nomipaq_nom10001> nomipaqRepo,
        [FromServices] IRepository<EmployeeAditionalInfo> isosaRepo,
        [FromServices] IConfigurationManager configManager)
        {
            var rowNomi = nomipaqRepo.SearhItemsFor(d => d.codigoempleado.Equals(model.Id));
            var rowIsosa = isosaRepo.SearhItemsFor(d => d.EMP_EmployeeID.Equals(model.Id));
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(model.Picture))
            {
                var imgb64 = model.Picture.Split(',')[1];
                filePath = $"{rowIsosa.FirstOrDefault().EMP_UserDef4}";
                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(imgb64));
            }

            if (rowNomi.Any())
            {
                var element = rowNomi.First();
                element.nombre = model.FirstName;
                element.apellidopaterno = model.LastName;
                element.apellidomaterno = model.LastName2;
                element.EntidadFederativa = "";
                element.fechaalta = DateTime.Parse(model.HireDate);
                element.fechanacimiento = DateTime.Parse(model.FechaNacimiento);
                element.sexo = model.Sexo;
                element.rfc = model.RFC.Substring(0, 4);
                element.homoclave = model.RFC.Substring(10, 3);
                element.curpi = model.CURP.Substring(0, 4);
                element.curpf = model.CURP.Substring(10, 8);
                if (!string.IsNullOrEmpty(filePath))
                {
                    rowIsosa.FirstOrDefault().EMP_UserDef4 = filePath;
                }

                nomipaqRepo.Update(element);
            }
            if (rowIsosa.Any())
            {
                var element = rowIsosa.First();

                element.EMP_FirstName = model.FirstName;
                element.EMP_LastName = $"{model.LastName} {model.LastName2}";
                element.EMP_RFC = model.RFC.Substring(0, 4);
                element.EMP_homoclave = model.RFC.Substring(10, 3);
                element.EMP_curpi = model.CURP.Substring(0, 4);
                element.EMP_curpF = model.CURP.Substring(10, 8);
                element.EMP_BirthDate = DateTime.Parse(model.FechaNacimiento);
                element.EMP_DateOfHire = DateTime.Parse(model.HireDate);
                if (!string.IsNullOrEmpty(filePath)) 
                {
                    element.EMP_UserDef4 = filePath;
                }
                element.EMP_EMailAddress = model.Email;
                element.EMP_UserDef1 = model.Observations;
                isosaRepo.Update(element);
            }

            return Json("ok");
        }



        #endregion

        #region Employee Unsuscribe

        [HttpPost]
        public JsonResult EmployeeUnsubscribe(string Id,
        [FromServices] IRepository<Nomipaq_nom10001> nomipaqRepo,
        [FromServices] IRepository<EmployeeAditionalInfo> isosaRepo)
        {
            var isosaRow = isosaRepo.SearhItemsFor(d => d.EMP_EmployeeID.Equals(Id));
            var nomipaqRow = nomipaqRepo.SearhItemsFor(d => d.codigoempleado.Equals(Id));

            if (isosaRow.Any()) 
            {
                var row = isosaRow.FirstOrDefault();
                row.EMP_StatusCode = "Inactive";
                isosaRepo.Update(row);
            }
            if (nomipaqRow.Any())
            {
                var row = nomipaqRow.FirstOrDefault();
                row.estadoempleado = "B";
                nomipaqRepo.Update(row);
            }

            return Json(new { employeeid = Id.TrimStart(new Char[] { '0' }) });
        }

        #endregion

    }
}
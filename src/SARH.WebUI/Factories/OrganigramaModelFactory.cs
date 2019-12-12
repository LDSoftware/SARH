using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Repository;
using ISOSADataMigrationTools;
using Microsoft.AspNetCore.Mvc;
using SARH.WebUI.Models.Organigrama;

namespace SARH.WebUI.Factories
{
    public class OrganigramaModelFactory : IOrganigramaModelFactory
    {
        private readonly IRepository<Nomipaq_nom10001> _nomipaqRepository;
        private readonly IRepository<EmployeeAditionalInfo> _isosaemployeesRepository;
        private readonly IRepository<EmployeeOrganigrama> _isosaemployeesOrganigramaRepository;

        public OrganigramaModelFactory(IRepository<Nomipaq_nom10001> nomipaqRepository, 
            IRepository<EmployeeAditionalInfo> isosaemployeesRepository,
            IRepository<EmployeeOrganigrama> isosaemployeesOrganigramaRepository)
        {
            _nomipaqRepository = nomipaqRepository;
            _isosaemployeesRepository = isosaemployeesRepository;
            _isosaemployeesOrganigramaRepository = isosaemployeesOrganigramaRepository;
        }

        public OrganigramaModel GetData(string organigramaPath)
        {
            string[] path = organigramaPath.Split('.');

            OrganigramaModel model = new OrganigramaModel()
            {
                Area = path[0],
                JobCenter = path[1],
                Employess = new List<OrganigramaEmployeeModel>()
            };

            //EmployeeManager manager = new EmployeeManager();
            //manager.Create();
            var emps = _isosaemployeesRepository.SearhItemsFor(a => a.Area.ToLower().Equals(path[0].ToLower()) && a.Centro.ToLower().Equals(path[1].ToLower())).Select(j => new OrganigramaEmployeeModel { Id = j.EMP_EmployeeID, Area = j.Area, JobCenter = j.Centro, Category = j.EMP_ASIGNACION, JobTitle = j.Perfil, Name = j.EMP_FirstName + " " + j.EMP_LastName, RowId = j.HrowGuid.ToString().ToLower() });
            //model.Employess.AddRange(emps);

            //HierarchyManager hierarchy = new HierarchyManager();
            //hierarchy.Create();

            var iden = _isosaemployeesOrganigramaRepository.SearhItemsFor(h => h.RowGuid.Equals(emps.Where(f => f.RowId != "").FirstOrDefault().RowId.ToLower()));

            model.RowId = iden.FirstOrDefault().IdentPuesto.ToString().ToLower();

            return model;
        }


        public OrganigramaModel GetAllData() 
        {
            OrganigramaModel model = new OrganigramaModel() 
            {
                Employess = new List<OrganigramaEmployeeModel>()
            };

            //EmployeeManager manager = new EmployeeManager();
            //manager.Create();

            var isosaOrg = _isosaemployeesOrganigramaRepository.GetAll();
            var isosaEmp = _isosaemployeesRepository.SearhItemsFor(o => o.EMP_StatusCode.Equals("Active"));


            var emps = (from emp in isosaEmp
                        join org in isosaOrg on emp.HrowGuid.Value equals org.RowGuid
                        where emp.EMP_StatusCode == "Active"
                        select new OrganigramaEmployeeModel()
                        { 
                            Id = emp.EMP_EmployeeID.TrimStart(new Char[] { '0' }),
                            Area = org.Area,
                            JobCenter = org.Centro,
                            Category = org.Departamento,
                            JobTitle = org.Puesto,
                            Name = $"{emp.EMP_FirstName} {emp.EMP_LastName}",
                            RowId = emp.HrowGuid.ToString()
                        }).ToList();

            //var emps = _isosaemployeesRepository.SearhItemsFor(o => o.EMP_StatusCode.Equals("Active")).Select(j => new OrganigramaEmployeeModel()
            //{
            //    Id = j.EMP_EmployeeID.TrimStart(new Char[] { '0' }),
            //    Area = GetInfoOrg("1", j.HrowGuid.Value.ToString(), isosaOrg),
            //    JobCenter = GetInfoOrg("2", j.HrowGuid.Value.ToString(), isosaOrg),
            //    Category = GetInfoOrg("3", j.HrowGuid.Value.ToString(), isosaOrg),
            //    JobTitle = GetInfoOrg("4", j.HrowGuid.Value.ToString(), isosaOrg),
            //    Name = $"{j.EMP_FirstName} {j.EMP_LastName}",
            //    RowId = j.HrowGuid.ToString()
            //});

            model.Employess.AddRange(emps) ;

            return model;
        }


        private string GetInfoOrg(string result, string hrow, IEnumerable<EmployeeOrganigrama> organigrama)
        {
            string response = string.Empty;
            if (result == "1") 
            {
                response = organigrama.Where(t => t.RowGuid.ToString().Equals(hrow)).FirstOrDefault().Area;
            }
            if (result == "2")
            {
                response = organigrama.Where(t => t.RowGuid.ToString().Equals(hrow)).FirstOrDefault().Centro;
            }
            if (result == "3")
            {
                response = organigrama.Where(t => t.RowGuid.ToString().Equals(hrow)).FirstOrDefault().Departamento;
            }
            if (result == "3")
            {
                response = organigrama.Where(t => t.RowGuid.ToString().Equals(hrow)).FirstOrDefault().Puesto;
            }

            return response;
        } 


        public OrganigramaEmployeeDetailModel GetEmployeeData(string employeeid)
        {
            OrganigramaEmployeeDetailModel model = new OrganigramaEmployeeDetailModel();

            //EmployeeManager manager = new EmployeeManager();
            //manager.Create();
            //HierarchyManager hierarchy = new HierarchyManager();
            //hierarchy.Create();
            //NomiPaqEmployeeManager nomiemployee = new NomiPaqEmployeeManager();
            //nomiemployee.Create();

            var employee = _isosaemployeesRepository.SearhItemsFor(e => e.EMP_EmployeeID.Equals(int.Parse(employeeid).ToString("00000")));


            if (employee.Any())
            {



                var rowIdent = _isosaemployeesOrganigramaRepository.SearhItemsFor(h => h.RowGuid.ToString().ToLower().Equals(employee.FirstOrDefault().HrowGuid.ToString().ToLower()));
                model.HierarchyGuid = employee.FirstOrDefault().HrowGuid.ToString().ToLower();
                if (rowIdent.Any())
                {
                    model.RowId = rowIdent.FirstOrDefault().IdentPuesto.Value.ToString().ToLower();
                }
                else
                {
                    model.RowId = employee.FirstOrDefault().HrowGuid.ToString().ToLower();
                }

                model.Area = rowIdent.FirstOrDefault().Area;
                model.JobCenter = rowIdent.FirstOrDefault().Centro;

                //var nomiemp = nomiemployee.Employees.Where(e => e.codigoempleado.Equals(employee.FirstOrDefault().EMP_EmployeeID));

                var nomiempNew = _nomipaqRepository.SearhItemsFor(e => e.codigoempleado.Equals(int.Parse(employeeid).ToString("00000")));


                if (nomiempNew.Any())
                {
                    model.GeneralInfo.Id = int.Parse(nomiempNew.FirstOrDefault().codigoempleado).ToString("00000");
                    model.GeneralInfo.FirstName = nomiempNew.FirstOrDefault().nombre;
                    model.GeneralInfo.LastName = nomiempNew.FirstOrDefault().apellidopaterno;
                    model.GeneralInfo.LastName2 = nomiempNew.FirstOrDefault().apellidomaterno;
                    model.GeneralInfo.RFC = $"{nomiempNew.FirstOrDefault().rfc}{nomiempNew.FirstOrDefault().fechanacimiento.Value.ToShortDateString().Replace("/", string.Empty)}{nomiempNew.FirstOrDefault().homoclave}";
                    model.GeneralInfo.NSS = nomiempNew.FirstOrDefault().numerosegurosocial;
                    model.GeneralInfo.CURP = $"{nomiempNew.FirstOrDefault().curpi}{nomiempNew.FirstOrDefault().fechanacimiento.Value.ToShortDateString().Replace("/", string.Empty)}{nomiempNew.FirstOrDefault().curpf}";
                    model.GeneralInfo.HireDate = nomiempNew.FirstOrDefault().fechaalta.Value.ToShortDateString();
                    model.GeneralInfo.FechaNacimiento = nomiempNew.FirstOrDefault().fechanacimiento.Value.ToShortDateString();
                    model.GeneralInfo.Sexo = nomiempNew.FirstOrDefault().sexo;
                }
                else
                {
                    model.GeneralInfo.Id = employee.FirstOrDefault().EMP_EmployeeID;
                    model.GeneralInfo.FirstName = employee.FirstOrDefault().EMP_FirstName;
                    model.GeneralInfo.LastName = employee.FirstOrDefault().EMP_LastName;
                    model.GeneralInfo.RFC = employee.FirstOrDefault().EMP_RFC;
                    model.GeneralInfo.NSS = employee.FirstOrDefault().EMP_SocialSecurNbr;
                    model.GeneralInfo.CURP = employee.FirstOrDefault().EMP_curpi;
                    model.GeneralInfo.HireDate = employee.FirstOrDefault().EMP_DateOfHire.Value.ToShortDateString();
                    model.GeneralInfo.FechaNacimiento = employee.FirstOrDefault().EMP_BirthDate.Value.ToShortDateString();
                    model.GeneralInfo.Sexo = nomiempNew.FirstOrDefault().sexo;
                }

                model.GeneralInfo.Email = employee.FirstOrDefault().EMP_EMailAddress;
                model.GeneralInfo.Observations = employee.FirstOrDefault().EMP_UserDef1;

                model.GeneralInfo.JobTitle = rowIdent.FirstOrDefault().Puesto;
                model.GeneralInfo.StartFood = "";
                model.GeneralInfo.StartJob = "";
                model.GeneralInfo.EndFood = "";
                model.GeneralInfo.EndJob = "";
                model.GeneralInfo.JobCenter = rowIdent.FirstOrDefault().Centro;
                model.GeneralInfo.Area = rowIdent.FirstOrDefault().Area;

                if (!string.IsNullOrEmpty(employee.FirstOrDefault().EMP_UserDef4))
                {
                    if (System.IO.File.Exists(employee.FirstOrDefault().EMP_UserDef4))
                    {
                        var filePicture = System.IO.File.ReadAllBytes(employee.FirstOrDefault().EMP_UserDef4);
                        model.GeneralInfo.Picture = "data:image/jpg;base64," + Convert.ToBase64String(filePicture, 0, filePicture.Length);
                    }
                    else
                    {
                        model.GeneralInfo.Picture = "";
                    }
                }
                else
                {
                    model.GeneralInfo.Picture = "";
                }

                model.PersonalInfo.City = employee.FirstOrDefault().EMP_City;
                model.PersonalInfo.State = employee.FirstOrDefault().EMP_State;
                model.PersonalInfo.Zip = employee.FirstOrDefault().EMP_Zip;
                model.PersonalInfo.Phone = employee.FirstOrDefault().EMP_Phone;
                model.PersonalInfo.Address = employee.FirstOrDefault().EMP_Address;
                model.PersonalInfo.BloodType = employee.FirstOrDefault().EMP_BloodType;
                model.PersonalInfo.Email = employee.FirstOrDefault().EMP_PersonalEmail;
                model.PersonalInfo.Sick = employee.FirstOrDefault().EMP_Sick;
                model.PersonalInfo.CellPhone = employee.FirstOrDefault().EMP_CellPhone;

                model.EmergencyData.Name = employee.FirstOrDefault().EMP_EmergencyContactName;
                model.EmergencyData.Phone = employee.FirstOrDefault().EMP_EmergencyContactPhone;
                model.EmergencyData.Relacion = employee.FirstOrDefault().EMP_EmergencyContactRelation;
            }

            return model;
        }

        public string GetNextIdEmployeeRepository(string optionSelected)
        {
            string value = string.Empty;
            if (optionSelected.Equals("2"))
            {
                value = _isosaemployeesRepository.GetAll().ToList().Max(d => int.Parse(d.EMP_EmployeeID)).ToString();
            }
            else
            {
                value = _nomipaqRepository.GetAll().ToList().Max(d => d.codigoempleado.TrimStart(new Char[] { '0' })).ToString();
            }

            return (int.Parse(value) + 1).ToString("00000");
        }




    }
}

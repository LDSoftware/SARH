using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using SARH.WebUI.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public class EmployeeFormatModelFactory : IEmployeeFormatModelFactory
    {
        private readonly IRepository<EmployeeFormat> _formatRepository;
        private readonly IOrganigramaModelFactory _organigrama;
        private readonly IRepository<PermissionType> _permissionType;

        public EmployeeFormatModelFactory(IRepository<EmployeeFormat> formatRepository,
        IOrganigramaModelFactory organigrama, IRepository<PermissionType> permissionType)
        {
            this._formatRepository = formatRepository;
            this._organigrama = organigrama;
            this._permissionType = permissionType;
        }


        public List<EmployeeFormatInfo> GetAllFormats(DateTime StartDate, DateTime EndDate)
        {
            List<EmployeeFormatInfo> response = new List<EmployeeFormatInfo>();

            var formats = this._formatRepository.SearhItemsFor(f => f.StartDate >= StartDate && f.EndDate <= EndDate);
            var organigrama = this._organigrama.GetAllData();
            var permissionTypes = this._permissionType.GetAll();

            var formatData = (from fmts in formats
                              join orgn in organigrama.Employess on fmts.EmployeeId equals orgn.Id
                              join ftypes in permissionTypes on fmts.PermissionType equals ftypes.Id
                              select new EmployeeFormatInfo()
                              {
                                  Area = orgn.Area,
                                  Centro = orgn.JobCenter,
                                  JobTitle = orgn.JobTitle,
                                  Departament = orgn.Category,
                                  Name = orgn.Name,
                                  FormatName = ftypes.Description,
                                  Comments = fmts.Comments,
                                  StartDate = fmts.StartDate.ToShortDateString(),
                                  EndDate = fmts.EndDate.ToShortDateString(),
                                  StartTime = fmts.StartTime,
                                  EndTime = fmts.EndTime,
                                  EmployeeId = fmts.EmployeeId,
                                  FormatType = FormatGroup(ftypes.Description)
                              }
                              ).ToList();

            response.AddRange(formatData);

            return response;
        }

        public EmployeeFormatInfo GetformatInfo(int Id)
        {
            EmployeeFormatInfo response = new EmployeeFormatInfo();
            var formats = this._formatRepository.SearhItemsFor(j =>j.Id.Equals(Id));
            var organigrama = this._organigrama.GetAllData();
            var permissionTypes = this._permissionType.GetAll();

            var formatData = (from fmts in formats
                              join orgn in organigrama.Employess on fmts.EmployeeId equals orgn.Id
                              join ftypes in permissionTypes on fmts.PermissionType equals ftypes.Id
                              select new EmployeeFormatInfo()
                              {
                                  Area = orgn.Area,
                                  Centro = orgn.JobCenter,
                                  JobTitle = orgn.JobTitle,
                                  Departament = orgn.Category,
                                  Name = orgn.Name,
                                  FormatName = ftypes.Description,
                                  Comments = fmts.Comments,
                                  StartDate = fmts.StartDate.ToShortDateString(),
                                  EndDate = fmts.EndDate.ToShortDateString(),
                                  StartTime = fmts.StartTime,
                                  EndTime = fmts.EndTime,
                                  EmployeeId = fmts.EmployeeId,
                                  FormatType = FormatGroup(ftypes.Description),
                                  FechaSolicitud = fmts.CreateDate.ToShortDateString(),
                                  Suplente = fmts.EmployeeSubstitute,
                                  Aprobadores = fmts.ApprovalWorkFlow,
                                  AdditionalInfo = fmts.WithPay ? "Con goce de sueldo" : "Sin goce de sueldo",
                                  Approved = fmts.ApprovalDate.HasValue
                              }
                              ).ToList();


            response = formatData.FirstOrDefault();
            return response;
        }

        public List<EmployeeFormatInfo> GetAllApprovedFormats(DateTime StartDate) 
        {
            List<EmployeeFormatInfo> response = new List<EmployeeFormatInfo>();

            var formats = this._formatRepository.SearhItemsFor(j => j.CreateDate.ToShortDateString().Equals(StartDate.ToShortDateString()));
            var organigrama = this._organigrama.GetAllData();
            var permissionTypes = this._permissionType.GetAll();

            var formatData = (from fmts in formats
                              join orgn in organigrama.Employess on fmts.EmployeeId equals orgn.Id
                              join ftypes in permissionTypes on fmts.PermissionType equals ftypes.Id
                              where fmts.ApprovalDate.HasValue
                              select new EmployeeFormatInfo()
                              {
                                  Area = orgn.Area,
                                  Centro = orgn.JobCenter,
                                  JobTitle = orgn.JobTitle,
                                  Departament = orgn.Category,
                                  Name = orgn.Name,
                                  FormatName = ftypes.Description,
                                  Comments = fmts.Comments,
                                  StartDate = fmts.StartDate.ToShortDateString(),
                                  EndDate = fmts.EndDate.ToShortDateString(),
                                  StartTime = fmts.StartTime,
                                  EndTime = fmts.EndTime,
                                  EmployeeId = fmts.EmployeeId,
                                  FormatType = FormatGroup(ftypes.Description)
                              }
                              ).ToList();

            response.AddRange(formatData);

            return response;
        }

        public List<EmployeeFormatInfo> GetAllFormatsByEmployee(string EmployeeId) 
        {
            List<EmployeeFormatInfo> response = new List<EmployeeFormatInfo>();

            var formats = this._formatRepository.SearhItemsFor(j => j.EmployeeId.Equals(EmployeeId));
            var organigrama = this._organigrama.GetAllData();
            var permissionTypes = this._permissionType.GetAll();

            var formatData = (from fmts in formats
                              join orgn in organigrama.Employess on fmts.EmployeeId equals orgn.Id
                              join ftypes in permissionTypes on fmts.PermissionType equals ftypes.Id
                              where fmts.ApprovalDate.HasValue
                              select new EmployeeFormatInfo()
                              {
                                  Area = orgn.Area,
                                  Centro = orgn.JobCenter,
                                  JobTitle = orgn.JobTitle,
                                  Departament = orgn.Category,
                                  Name = orgn.Name,
                                  FormatName = ftypes.Description,
                                  Comments = fmts.Comments,
                                  StartDate = fmts.StartDate.ToShortDateString(),
                                  EndDate = fmts.EndDate.ToShortDateString(),
                                  StartTime = fmts.StartTime,
                                  EndTime = fmts.EndTime,
                                  EmployeeId = fmts.EmployeeId,
                                  FormatType = FormatGroup(ftypes.Description)
                              }
                              ).ToList();

            response.AddRange(formatData);

            return response;

        }

        private string FormatGroup(string formatType)
        {
            string result = string.Empty;

            if (formatType.ToLower().Contains("incapaci"))
            {
                result = "Incapacidades";
            }
            if (formatType.ToLower().Contains("permiso"))
            {
                result = "Permisos";
            }
            if (formatType.ToLower().Contains("vacacion"))
            {
                result = "Vacaciones";
            }

            return result;
        }



    }
}

using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SARH.WebUI.Models.Notification;
using SARH.WebUI.Models.Organigrama;
using SmartAdmin.WebUI.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public class NotificationModelFactory : INotificationModelFactory
    {
        private int _notification;
        private readonly IRepository<EmployeeFormat> _formatRepository;
        private readonly IRepository<FormatApprover> _formatApproverRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganigramaModelFactory _organigramaModelFactory;
        private readonly IRepository<PermissionType> _permissionTypeRepository;

        public NotificationModelFactory(IRepository<EmployeeFormat> formatRepository,
            IRepository<FormatApprover> formatApproverRepository,
            IHttpContextAccessor httpContextAccessor,
            IOrganigramaModelFactory organigramaModelFactory,
            IRepository<PermissionType> permissionTypeRepository)
        {
            _notification = 0;
            this._formatApproverRepository = formatApproverRepository;
            this._formatRepository = formatRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._organigramaModelFactory = organigramaModelFactory;
            this._permissionTypeRepository = permissionTypeRepository;

            AllNotificaticonsItems = new List<NotificacionModelItem>();
            NotificaticonsItems = new List<NotificacionModelItem>();
            LastVacationsNotificationItems = new List<NotificacionModelItem>();
            LastPermissionsNotificationItems = new List<NotificacionModelItem>();

            CountPendigFormat();
        }

        public int Notification { get => _notification; set => _notification = value; }

        public List<NotificacionModelItem> AllNotificaticonsItems { get; set; }
        public List<NotificacionModelItem> NotificaticonsItems { get; set; }
        public List<NotificacionModelItem> LastVacationsNotificationItems { get; set; }
        public List<NotificacionModelItem> LastPermissionsNotificationItems { get; set; }
        public List<NotificacionModelItem> LastOthersNotificationItems { get; set; }

        private void CountPendigFormat() 
        {
            var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");
            var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
            string userName = loginInfo != null ? loginInfo.Email : null;


            var employees = this._organigramaModelFactory.GetAllData().Employess;
            var formats = this._formatRepository.GetAll();
            var employee = employees.Where(t => t.UserName.Equals(userName)).FirstOrDefault();
            if (employee != null) 
            {
                var approver = _formatApproverRepository.SearhItemsFor(s => s.RowGuid.ToString().ToLower().Equals(employee.RowId)).FirstOrDefault();
                var permissionTypes = this._permissionTypeRepository.GetAll();

                var currentformats = (from fmt in formats
                                      join emp in employees on fmt.EmployeeId equals emp.Id
                                      join ptype in permissionTypes on fmt.PermissionType equals ptype.Id
                                      select new NotificacionModelItem()
                                      {
                                          Id = fmt.Id,
                                          EmployeeId = fmt.EmployeeId.TrimStart(new Char[] { '0' }),
                                          Area = emp.Area,
                                          JobCenter = emp.JobCenter,
                                          Deparment = emp.Category,
                                          JobTitle = emp.JobTitle,
                                          Name = emp.Name,
                                          RowId = emp.RowId,
                                          ApproverWorkFlow = fmt.ApprovalWorkFlow,
                                          Estatus = string.IsNullOrEmpty(fmt.ApprovalWorkFlow) ? "Pendiente" : fmt.ApprovalDate.HasValue ? "Aprobado" : fmt.Declined ? "Rechazado" : "En Curso",
                                          CreateDate = fmt.CreateDate.ToShortDateString(),
                                          Period = $"{fmt.StartDate.ToShortDateString()} - {fmt.EndDate.ToShortDateString()}",
                                          Type = ptype.Description
                                      }).ToList();

                if (employee != null && approver != null)
                {
                    var formatsPendigs = currentformats.Where(m => m.Area.Equals(approver.Area)
                    && m.JobCenter.Equals(approver.Centro)
                    && m.Deparment.Equals(approver.Departamento)).ToList();

                    if (formatsPendigs.Any())
                    {
                        _notification = formatsPendigs.Count;
                        AllNotificaticonsItems.AddRange(formatsPendigs);
                        NotificaticonsItems.AddRange(formatsPendigs.Where(k => string.IsNullOrEmpty(k.ApproverWorkFlow)));
                        LastVacationsNotificationItems = NotificaticonsItems.OrderByDescending(f => f.Id).Where(y => y.Type.Contains("Vacaci")).Take(10).ToList();
                        LastPermissionsNotificationItems = NotificaticonsItems.OrderByDescending(f => f.Id).Where(y => y.Type.Contains("Permi")).Take(10).ToList();
                        LastOthersNotificationItems = NotificaticonsItems.OrderByDescending(f => f.Id).Where(y => !y.Type.Contains("Permi") && !y.Type.Contains("Vacaci")).Take(10).ToList();
                    }
                }
            }
        }

    }
}

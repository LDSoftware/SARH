using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

        public NotificationModelFactory(IRepository<EmployeeFormat> formatRepository,
            IRepository<FormatApprover> formatApproverRepository,
            IHttpContextAccessor httpContextAccessor,
            IOrganigramaModelFactory organigramaModelFactory)
        {
            _notification = 0;
            this._formatApproverRepository = formatApproverRepository;
            this._formatRepository = formatRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._organigramaModelFactory = organigramaModelFactory;

            CountPendigFormat();
        }

        public int Notification { get => _notification; set => _notification = value; }



        private void CountPendigFormat() 
        {
            var modellogin = _httpContextAccessor.HttpContext.Session.GetString("loginmodel");
            var loginInfo = !string.IsNullOrEmpty(modellogin) ? JsonConvert.DeserializeObject<LoginModel.InputModel>(modellogin) : null;
            string userName = loginInfo != null ? loginInfo.Email : null;


            var employees = this._organigramaModelFactory.GetAllData().Employess;
            var formats = this._formatRepository.GetAll();
            var employee = employees.Where(t => t.UserName.Equals(userName)).FirstOrDefault();
            var approver = _formatApproverRepository.SearhItemsFor(s => s.RowGuid.ToString().ToLower().Equals(employee.RowId)).FirstOrDefault();

            var currentformats = (from fmt in formats
                                  join emp in employees on fmt.EmployeeId equals emp.Id
                                  where fmt.ApprovalWorkFlow.Equals(string.Empty)
                                  select new OrganigramaEmployeeModel()
                                  {
                                      Id = fmt.EmployeeId.TrimStart(new Char[] { '0' }),
                                      Area = emp.Area,
                                      JobCenter = emp.JobCenter,
                                      Category = emp.Category,
                                      JobTitle = emp.JobTitle,
                                      Name = emp.Name,
                                      RowId = emp.RowId,
                                      UserName = emp.UserName
                                  }).ToList();

            if (employee != null)
            {
                var formatsPendigs = currentformats.Where(m => m.Area.Equals(approver.Area)
                && m.JobCenter.Equals(approver.Centro)
                && m.Category.Equals(approver.Departamento)).ToList();

                if (formatsPendigs.Any())
                {
                    _notification = formatsPendigs.Count;
                }
            }

        }





    }
}

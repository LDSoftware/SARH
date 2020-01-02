using ISOSA.SARH.Data.Domain.Assignation;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using SARH.WebUI.Data;
using SARH.WebUI.Factories;
using SARH.WebUI.Hubs;
using SARH.WebUI.Models;
using SARH.WebUI.Models.OrganizationChart;
using System;

namespace SmartAdmin.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache(); //This way ASP.NET Core will use a Memory Cache to store session variables
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1); // It depends on user requirements.
            });

            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IDashboardModelFactory, DashboardModelFactory>();
            services.AddTransient<OrganizationChartAreaModelFactory>();
            services.AddTransient<IOrganigramaModelFactory, OrganigramaModelFactory>();
            services.AddTransient<INotificationModelFactory, NotificationModelFactory>();
            services.AddTransient<IRepository<DocumentType>, DocumentTypeRepository>(s => new DocumentTypeRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<HardwareAssigned>, HardwareAssignedRepository>(s => new HardwareAssignedRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<PermissionType>, PermissionTypeRepository>(s => new PermissionTypeRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<SafeEquipmentAssigned>, SafeEquipmentAssignedRepository>(s => new SafeEquipmentAssignedRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeObjectAsignation>, EmployeeObjectAsignationRepository>(s => new EmployeeObjectAsignationRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<NonWorkingDay>, NonWorkingDayRepository>(s => new NonWorkingDayRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<NonWorkingDayException>, NonWorkingDayExceptionRepository>(s => new NonWorkingDayExceptionRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<Schedule>, ScheduleRepository>(s => new ScheduleRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeScheduleAssigned>, EmployeeScheduleAssignedRepository>(s => new EmployeeScheduleAssignedRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeScheduleAssignedTemp>, EmployeeScheduleAssignedTempRepository>(s => new EmployeeScheduleAssignedTempRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeFormat>, EmployeeFormatRepository>(s => new EmployeeFormatRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<PersonalDocument>, PersonalDocumentRepository>(s => new PersonalDocumentRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeProfile>, EmployeeProfileRepository>(s => new EmployeeProfileRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeDiscount>, EmployeeDiscountRepository>(s => new EmployeeDiscountRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<Nomipaq_nom10001>, NomipaqEmployeesRepository>(s => new NomipaqEmployeesRepository((Configuration.GetConnectionString("DataConnectionStringNomipaq"))));
            services.AddTransient<IRepository<EmployeeAditionalInfo>, EmployeeAditionalInfoRepository>(s => new EmployeeAditionalInfoRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<FormatApprovedEmployee>, FormatApprobedEmployeeRepository>(s => new FormatApprobedEmployeeRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<FormatApprovedHubId>, FormatApprobedHubIdRepository>(s => new FormatApprobedHubIdRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<FormatApprover>, FormatApproverRepository>(s => new FormatApproverRepository((Configuration.GetConnectionString("DataConnectionString"))));
            services.AddTransient<IRepository<EmployeeOrganigrama>, EmployeeOrganigramaRepository>(s => new EmployeeOrganigramaRepository((Configuration.GetConnectionString("DataConnectionStringISOSA"))));
            services.AddTransient<SARH.Core.Configuration.IConfigurationManager, SARH.WebUI.Configuration.ConfigurationManager>(s => new SARH.WebUI.Configuration.ConfigurationManager("appsettings.json", "ApplicationFormatPath"));
            services.AddTransient<IElementUpdateModelFactory, ElementUpdateModelFactory>(s => new ElementUpdateModelFactory(new ElementUpdateRepository(Configuration.GetConnectionString("DataConnectionString"))));

            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                    options.Filters.Add<ViewBagFilter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/notificationHub", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

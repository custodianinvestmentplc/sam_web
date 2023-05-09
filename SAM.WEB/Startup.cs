using SAM.WEB.Domain;
using SAM.WEB.Models;
using SAM.WEB.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;
using SAM.WEB.Repo;
using AutoMapper;
using SAM.WEB.MiddleWares;
using System.Configuration;
using SAM.WEB.Domain.Dtos;

namespace SAM.WEB
{
    public class Startup
    {
        private readonly LoginConfig _loginConfig;
        private readonly DatabaseConfig _dbConfig;

        private readonly string _baseUrl;
        private readonly MediaTypeWithQualityHeaderValue _contentType;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            var config = new LoginConfig();
            var db = new DatabaseConfig();

            string baseUrl = configuration.GetValue<string>("AppSettings:BaseUrl");
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");

            configuration.GetSection("auth").Bind(config);
            configuration.GetSection("databases").Bind(db);

            _loginConfig = config;
            _dbConfig = db;
            _baseUrl = baseUrl;
            _contentType = contentType;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "RDS.Custodian.Cookie";
                options.LoginPath = new PathString("/Home/Index");
            });

            services.AddHttpClient("client", httpClient =>
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(_contentType);
            });

            services.AddSingleton(_loginConfig);
            services.AddSingleton(_dbConfig);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            services.AddScoped<IUserServices>(s => new UserServiceFacade(_dbConfig.cpc));
            services.AddScoped<IAgentServices>(s => new AgentServiceFacade(_dbConfig.cpc));
            services.AddScoped<IReportService>(s => new ReportServiceFacade(_dbConfig.cpc));
            services.AddScoped<IGroupSynergyServices>(s => new GroupSynergyServiceFacade(_dbConfig.cpc));
            services.AddScoped<ICPCHubServices>(s => new CPCHubServiceFacade(_dbConfig.cpc));
            services.AddScoped<ICtsService>(s => new CtsFacade(_dbConfig.cpc));
            services.AddScoped<IAuthProvider, AuthProvider>();
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllersWithViews();

            services.Configure<UserRegisterDto>(configuration.GetSection("userregister"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = env.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always
            };

            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            //app.UseMiddleware<UserAuthMiddleware>();
            app.UseMiddleware<UserPermissionsMiddleWare>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

//services.AddHttpContextAccessor();
//services.AddScoped<ActivityLoggerActionFilter>();

//app.GetAppUserFromHttpContext();

//if (env.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//}
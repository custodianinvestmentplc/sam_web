using SAM.NUGET.Domain;
using SAM.NUGET.Models;
using SAM.NUGET.Services;
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
using SAM.WEB.Services;

namespace SAM.WEB
{
    public class Startup
    {
        private readonly LoginConfig _loginConfig;
        private readonly DatabaseConfig _dbConfig;

        private readonly string _baseUrl;
        private readonly MediaTypeWithQualityHeaderValue _contentType;

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

            services.AddScoped<IUserServices>(s => new UserServiceFacade(_dbConfig.cpc));
            services.AddScoped<IAgentServices>(s => new AgentServiceFacade(_dbConfig.cpc));
            services.AddScoped<IReportService>(s => new ReportServiceFacade(_dbConfig.cpc));
            services.AddScoped<IGroupSynergyServices>(s => new GroupSynergyServiceFacade(_dbConfig.cpc));
            services.AddScoped<ICPCHubServices>(s => new CPCHubServiceFacade(_dbConfig.cpc));
            services.AddScoped<ICtsService>(s => new CtsFacade(_dbConfig.cpc));
            services.AddScoped<IAuthProvider, AuthProvider>();
            //services.AddScoped<ActivityLoggerActionFilter>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

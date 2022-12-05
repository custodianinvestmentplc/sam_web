using System;
using System.Reflection;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.NUGET.Services;
using SAM.NUGET.ViewModels;
using SAM.NUGET.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SAM.NUGET.Domain.Dtos;
using System.Collections.Generic;
using SAM.WEB.Services;

namespace SAM.WEB.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ModulesController(IConfiguration configuration, IHttpClientFactory httpClientFactory, LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
        }

        public Task<IActionResult> AgencyOperations()
        {
            return GetUserView("AgencyOperationHome");
        }

        public Task<IActionResult> PolicyAdmin()
        {
            return GetUserView("PolicyAdmin");
        }

        public IActionResult Reporting()
        {
            return View(Url.Content("/Views/Modules/ReportingHome.cshtml"));
        }

        public Task<IActionResult> CPCHub()
        {
            return GetUserView("CPCHubHome");
        }

        public async Task<IActionResult> GetUserView(string route)
        {
            var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:AuthUrl");

                if (!string.IsNullOrEmpty(useremail))
                {
                    var query = new Dictionary<string, string>()
                    {
                        ["useremail"] = useremail
                    };

                    var uri = QueryHelpers.AddQueryString(url, query);

                    var user = await DataServices<UserRegisterDto>.GetPayload(uri, _httpClientFactory);

                    ViewBag.Module = user.UserRole.ToLower();

                    ViewBag.Username = user.UserDisplayName;

                    log.Info($"{DateTime.Now.ToString()} - Logged in the User {useremail}");

                    return View(Url.Content($"/Views/Modules/{route}.cshtml"));
                }

                indexVM.ErrorTitle = "Unable to retreive user email";
                indexVM.ExceptionType = "Access Denied";

                return RedirectToAction("Index", "Home", indexVM);
            }
            catch (Exception ex)
            {
                indexVM.ErrorTitle = "You do not belong to any of the designated permission groups";
                indexVM.ExceptionType = "Access Denied";
                indexVM.ErrorDescription = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("Index", "Home", indexVM);
            }
        }

    }
}



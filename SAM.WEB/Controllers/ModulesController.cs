using System;
using System.Reflection;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
using SAM.WEB.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;

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
            //var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:AuthUrl");

                var useremail = HttpContext.Session.Get<string>("userEmail");

                if (!string.IsNullOrEmpty(useremail))
                {
                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = useremail
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //var user = await DataServices<UserRegisterDto>.GetPayload(uri, _httpClientFactory);


                    var user = HttpContext.Session.Get<UserRegisterDto>("UserRegisterDto");

                    if (user != null)
                    {
                        ViewBag.Module = user.UserRole.ToLower();

                        ViewBag.Username = user.UserDisplayName;

                        return View(Url.Content($"/Views/Modules/{route}.cshtml"));
                    }
                    else throw new Exception("No User found.");
                }

                TempData["ErrorTitle"] = "Unable to retreive user email";
                TempData["ExceptionType"] = "Access Denied";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorTitle"] = "You do not belong to any of the designated permission groups";
                TempData["ExceptionType"] = "Access Denied";
                TempData["ErrorDescription"] = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("Index", "Home");
            }
        }

    }
}



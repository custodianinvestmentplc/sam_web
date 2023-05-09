using System;
using System.Reflection;
using SAM.WEB.Models;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Authorize]
    public class AgencyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;

        public AgencyController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices, LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _cpcServices = cpcHubServices;
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
        }

        public IActionResult Home([FromQuery] string userName)
        {
            var model = new ModuleInitOptions
            {
                UserEmail = userName
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ActiveAgentsAsync()
        {
            return await GetUserView();
        }

        [HttpGet]
        public async Task<IActionResult> CreateBatchAsync()
        {
            return await GetUserView();
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteBatchAsync()
        {
            return await GetUserView();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateUploadAsync()
        {
            return await GetUserView();
        }

        [HttpGet]
        public async Task<IActionResult> ORCRateMatrixAsync()
        {
            return await GetUserView();
        }

        [HttpGet]
        public async Task<IActionResult> ORCScenariosAsync()
        {
            return await GetUserView();
        }

        [HttpGet]
        public async Task<IActionResult> OtherAgentTasksAsync()
        {
            return await GetUserView();
        }

        public async Task<IActionResult> GetUserView()
        {
            //var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                //var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var useremail = HttpContext.Session.Get<string>("userEmail");

                var url = _configuration.GetValue<string>("AppSettings:AuthUrl");

                if (!string.IsNullOrEmpty(useremail))
                {
                    var user = HttpContext.Session.Get<UserRegisterDto>("UserRegisterDto");

                    ViewBag.Module = user.UserRole.ToLower();

                    ViewBag.Username = user.UserDisplayName;

                    log.Info($"{DateTime.Now.ToString()} - Logged in the User {useremail}");

                    return View();
                }

                ViewBag.ErrorTitle = "Unable to retreive user email";
                ViewBag.ExceptionType = "Access Denied";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = "You do not belong to any of the designated permission groups";
                ViewBag.ExceptionType = "Access Denied";
                ViewBag.ErrorDescription = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("Index", "Home");
            }
        }
    }

}

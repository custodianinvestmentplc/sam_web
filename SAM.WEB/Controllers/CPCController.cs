using SAM.NUGET.Domain.Dtos;
using SAM.NUGET.Domain.Options;
using SAM.NUGET.Models;
using SAM.NUGET.Payloads;
using SAM.NUGET.Services;
using SAM.NUGET.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using log4net;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.Services;
using SAM.API.Controllers;

namespace SAM.WEB.Controllers
{
    public class CPCController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;

        public CPCController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices, LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _cpcServices = cpcHubServices;
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
        }

        [HttpGet]
        public Task<IActionResult> CreateProposalPack()
        {
            return GetUserViewAsync();
        }


        [HttpGet]
        public Task<IActionResult> GetDraftProposalPacks()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> SubmittedProposalPacks()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> InboundProposalPacks()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> WIPProposalPacks()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> AcceptedProposalPacks()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> ApprovedProposalPacks()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> UsersProfile()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> RolesSetting()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> Permissions()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> Forms()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> Branches()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> CPCTypes()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> CPCProducts()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> States()
        {
            return GetUserViewAsync();
        }

        [HttpGet]
        public Task<IActionResult> SupportingDocuments()
        {
            return GetUserViewAsync();
        }

        public async Task<IActionResult> GetUserViewAsync()
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

                    var _permissions = await ControllerHelper.Authorization("new_PP", useremail, _configuration, _httpClientFactory);

                    ViewBag.CanViewPermission = _permissions.Exists(permission => permission.ToLower() == "can_view");

                    if (!ViewBag.CanViewPermission) TempData["Warning"] = "View New Proposal Form \n You do not have permission to view New Proposal Form";

                    else
                    {
                        ViewBag.Branches = await ControllerHelper.GetAllBranches( _configuration, _httpClientFactory);

                        ViewBag.CanEditPermission = _permissions.Exists(permission => permission.ToLower() == "can_edit");

                        if (!ViewBag.CanEditPermission) TempData["Warning"] = "Create Proposal Pack \n You do not have permission to create Proposal Pack"; 
                    }

                    ViewBag.Module = user.UserRole.ToLower();

                    ViewBag.Username = user.UserDisplayName;

                    log.Info($"{DateTime.Now.ToString()} - Logged in the User {useremail}");

                    return View();
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
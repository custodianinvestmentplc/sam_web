
using SAM.NUGET.Domain.Dtos;
using SAM.NUGET.Domain.Options;
using SAM.NUGET.Models;
using SAM.NUGET.Services;
using SAM.NUGET.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Services;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PermissionsController(IConfiguration configuration, IHttpClientFactory httpClientFactory, LoginConfig config, IUserServices userServices, IAuthProvider authprovider, ICPCHubServices cpcHubServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
            _cpcServices = cpcHubServices;
        }


        [HttpGet]
        public IActionResult Authorization([FromQuery] string form)
        {
            var indexVM = new IndexViewModel();

            try
            {
                var isLoggedIn = User.Identity.IsAuthenticated;

                if (!isLoggedIn)
                {
                    indexVM.ErrorTitle = "Unable to signin user";
                    indexVM.ExceptionType = "Authentication Error";
                    indexVM.ErrorDescription = "";

                    var ex = new Exception(indexVM.ExceptionType);

                    log.Error(DateTime.Now.ToString(), ex);

                    return RedirectToAction("Index", "Home", indexVM);

                }

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var permissionOptions = new PermissionOptions()
                {
                    Form = form,
                    //Permission = permission,
                    UserEmail = userEmail
                };

                var permissions = _cpcServices.GetPermissions(permissionOptions);

                return StatusCode(200, permissions.ToArray());

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "IsReadOnlyCheckError"
                });

            }
        }
    }
}

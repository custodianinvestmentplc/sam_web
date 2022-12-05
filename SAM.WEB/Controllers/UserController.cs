using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SAM.NUGET.ViewModels;
using SAM.NUGET.Models;
using SAM.NUGET.Services;
using Microsoft.AspNetCore.Authorization;
using log4net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Services;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices, LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cpcServices = cpcHubServices;
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var indexVM = new IndexViewModel();

            try
            {
                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                if (!string.IsNullOrEmpty(useremail))
                {
                    var user = _userServices.GetUserByEmail(useremail);

                    return StatusCode(200, user);
                }
                else return StatusCode(500);
            }

            catch (Exception ex)
            { 
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500);
            }
        }
    }

}



//else
//{
//    indexVM.ErrorTitle = "Unable to retreive user email";
//    indexVM.ExceptionType = "Access Denied";

//    return StatusCode(500, indexVM);
//}
//            }

//            catch (Exception ex)
//{
//    indexVM.ErrorTitle = "You do not belong to any of the designated permission groups";
//    indexVM.ExceptionType = "Access Denied";
//    indexVM.ErrorDescription = ex.Message;

//    log.Error(DateTime.Now.ToString(), ex);

//    return StatusCode(500, indexVM);
//}
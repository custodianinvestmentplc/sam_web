using SAM.WEB.Domain.Dtos;
using SAM.WEB.Models;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
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
using Microsoft.Extensions.Logging;
using log4net;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using SAM.WEB.Errors;
using System.Net;

namespace SAM.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly LoginConfig _config;
        private readonly IAuthProvider _authProvider;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController(IConfiguration configuration, IHttpClientFactory httpClientFactory, LoginConfig config, IAuthProvider authprovider)
        {
            _config = config;
            _authProvider = authprovider;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Index(IndexViewModel indexViewModel)
        {
            ViewBag.ShowLayout = false;

            return View(indexViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            var isLoggedIn = User.Identity.IsAuthenticated;

            if (isLoggedIn)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(Login), "Home")
                });
            }

            var urlString = $"{_config.Instance}/{_config.TenantId}/oauth2/v2.0/authorize?client_id={_config.ClientId}&response_type=code&redirect_uri={_config.CallbackPath}&response_mode=form_post&scope={HttpUtility.UrlEncode("https://graph.microsoft.com/User.Read")}&state=12345&nonce=678910&prompt=select_account";
            var url = new Uri(urlString).ToString();

            return Redirect(url);
        }

        public async Task<IActionResult> Callback(string code, string state, string error)
        {
            //var indexVM = new IndexViewModel();

            try
            {
                var token = _authProvider.AcquireAdToken(code);
                var adUser = _authProvider.GetLoggedInUser(token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, adUser.UserPrincipalName)
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, props);

                return RedirectToAction("Modules", "Home");

            }
            catch (Exception ex)
            {
                TempData["ErrorTitle"] = "Unable to signin user";
                TempData["ExceptionType"] = "Authentication Error";
                TempData["ErrorDescription"] = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public IActionResult Modules()
        {
            //var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                //var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:AuthUrl");

                var useremail = HttpContext.Session.Get<string>("userEmail");

                if (!string.IsNullOrEmpty(useremail))
                {
                    //var fetchUrl = $"{url}?useremail={useremail}";

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

                        log.Info($"{DateTime.Now.ToString()} - Logged in the User {useremail}");

                        return View(Url.Content("/Views/Home/AppList.cshtml"));
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

        [HttpGet]
        public IActionResult Error([FromQuery] string errorcode, string message, string detail)
        {
            TempData["Error"] = $"Error message\r\n {message}.";

            ViewBag.ShowLayout = false;

            return View(new ApiExceptionsResponse(errorcode, message, detail));
        }
    }
}



//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//public IActionResult Error()
//{
//    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//}

//if (string.IsNullOrWhiteSpace(indexVM.ErrorTitle) && string.IsNullOrWhiteSpace(indexVM.ErrorDescription))

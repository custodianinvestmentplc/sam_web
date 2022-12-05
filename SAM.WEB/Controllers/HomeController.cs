using SAM.NUGET.Domain.Dtos;
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
using Microsoft.Extensions.Logging;
using log4net;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.Services;

namespace SAM.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController(IConfiguration configuration, IHttpClientFactory httpClientFactory, LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _config = config;
            _userServices = userServices;
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
            var indexVM = new IndexViewModel();

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
                indexVM.ErrorTitle = "Unable to signin user";
                indexVM.ExceptionType = "Authentication Error";
                indexVM.ErrorDescription = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("Index", "Home", indexVM);
            }
        }


        [Authorize]
        public async Task<IActionResult> Modules()
        {
            var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:AuthUrl");

                if (!string.IsNullOrEmpty(useremail))
                {
                    //var fetchUrl = $"{url}?useremail={useremail}";

                    var query = new Dictionary<string, string>()
                    {
                        ["useremail"] = useremail
                    };

                    var uri = QueryHelpers.AddQueryString(url, query);

                    var user = await DataServices<UserRegisterDto>.GetPayload(uri, _httpClientFactory);

                    ViewBag.Module = user.UserRole.ToLower();

                    ViewBag.Username = user.UserDisplayName;

                    log.Info($"{DateTime.Now.ToString()} - Logged in the User {useremail}");

                    return View(Url.Content("/Views/Home/AppList.cshtml"));
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



//if (string.IsNullOrWhiteSpace(indexVM.ErrorTitle) && string.IsNullOrWhiteSpace(indexVM.ErrorDescription))

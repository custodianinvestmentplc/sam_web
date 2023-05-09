using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Options;
using SAM.WEB.Models;
using SAM.WEB.Services;

namespace SAM.WEB.MiddleWares
{
    public class UserAuthMiddleware
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly LoginConfig _config;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private RequestDelegate next;
        private readonly UserRegisterDto _user;

        public UserAuthMiddleware(RequestDelegate nextDelegate, IConfiguration Configuration, IHttpClientFactory httpClientFactory, LoginConfig config, IOptions<UserRegisterDto> userOpt)
        {
            configuration = Configuration;
            this.httpClientFactory = httpClientFactory;
            _config = config;
            _user = userOpt.Value;
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path != "/" && context.Request.Path != "/Home/Index" && context.Request.Path != "/Home/Login" && context.Request.Path != "/Home/Callback")
            {
                if (!context.User.Identities.Any(identity => identity.IsAuthenticated))
                {
                    var urlString = $"{_config.Instance}/{_config.TenantId}/oauth2/v2.0/authorize?client_id={_config.ClientId}&response_type=code&redirect_uri={_config.CallbackPath}&response_mode=form_post&scope={HttpUtility.UrlEncode("https://graph.microsoft.com/User.Read")}&state=12345&nonce=678910&prompt=select_account";
                    var url = new Uri(urlString).ToString();

                    //var url = $"{context.Request.Host}";
                    //var url = $@"https://localhost:7178/";

                    //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    context.Response.Redirect(url);
                }

                else await next(context);
            }
            else await next(context);
        }
    }
}


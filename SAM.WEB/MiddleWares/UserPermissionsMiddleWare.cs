using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAM.WEB.Controllers;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Options;
using SAM.WEB.Models;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace SAM.WEB.MiddleWares
{
    public class UserPermissionsMiddleWare
    {
        private RequestDelegate next;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly UserRegisterDto _user;

        public UserPermissionsMiddleWare(RequestDelegate nextDelegate, IConfiguration configuration, IHttpClientFactory httpClientFactory, IOptions<UserRegisterDto> userOpt)
        {
            next = nextDelegate;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _user = userOpt.Value;
        }
        
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path != "/" && context.Request.Path != "/Home/Index" && context.Request.Path != "/Home/Login" && context.Request.Path != "/Home/Callback" && context.Request.Path != "/Home/Error" && context.Request.Path != "/Home/images/ms_logo.png")
            {
                if (string.IsNullOrWhiteSpace(context.Session.Get<string>("userEmail")) || context.Session.Get<UserRegisterDto>("UserRegisterDto") == null || context.Session.Get<List<PermissionOptions>>("UserPermissions") == null || context.Session.Get<List<PermissionOptions>>("UserPermissions").Count() <= 0)
                {
                    try
                    {
                        var userEmail = ControllerHelper.GetAppUserFromHttpContext(context);

                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            context.Session.Set("userEmail", userEmail);

                            //var url = _configuration.GetValue<string>("AppSettings:AuthUrl");
                            //var query = new Dictionary<string, string>()
                            //{
                            //    ["useremail"] = useremail
                            //};
                            //var uri = QueryHelpers.AddQueryString(url, query);
                            //var user = await DataServices<UserRegisterDto>.GetPayload(uri, _httpClientFactory);

                            if (_user != null)
                            {
                                context.Session.Set("UserRegisterDto", _user);

                                var _permissions = await ControllerHelper.Authorization(userEmail, _configuration, _httpClientFactory);

                                if (_permissions != null && _permissions.Count() > 0)
                                {
                                    context.Session.Set("UserPermissions", _permissions);

                                    await next(context);
                                }
                                else throw new Exception("No permission found of logged-in user.");
                            }
                            else throw new Exception("User payload is empty.");
                        }
                        else throw new Exception("No usaer email found.");
                    }
                    catch { throw; }
                }
                else await next(context);
            }
            else await next(context);
        }
    }
}

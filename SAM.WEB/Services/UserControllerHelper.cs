using System;
using System.Reflection;
using SAM.WEB.Models;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SAM.WEB.Domain.Dtos;
using log4net;

namespace SAM.WEB.Services
{
    public static class UserControllerHelper
    {
        public static async Task<IndexViewModel> GetUserModuleAsync(string url, IHttpClientFactory httpClientFactory)
        {
            try
            {
                var indexVM = new IndexViewModel();

                var _client = httpClientFactory.CreateClient("client");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();

                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var user = JsonSerializer.Deserialize<UserRegisterDto>(stringData, option);

                    indexVM.Module = user.UserRole.ToLower();

                    indexVM.UserName = user.UserDisplayName;
                }
                else if(response.StatusCode.ToString() == "InternalServerError")
                {
                    indexVM.ErrorTitle = "Server side error";
                    indexVM.ExceptionType = "Access Denied";
                }

                else
                {
                    indexVM.ErrorTitle = "Error 400";
                    indexVM.ExceptionType = "Bad Request";
                }

                return indexVM;
            }
            catch { throw; }
        }
    }
}
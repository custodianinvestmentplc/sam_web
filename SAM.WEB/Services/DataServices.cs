using System;
using System.Reflection;
using SAM.NUGET.Models;
using SAM.NUGET.Services;
using SAM.NUGET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SAM.NUGET.Domain.Dtos;
using log4net;
using System.Security.Policy;
using System.Text;

namespace SAM.WEB.Services
{
    public static class DataServices<T> where T : class
    {

        public static async Task<T> GetPayload (string url, IHttpClientFactory httpClientFactory)
        {
            try
            {
                var _client = httpClientFactory.CreateClient("client");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();

                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<T>(stringData, option);
                }

                else if (response.StatusCode.ToString() == "InternalServerError") throw new Exception("Server side error");

                else throw new Exception("Bad Request");
            }

            catch { throw; }
        }


        public static async Task<T> PostPayload<U> (U payload, string url, IHttpClientFactory httpClientFactory)
        {
            try
            {
                var stringData = JsonSerializer.Serialize(payload);
                var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                var _client = httpClientFactory.CreateClient("client");

                var response = await _client.PostAsync(url, contentData);

                if (response.IsSuccessStatusCode)
                {
                    var returnData = await response.Content.ReadAsStringAsync();

                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<T>(returnData, option);
                }

                else if (response.StatusCode.ToString() == "InternalServerError") throw new Exception("Server side error");

                else throw new Exception("Bad Request");
            }

            catch { throw; }
        }
    }
}
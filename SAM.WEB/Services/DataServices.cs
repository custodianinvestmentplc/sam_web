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
using System.Security.Policy;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Mime;
//using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.IO;

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

        //public static async Task<T> PostFilePayload(IFormFileCollection payload, string url, IHttpClientFactory httpClientFactory)
        //{
        //    try
        //    {
        //        //var stringData = JsonSerializer.Serialize(payload);
        //        //var contentType = new MediaTypeWithQualityHeaderValue("multipart/form-data");
        //        //var contentData = new StringContent(stringData, Encoding.UTF8, "multipart/form-data");

        //        //byte[] Data;

        //        //var dataByte = payload[0];

        //        //using (var ms = new MemoryStream())
        //        //{
        //        //    dataByte.CopyTo(ms);
        //        //    Data = ms.ToArray();
        //        //}

        //        var stringData = JsonSerializer.Serialize(payload);

        //        var _client = httpClientFactory.CreateClient("client");

        //        var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

        //        var response = await _client.PostAsync(url, contentData);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var returnData = await response.Content.ReadAsStringAsync();

        //            var option = new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            };

        //            return JsonSerializer.Deserialize<T>(returnData, option);
        //        }

        //        else if (response.StatusCode.ToString() == "InternalServerError") throw new Exception("Server side error");

        //        else throw new Exception("Bad Request");
        //    }

        //    catch { throw; }
        //}

    }
}


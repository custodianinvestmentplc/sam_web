using SAM.WEB.Domain.Dtos;
using SAM.WEB.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.ViewModels;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace SAM.WEB.Services
{
    public static class ControllerHelper
    {
        public static string GetAppUserFromHttpContext(HttpContext context)
        {
            try
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var userclaims = context.User.Claims.ToList();
                    var useremail = userclaims.Find(x => x.Type == ClaimTypes.Email).Value.ToString();

                    return useremail;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<IFormFile> CreateFormFileAsync(SupportingDocFile sFile, string uploads)
        {
            var fileNameWithPath = uploads + sFile.FileName;
            IFormFile file = null;

            using (var fs = new FileStream(fileNameWithPath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    await fs.CopyToAsync(ms);

                    file = new FormFile(ms, 0, ms.ToArray().Length, sFile.Name, sFile.FileName);
                }
            }

            return file;
        }

        public static void DeleteFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }

        public static void SaveFileToDirectory(IFormFile file, string filename)
        {
            using (FileStream fs = System.IO.File.Create(filename))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        public static void SaveFileToDirectory(byte[] file, string filename)
        {
            System.IO.File.WriteAllBytes(filename, file);
        }

        public static async Task<List<string>> Authorization(string form, string userEmail, IConfiguration _configuration, IHttpClientFactory _httpClientFactory)
        {
            var indexVM = new IndexViewModel();

            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:PermissionsUrl");

                //var query = new Dictionary<string, string>()
                //{
                //    ["form"] = form,
                //    ["useremail"] = userEmail,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //return await DataServices<List<string>>.GetPayload(uri, _httpClientFactory);

                return new List<string>
                {
                    "can_view",
                    "can_edit"
                };
            }
            catch { throw; }
        }

        public static async Task<List<CPCBranchDto>> GetAllBranches(IConfiguration _configuration, IHttpClientFactory _httpClientFactory)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAllBranches");

                return await DataServices<List<CPCBranchDto>>.GetPayload(url, _httpClientFactory);
            }
            catch { throw; }
        }
    }
}

























//public static SupportingDocFile HandleFormFile(IFormFile file, string uploads, string refNbr, string contentTypeCode, string docType, string docFileName)
//{
//    var supportingFile = new SupportingDocFile();

//    var filename = uploads + docFileName;

//    if (System.IO.File.Exists(filename))
//    {
//        System.IO.File.Delete(filename);
//    }

//    using (FileStream fs = System.IO.File.Create(filename))
//    {
//        file.CopyTo(fs);
//        fs.Flush();
//    }

//    supportingFile.FileName = docFileName;
//    supportingFile.Name = file.Name;
//    supportingFile.ContentType = file.ContentType;
//    supportingFile.Size = file.Length;
//    supportingFile.ContentTypeCode = contentTypeCode;
//    supportingFile.ReferenceNbr = refNbr;
//    supportingFile.DocType = docType;

//    return supportingFile;
//}



//supportingFile.FileUrl = uploads;
//supportingFile.ContentDispositionHeaderValue = file.ContentDisposition;

//var fileNameWithPath = @"/Users/apple/Desktop/CPC(3)/CPCHub/app-login/wwwroot/cpc_files/gettyimages-1023706338-2048x2048.jpg";
//var fileNameWithPath = @"https://localhost:7178//Users/apple/Desktop/CPC(3)/CPCHub/app-login/wwwroot/cpc_files/gettyimages-1023706338-2048x2048.jpg";
//using (var stream = new MemoryStream())
//{
//    file = new FormFile(stream, 0, ad.ImageInByteArray.Length, "name", "fileName");
//}

//using var ms = new MemoryStream();
//file.CopyTo(ms);
//var data = ms.ToArray();
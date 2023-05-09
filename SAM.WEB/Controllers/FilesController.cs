using SAM.WEB.Domain.Dtos;
using SAM.WEB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FilesController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cpcServices = cpcHubServices;
        }

        [HttpGet]
        [Route("cpc")]
        public IActionResult GetAllCpcFiles()
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:GetAllCpcFiles");

                //var model = await DataServices<List<CpcFileDto>>.GetPayload(url, _httpClientFactory);

                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var model = new List<CpcFileDto>
                    {
                        new CpcFileDto
                        {
                             FileName = "Means of Identification",
                             FileCode = "id_file",
                             CreatedBy = useremail,
                             CreateDate = new DateTime(2022, 01, 06),
                        },
                        new CpcFileDto
                        {
                             FileName = "Utility Bill",
                             FileCode = "utility_file",
                             CreatedBy = useremail,
                             CreateDate = new DateTime(2022, 01, 06),
                        },
                    };

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = $"GetCpcFilesError: {ex.Message}",
                    ExceptionType = "DataRetrievalError"
                });
            }
        }
    }
}

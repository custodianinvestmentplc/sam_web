using SAM.WEB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportingController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ReportingController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IReportService reportService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("modules/{id}/reports")]
        public async Task<IActionResult> GetReportList([FromRoute] int id)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetReportList");

                var query = new Dictionary<string, string>()
                {
                    ["id"] = id.ToString(),
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var model = await DataServices<List<ReportItemResource>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "ReportListRetrievalError"
                });
            }
        }
    }
}

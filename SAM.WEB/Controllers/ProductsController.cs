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
    public class ProductsController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cpcServices = cpcHubServices;
        }

        [HttpGet]
        [Route("cpc")]
        public async Task<IActionResult> GetAllCpcProducts()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAllCpcProducts");

                var model = await DataServices<List<CpcProductDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = $"GetCpcProductError: {ex.Message}",
                    ExceptionType = "DataRetrievalError"
                });
            }
        }
    }
}

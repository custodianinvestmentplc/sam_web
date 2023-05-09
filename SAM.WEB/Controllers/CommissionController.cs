using SAM.WEB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAM.WEB.Resources;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommissionController : ControllerBase
    {
        private readonly IAgentServices _agentServices;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CommissionController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAgentServices agentServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _agentServices = agentServices;
        }

        [HttpGet]
        [Route("orc/scenarios")]
        public async Task<IActionResult> GetOrcScenariosAsync()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetOrcScenarios");

                var model = await DataServices<List<OrcScenarioDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "OrcScenarioRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("orc/scenarios/{scenarioId}")]
        public async Task<IActionResult> GetOrcScenarioDetailsAsync([FromRoute] string scenarioId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetOrcScenarioDetails");

                var model = await DataServices<OrcScenarioDetailResource>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "OrcScenarioDetailsRetrievalError"
                });
            }
        }
    }
}

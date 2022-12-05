using SAM.NUGET.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.NUGET.Domain.Dtos;
using SAM.WEB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SAM.NUGET.Resources;

namespace SAM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesTeamController : ControllerBase
    {
        private readonly IAgentServices _agentServices;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SalesTeamController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAgentServices agentServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _agentServices = agentServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAllAgents");

                var model = await DataServices<List<SalesTeamMemberDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SalesTeamRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("agents/{id}")]
        public async Task<IActionResult> GetAgentDetails([FromRoute] string id)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAgentDetails");

                var uri = $"{url}/{id}";

                var model = await DataServices<AgentResource>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SalesAgentRetrievalError"
                });
            }
        }
    }
}

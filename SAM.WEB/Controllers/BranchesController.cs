using SAM.NUGET.Domain.Dtos;
using SAM.NUGET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Services;
using System.Threading.Tasks;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BranchesController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cpcServices = cpcHubServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranchesAsync()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAllBranches");

                var model = await DataServices<List<CPCBranchDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcBranchesRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("branchesandroles")]
        public async Task<IActionResult> GetBranchesAndRolesAsync()
        {
            try
            { 
                var url = _configuration.GetValue<string>("AppSettings:GetBranchesAndRoles");

                var model = await DataServices<CPCBranchAndRolesDto>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcBranchesAndRolesRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("states")]
        public async Task<IActionResult> GetAllStatesAsync()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAllStates");

                var model = await DataServices<List<StatesInNigeriaDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = $"CpcStateRetrievalError: { ex.Message }",
                    ExceptionType = "DataRetrievalError"
                });
            }
        }
    }
}

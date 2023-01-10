using SAM.WEB.Domain.Dtos;
using SAM.WEB.Services;
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
                //var url = _configuration.GetValue<string>("AppSettings:GetAllBranches");

                //var model = await DataServices<List<CPCBranchDto>>.GetPayload(url, _httpClientFactory);

                var model = new List<CPCBranchDto>
                {
                    new CPCBranchDto
                    {
                        RowId = 1,
                        BranchEmailAddress = "testEmail@test1.com",
                        LocalSystemCode = "1",
                        BranchName = "Branch1",
                        SourceSystemCode = "testBranchSourceCode1",
                    },
                    new CPCBranchDto
                    {
                        RowId = 2,
                        BranchEmailAddress = "testEmail@test2.com",
                        LocalSystemCode = "2",
                        BranchName = "Branch2",
                        SourceSystemCode = "2",
                    },
                };

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
                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:GetAllStates");

                //var model = await DataServices<List<StatesInNigeriaDto>>.GetPayload(url, _httpClientFactory);

                var model = new List<StatesInNigeriaDto>
                {
                    new StatesInNigeriaDto
                    {
                        StateName = "State1",
                        StateCode = "1",
                        CreateDate = new DateTime(2022,01,06),
                        CreatedBy = useremail,
                    },
                    new StatesInNigeriaDto
                    {
                        StateName = "State2",
                        StateCode = "1",
                        CreateDate = new DateTime(2022,01,06),
                        CreatedBy = useremail,
                    },
                };

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = $"CpcStateRetrievalError: {ex.Message}",
                    ExceptionType = "DataRetrievalError"
                });
            }
        }
    }
}

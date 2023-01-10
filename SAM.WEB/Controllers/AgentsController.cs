using SAM.WEB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Linq;
using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentServices _agentServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AgentsController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAgentServices agentServices)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _agentServices = agentServices;
        }

        [HttpGet]
        [Route("find")]
        public async Task<IActionResult> FindAgentByNameAsync([FromQuery] string searchTerm, string Ref)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:AgentsUrl");

                //var query = new Dictionary<string, string>()
                //{
                //    ["searchTerm"] = searchTerm,
                //    ["Ref"] = Ref,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<List<SalesTeamMemberDto>>.GetPayload(uri, _httpClientFactory);

                var model = new List<SalesTeamMemberDto>()
                {
                    new SalesTeamMemberDto
                    {
                        AgentSystemCode = "test1",
                        BusinessCode = "test",
                         FullName = "AgentTest1",
                         SalesLevel = "test",
                    },
                    new SalesTeamMemberDto
                    {
                        AgentSystemCode = "test2",
                        BusinessCode = "test",
                         FullName = "AgentTest2",
                         SalesLevel = "test",
                    },
                };

                var modelArray = new List<SalesTeamMemberArrayDto>();

                if (model != null && model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        var recordArray = new List<string>();

                        recordArray.Add(item.FullName);
                        recordArray.Add(item.SalesLevel);
                        recordArray.Add(item.BusinessCode);

                        modelArray.Add(new SalesTeamMemberArrayDto
                        {
                            ResourceArray = recordArray.ToArray()
                        });
                    }
                }

                return StatusCode(200, new
                {
                    RecordCount = modelArray.Count,
                    results = modelArray.ToArray()
                });

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcFindAgentError"
                });
            }
        }
    }
}

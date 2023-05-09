using SAM.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SAM.WEB.Resources;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupSynergyServices _groupService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GroupController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGroupSynergyServices groupservice)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _groupService = groupservice;
        }

        [HttpGet]
        [Route("customers/search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:Search");

                var query = new Dictionary<string, string>()
                {
                    ["searchTerm"] = searchTerm,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var model = await DataServices<List<GroupCustomerSearchResultDto>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GroupSearchError"
                });
            }
        }

        [HttpGet]
        [Route("customers/{fuzzykey}")]
        public async Task<IActionResult> SearchByFuzzyKey([FromRoute] int fuzzykey)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:SearchByFuzzyKey");

                var query = new Dictionary<string, string>()
                {
                    ["fuzzykey"] = fuzzykey.ToString(),
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var model = await DataServices<List<GroupCustomerSearchDetailsDto>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GroupSearchDetailError"
                });
            }
        }

        [HttpGet]
        [Route("customers")]
        public async Task<IActionResult> FetchAllCustomers()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchAllCustomers");

                var model = await DataServices<List<GroupCustomerItemDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AllCustomersRetrievalError"
                });
            }
        }
    }
}

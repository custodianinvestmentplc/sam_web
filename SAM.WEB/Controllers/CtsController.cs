using SAM.WEB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using SAM.WEB.Payloads.Cts;
using System.Linq;
using System.Web;
using SAM.WEB.Domain.RequestModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Services;
using System.Collections.Generic;
using SAM.WEB.Domain.Dtos.Cts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using SAM.WEB.Domain.Options;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CtsController : ControllerBase
    {
        private readonly ICtsService _ctsService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CtsController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICtsService ctsService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _ctsService = ctsService;
        }

        [HttpGet]
        [Route("tickets/new")]
        public async Task<IActionResult> FetchNewTickets()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchNewTickets");

                var model = await DataServices<List<NewCtsTicket>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetNewTicketsError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/{ticketNumber}/activity-log")]
        public async Task<IActionResult> TicketActivityLog([FromRoute] string ticketNumber)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:TicketActivityLog");

                var uri = $"{url}/{ticketNumber}/activity-log";

                var model = await DataServices<List<ActivityLog>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetTicketActivityLogError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/search")]
        public async Task<IActionResult> FetchNewTickets([FromQuery] string searchTerm)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchNewTickets");

                var uri = $"{url}/{searchTerm}";

                var model = await DataServices<List<NewCtsTicket>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetNewTicketsError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/{ticketNumber}")]
        public async Task<IActionResult> GetTicketDetailsById([FromRoute] string ticketNumber)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetTicketDetailsById");

                var uri = $"{url}/{ticketNumber}";

                var model = await DataServices<TicketDetail>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetTicketError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/{ticketNumber}/categorization")]
        public async Task<IActionResult> GetTicketCategorization([FromRoute] string ticketNumber)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetTicketCategorization");

                var uri = $"{url}/{ticketNumber}/categorization";

                var model = await DataServices<TicketCategorization>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetTicketCategorizationError"
                });
            }
        }

        [HttpGet]
        [Route("companies")]
        public async Task<IActionResult> FetchAllCompanies()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchAllCompanies");

                var model = await DataServices<List<Companies>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetCompaniesError"
                });
            }
        }

        [HttpGet]
        [Route("service-types")]
        public async Task<IActionResult> FetchServiceTypes()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchServiceTypes");

                var model = await DataServices<List<ServiceType>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeError"
                });
            }
        }

        [HttpGet]
        [Route("service-types/{serviceTypeId}")]
        public async Task<IActionResult> GetServiceType([FromRoute] int serviceTypeId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetServiceType");

                var uri = $"{url}/{serviceTypeId}";

                var model = await DataServices<ServiceTypeDetailDto>.GetPayload(uri, _httpClientFactory);

                if (model != null)
                {
                    return StatusCode(200, model);
                }

                return StatusCode(404, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeError"
                });
            }
        }

        [HttpGet]
        [Route("service-types/{serviceTypeId}/categories")]
        public async Task<IActionResult> FetchCategoriesByServiceTypeId([FromRoute] int serviceTypeId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchCategoriesByServiceTypeId");

                var uri = $"{url}/{serviceTypeId}/categories";

                var model = await DataServices<List<ServiceTypeCategory>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("categories/{categoryId}/sub-categories")]
        public async Task<IActionResult> FetchSubcategoriesByCategoryId([FromRoute] int categoryId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchSubcategoriesByCategoryId");

                var uri = $"{url}/{categoryId}/sub-categories";

                var model = await DataServices<List<ServiceTypeSubCategory>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("categories/{categoryId}")]
        public async Task<IActionResult> GetCategoryDetail([FromRoute] int categoryId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetCategoryDetail");

                var uri = $"{url}/{categoryId}";

                var model = await DataServices<CategoryDetailDto>.GetPayload(uri, _httpClientFactory);

                if (model != null)
                {
                    return StatusCode(200, model);
                }

                return StatusCode(404, new
                {
                    ErrorDescription = $"Could not find the Category with Id { categoryId } in the database",
                    ExceptionType = "InvalidCategoryId"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetCategoryError"
                });
            }
        }


        [HttpGet]
        [Route("sub-categories/{subcategoryId}/subcategory-items")]
        public async Task<IActionResult> FetchSubcategoryItemIdBySubcategoryId([FromRoute] int subcategoryId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchSubcategoryItemIdBySubcategoryId");

                var uri = $"{url}/{subcategoryId}/subcategory-items";

                var model = await DataServices<List<ServiceTypeSubCategoryItem>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("sub-categories/{subcategoryId}")]
        public async Task<IActionResult> GetSubcategoryDetail([FromRoute] int subcategoryId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetSubcategoryDetail");

                var uri = $"{url}/{subcategoryId}";

                var model = await DataServices<SubCategoryDetailDto>.GetPayload(uri, _httpClientFactory);

                if (model != null)
                {
                    return StatusCode(200, model);
                }

                return StatusCode(404, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("technicians")]
        public async Task<IActionResult> GetAllTechnicians()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetAllTechnicians");

                var model = await DataServices<List<TechnicianUser>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "FetchTechniciansError"
                });
            }
        }

        [HttpPost]
        [Route("assign-new-ticket")]
        public async Task<IActionResult> AllocateAndAssignNewTicket([FromBody] AssignNewTicket payload)
        {
            try
            {
                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = useremail;

                var url = _configuration.GetValue<string>("AppSettings:AllocateAndAssignNewTicket");

                await DataServices<string>.PostPayload<AssignNewTicket>(payload, url, _httpClientFactory);

                return StatusCode(201, new 
                {
                    ErrorMessage = "Operation Successful"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "FetchTechniciansError"
                });
            }
        }
    }
}

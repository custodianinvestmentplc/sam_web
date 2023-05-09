using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Options;
using SAM.WEB.Models;
using SAM.WEB.Payloads;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using SAM.WEB;
using SAM.WEB.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Route("api/proposal-pack")]
    [ApiController]
    [Authorize]
    public class ProposalPackController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICPCHubServices _cpcServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProposalPackController(IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices, IWebHostEnvironment webHostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _configuration = configuration;
            _cpcServices = cpcHubServices;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProposalPack([FromBody] NewProposalPack payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var packOptions = new CreateProposalPackOptions
                {
                    InitiatingAgentCode = payload.AgentCode,
                    CallerEmail = userEmail,
                    InitiatingBranchCode = payload.BranchCode,
                    ProposalTitle = payload.Title,
                    ProposalPackType = payload.ProposalType
                };

                var url = _configuration.GetValue<string>("AppSettings:CreatePPUrl");

                var refNbr = await DataServices<string>.PostPayload<CreateProposalPackOptions>(packOptions, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    ProposalReferenceNbr = refNbr
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CreateProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("authorization")]
        public async Task<IActionResult> Authorization([FromQuery] string form)
        {
            var indexVM = new IndexViewModel();

            try
            {
                var isLoggedIn = User.Identity.IsAuthenticated;

                if (!isLoggedIn)
                {
                    indexVM.ErrorTitle = "Unable to signin user";
                    indexVM.ExceptionType = "Authentication Error";
                    indexVM.ErrorDescription = "";

                    var ex = new Exception(indexVM.ExceptionType);

                    log.Error(DateTime.Now.ToString(), ex);

                    return RedirectToAction("Index", "Home", indexVM);

                }

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:PermissionsUrl");

                var query = new Dictionary<string, string>()
                {
                    ["form"] = form,
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var permissions = await DataServices<List<string>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, permissions.ToArray());

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetPermissionsError"
                });

            }
        }

        [HttpPost]
        [Route("adduser")]
        public async Task<IActionResult> AddNewUser([FromBody] UserProfile payload)
        {
            try
            {
                var addedBy = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var payloadRef = new UserProfileOptions()
                {
                    UserEmail = payload.UserEmail,
                    BranchCode = payload.BranchCode,
                    RoleCode = payload.RoleCode,
                    AddedBy = addedBy,
                    DisplayName = payload.DisplayName,
                    AgentDescription = payload.AgentDescription,
                    AgentPosition = payload.AgentPosition
                };

                payloadRef.BranchCode = string.IsNullOrWhiteSpace(payloadRef.BranchCode) ? "1003" : payloadRef.BranchCode;
                //payloadRef.AgentCode = string.IsNullOrWhiteSpace(payloadRef.AgentCode) ? "NA" : payloadRef.AgentCode;

                var url = _configuration.GetValue<string>("AppSettings:AddNewUser");

                var refNbr = await DataServices<string>.PostPayload<UserProfileOptions>(payloadRef, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    UserReferenceNbr = refNbr
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AddNewUserError"
                });
            }
        }

        [HttpPost]
        [Route("add-role")]
        public async Task<IActionResult> AddRoleSettings([FromBody] RoleSettings payload)
        {
            try
            {
                var addedBy = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var payloadRef = new AddNewRoleOptions()
                {
                    Role = payload.Role,
                    AddedBy = addedBy
                };

                var url = _configuration.GetValue<string>("AppSettings:AddRoleSettings");

                var refNbr = await DataServices<string>.PostPayload<AddNewRoleOptions>(payloadRef, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    RoleReferenceNbr = refNbr
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AddRoleSettingsError"
                });
            }
        }

        [HttpPost]
        [Route("add-template")]
        public async Task<IActionResult> AddTemplateSettings([FromBody] NewTemplateSettings payload)
        {
            try
            {
                var addedBy = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var payloadRef = new AddNewTemplateOptions()
                {
                    RefTypeRefCode = payload.RefTypeRefCode,
                    Template = payload.Template,
                    TemplateType = payload.TemplateType,
                    TemplateDesc = payload.TemplateDesc,
                    TemplateEmail = payload.TemplateEmail,
                    TemplateShortDesc = payload.TemplateShortDesc,
                    RefTypeRefCodeDesc = payload.RefTypeRefCodeDesc,
                    ClassCode = payload.ClassCode,
                    ClassCodeDesc = payload.ClassCodeDesc,
                    AddedBy = addedBy
                };

                var url = _configuration.GetValue<string>("AppSettings:AddTemplateSettings");

                var refNbr = await DataServices<string>.PostPayload<AddNewTemplateOptions>(payloadRef, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    TemplateReferenceNbr = refNbr
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Add{payload.Template}SettingsError"
                });
            }
        }

        [HttpPost]
        [Route("edit-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfile payload)
        {
            try
            {
                var updatedBy = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var payloadRef = new UserProfileOptions()
                {
                    ReferenceNumber = payload.ReferenceNumber,
                    UserEmail = payload.UserEmail,
                    BranchCode = payload.BranchCode,
                    RoleCode = payload.RoleCode,
                    AddedBy = updatedBy,
                    DisplayName = payload.DisplayName,
                    AgentDescription = payload.AgentDescription,
                    AgentPosition = payload.AgentPosition
                };

                payloadRef.BranchCode = string.IsNullOrWhiteSpace(payloadRef.BranchCode) ? "1003" : payloadRef.BranchCode;
                payloadRef.AgentDescription = string.IsNullOrWhiteSpace(payloadRef.AgentDescription) ? null : payloadRef.AgentDescription;
                payloadRef.AgentPosition = string.IsNullOrWhiteSpace(payloadRef.AgentPosition) ? null : payloadRef.AgentPosition;

                var url = _configuration.GetValue<string>("AppSettings:UpdateUser");

                var res = await DataServices<string>.PostPayload<UserProfileOptions>(payloadRef, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "UpdatedUserProfileError"
                });
            }
        }

        [HttpPost]
        [Route("edit-role")]
        public async Task<IActionResult> EditRoleSettings([FromBody] RoleSettings payload)
        {
            try
            {
                var updatedBy = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var payloadRef = new AddNewRoleOptions()
                {
                    ReferenceNumber = payload.ReferenceNumber,
                    Role = payload.Role,
                    AddedBy = updatedBy
                };

                var url = _configuration.GetValue<string>("AppSettings:EditRoleSettings");

                var res = await DataServices<string>.PostPayload<AddNewRoleOptions>(payloadRef, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "UpdatedRoleSettingsError"
                });
            }
        }

        [HttpPost]
        [Route("edit-template")]
        public async Task<IActionResult> EditTemplateSettings([FromBody] NewTemplateSettings payload)
        {
            try
            {
                var updatedBy = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var payloadRef = new AddNewTemplateOptions()
                {
                    ReferenceNumber = payload.ReferenceNumber,
                    TemplateType = payload.TemplateType,
                    Template = payload.Template,
                    TemplateDesc = payload.TemplateDesc,
                    TemplateEmail = payload.TemplateEmail,
                    TemplateShortDesc = payload.TemplateShortDesc,
                    RefTypeRefCodeDesc = payload.RefTypeRefCodeDesc,
                    RefTypeRefCode = payload.RefTypeRefCode,
                    ClassCode = payload.ClassCode,
                    ClassCodeDesc = payload.ClassCodeDesc,
                    AddedBy = updatedBy
                };

                var url = _configuration.GetValue<string>("AppSettings:EditTemplateSettings");

                var res = await DataServices<string>.PostPayload<AddNewTemplateOptions>(payloadRef, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Updated{payload.Template}SettingsError"
                });
            }
        }

        [HttpGet]
        [Route("reference-nbr")]
        public async Task<IActionResult> GetProposalPack([FromQuery] string refnbr)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetProposalPack");

                var query = new Dictionary<string, string>()
                {
                    ["refnbr"] = refnbr,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var proposalPack = await DataServices<CpcProposalPack>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, proposalPack);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("user-profile")]
        public async Task<IActionResult> GetUserProfile([FromQuery] string refnbr)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetUserProfile");

                var query = new Dictionary<string, string>()
                {
                    ["refnbr"] = refnbr
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var userProfile = await DataServices<UserRegisterDto>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, userProfile);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetUserProfileError"
                });
            }
        }

        [HttpGet]
        [Route("get-role")]
        public async Task<IActionResult> GetRole([FromQuery] string refnbr)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetRole");

                var query = new Dictionary<string, string>()
                {
                    ["refnbr"] = refnbr,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var role = await DataServices<CpcRoleDto>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, role);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetRoleSettingsError"
                });
            }
        }

        [HttpGet]
        [Route("get-template")]
        public async Task<IActionResult> GetTemplate([FromQuery] string refnbr, string templateType)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetTemplate");

                var query = new Dictionary<string, string>()
                {
                    ["refnbr"] = refnbr,
                    ["templateType"] = templateType,
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var template = await DataServices<CpcTemplateDto>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, template);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Get{templateType}SettingsError"
                });
            }
        }

        [HttpGet]
        [Route("users-profile")]
        public async Task<IActionResult> GetUsersProFile()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetUsersProFile");

                var usersProfile = await DataServices<List<UserRegisterDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, usersProfile);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetUserProfilesError"
                });
            }
        }

        [HttpGet]
        [Route("roles-settings")]
        public async Task<IActionResult> GetRolesSettings()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetRolesSettings");

                var roles = await DataServices<List<CpcRoleDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, roles);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcRolesRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("template-settings")]
        public async Task<IActionResult> GetTemplatesSettings([FromQuery] string templateType)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetTemplatesSettings");

                var query = new Dictionary<string, string>()
                {
                    ["templateType"] = templateType,
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var templates = await DataServices<List<CpcTemplateDto>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, templates);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Cpc{templateType}sRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("drafts")]
        public async Task<IActionResult> GetDraftProposalPack()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetDraftProposalPack");

                var proposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetDraftProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("submits")]
        public async Task<IActionResult> GetSubmittedProposalPack()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetSubmittedProposalPack");

                var proposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetSubmittedProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("inbounds")]
        public async Task<IActionResult> GetInboundProposalPacks()
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetInboundProposalPacks");

                var query = new Dictionary<string, string>()
                {
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var proposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetInboundProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("wip")]
        public async Task<IActionResult> GetWIPProposalPacks()
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetWIPProposalPacks");

                var query = new Dictionary<string, string>()
                {
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var proposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetWIPProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("accepted")]
        public async Task<IActionResult> GetAcceptedProposalPacks()
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetAcceptedProposalPacks");

                var query = new Dictionary<string, string>()
                {
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var proposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetAcceptedProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("approved")]
        public async Task<IActionResult> GetApprovedProposalPacks()
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var url = _configuration.GetValue<string>("AppSettings:GetApprovedProposalPacks");

                var query = new Dictionary<string, string>()
                {
                    ["useremail"] = userEmail,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var proposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetApprovedProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("content-types")]
        public async Task<IActionResult> FetchProposalPackContentType()
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:FetchProposalPackContentType");

                var model = await DataServices<List<ProposalPackContentTypeDto>>.GetPayload(url, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetDraftProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("contents")]
        public async Task<IActionResult> GetProposalPackContents([FromQuery] string refNbr)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetProposalPackContents");

                var query = new Dictionary<string, string>()
                {
                    ["refNbr"] = refNbr,
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var model = await DataServices<List<ProposalPackContentDto>>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("contents/delete")]
        public IActionResult DeleteProposalPackContent([FromBody] DeleteProposalPackContentRequest payload)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:DeleteProposalPackContent");
                //var result = await DataServices<string>.PostPayload<DeleteProposalPackContentRequest>(payload, url, _httpClientFactory);
                //var result = await DataServices<string>.PostPayload<DeleteProposalPackContentRequest>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposalPackContentDto>.DeleteDbSet("ProposalPackReferenceNbr", payload.ProposalPackReferenceNbr, WebConstants.ProposalPackContent);

                //if (result == "Success")
                if (isSuccessful)
                {
                    return StatusCode(200, new
                    {
                        RequestedAction = "Delete Proposal Content Record",
                        OperationResult = "Deleted"
                    });
                }

                return StatusCode(404, new
                {
                    RequestedAction = "Delete Proposal Content Record",
                    OperationResult = "Error"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("files/delete")]
        public IActionResult DeleteProposalPackFile([FromBody] DeleteProposalPackFileRequest payload)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:DeleteProposalPackFile");
                //var result = await DataServices<string>.PostPayload<DeleteProposalPackFileRequest>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<SupportingDocFile>.DeleteDbDocSet("ProposalPackReferenceNbr", payload.ProposalPackReferenceNbr, payload.proposalPackDocType, WebConstants.ProposalPackFiles);

                if (isSuccessful)
                {
                    var uploads = webRootPath + WebConstants.ImagePath;

                    var fileName = uploads + payload.proposalPackDocName;

                    ControllerHelper.DeleteFile(fileName);

                    return StatusCode(200, new
                    {
                        RequestedAction = "Delete Proposal Content Record",
                        OperationResult = "Deleted"
                    });
                }

                return StatusCode(404, new
                {
                    RequestedAction = "Delete Proposal File Record",
                    OperationResult = "Error"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackFileError"
                });
            }
        }

        [HttpPost]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUserProfile([FromBody] DeleteUserProfile payload)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:DeleteUserProfile");

                var result = await DataServices<string>.PostPayload<DeleteUserProfile>(payload, url, _httpClientFactory);

                if (result == "Success")
                {
                    return StatusCode(200, new
                    {
                        RequestedAction = "Delete User Profile",
                        OperationResult = "Deleted"
                    });
                }

                return StatusCode(404, new
                {
                    RequestedAction = "Delete User Profile",
                    OperationResult = "Error"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteUserProfileError"
                });
            }
        }

        [HttpPost]
        [Route("delete-role")]
        public async Task<IActionResult> DeleteRoleSettings([FromBody] DeleteRoleSettings payload)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:DeleteRoleSettings");

                var result = await DataServices<string>.PostPayload<DeleteRoleSettings>(payload, url, _httpClientFactory);

                if (result == "Success")
                {
                    return StatusCode(200, new
                    {
                        RequestedAction = "Delete Role Settings",
                        OperationResult = "Deleted"
                    });
                }

                return StatusCode(404, new
                {
                    RequestedAction = "Delete Role Settings",
                    OperationResult = "Error"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteRoleSettingsError"
                });
            }
        }

        [HttpPost]
        [Route("delete-template")]
        public async Task<IActionResult> DeleteTemplateSettings([FromBody] TemplateIdSettings payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                var url = _configuration.GetValue<string>("AppSettings:DeleteTemplateSettings");

                var result = await DataServices<string>.PostPayload<TemplateIdSettings>(payload, url, _httpClientFactory);

                if (result == "Success")
                {
                    return StatusCode(200, new
                    {
                        RequestedAction = $"Delete {payload.TemplateType} Settings",
                        OperationResult = "Deleted"
                    });
                }

                return StatusCode(404, new
                {
                    RequestedAction = $"Delete {payload.TemplateType} Settings",
                    OperationResult = "Error"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Delete{payload.TemplateType}SettingsError"
                });
            }
        }

        [HttpPost]
        [Route("contents")]
        public async Task<IActionResult> AddProposalPackContentRecord([FromBody] NewProposalPackContent payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var request = new AddProposalPackContentRecordOption
                {
                    CallerEmail = userEmail,
                    ContentTypeCode = payload.ContentRecordType,
                    ReferenceNumber = payload.ReferenceNumber
                };

                var url = _configuration.GetValue<string>("AppSettings:AddProposalPackContentRecord");

                var newrecord = await DataServices<ProposalPackContentDto>.PostPayload<AddProposalPackContentRecordOption>(request, url, _httpClientFactory);

                return StatusCode(201, newrecord);

                //return StatusCode(404, new
                //{
                //    messageText = model.Message,
                //    IsAdded = false
                //});
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpGet]
        [Route("contents/record")]
        public async Task<IActionResult> GetProposalPackContentRecord([FromQuery] string refNbr, [FromQuery] decimal rowId)
        {
            try
            {
                var url = _configuration.GetValue<string>("AppSettings:GetProposalPackContentRecord");

                var query = new Dictionary<string, string>()
                {
                    ["refNbr"] = refNbr,
                    ["rowId"] = rowId.ToString(),
                };

                var uri = QueryHelpers.AddQueryString(url, query);

                var newrecord = await DataServices<ProposalPackContentDto>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, newrecord);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/general-info")]
        public IActionResult SaveTraditionalBusinessGeneral([FromBody] ProposalFormTradGeneral payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var option = new ProposalFormTradGeneralOption
                {
                    Address = payload.Address,
                    CityOfAddress = payload.CityOfAddress,
                    CountryOfAddress = payload.CountryOfAddress,
                    StateOfAddress = payload.StateOfAddress,
                    TownOfAddress = payload.TownOfAddress,
                    ContentTypeCode = payload.ContentTypeCode,
                    CountryOfBirth = payload.CountryOfBirth,
                    Dob = payload.Dob,
                    Firstname = payload.Firstname,
                    Gender = payload.Gender ?? "",
                    GenderOthers = payload.GenderOthers ?? "",
                    Middlename = payload.Middlename,
                    Nationality = payload.Nationality ?? "",
                    NationalityOthers = payload.NationalityOthers ?? "",
                    OtherTitle = payload.OtherTitle ?? "",
                    ProductCode = payload.ProductCode,
                    ReferenceNbr = payload.ReferenceNbr,
                    StateOfOrigin = payload.StateOfOrigin,
                    Surname = payload.Surname,
                    Title = payload.Title ?? "",
                    TownOrCityOfBirth = payload.TownOrCityOfBirth,
                    UserEmail = userEmail,
                };

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessGeneral");

                //var res = await DataServices<string>.PostPayload<ProposalFormTradGeneralOption>(option, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposalFormTradGeneralOption>.UpdateDbSet(option, WebConstants.ProposalFormTradGeneralOption, "ReferenceNbr", payload.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/general-info")]
        public IActionResult FindTraditionalBusinessStep1([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep1");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                var matched = Repository<ProposalFormTradGeneralOption>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.ProposalFormTradGeneralOption);

                var model = _mapper.Map<Step1DataCaptureFormTraditionalDto>(matched);

                //var model = new Step1DataCaptureFormTraditionalDto
                //{
                //    Address = matched.Address,
                //    CountryOfAddress = matched.CountryOfAddress,
                //    ContentTypeCode = matched.ContentTypeCode,
                //    ReferenceNbr = matched.ReferenceNbr,
                //    CityOfAddress = matched.CityOfAddress,
                //    CountryOfBirth = matched.CountryOfBirth,
                //    Dob = matched.Dob,
                //    Firstname = matched.Firstname,
                //    Gender = matched.Gender,
                //    GenderOthers 
                //};

                //var model = await DataServices<Step1DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/tax-info")]
        public IActionResult SaveTraditionalBusinessTaxDetails([FromBody] ProposalFormTradTaxDetails payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessTaxDetails");

                payload.UserEmail = userEmail;

                //var res = await DataServices<string>.PostPayload<ProposalFormTradTaxDetails>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposalFormTradTaxDetails>.UpdateDbSet(payload, WebConstants.ProposalFormTradTaxDetails, "ReferenceNbr", payload.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/tax-info")]
        public IActionResult FindTraditionalBusinessStep2([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep2");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step2DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<ProposalFormTradTaxDetails>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.ProposalFormTradTaxDetails);

                var model = _mapper.Map<Step2DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/identity-info")]
        public IActionResult SaveTraditionalBusinessIdentificationDetails([FromBody] ProposalFormTradIdentification payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessIdentificationDetails");

                payload.UserEmail = userEmail;

                payload.MeansOfIdentification = payload.MeansOfIdentification ?? "";
                payload.MeansOfidentificationOthers = payload.MeansOfidentificationOthers ?? "";
                payload.IdCountryOfIssue = payload.IdCountryOfIssue ?? "";
                payload.IdCountryOfIssue = payload.IdCountryOfIssueOthers ?? "";
                payload.ResidentPermitNbr = payload.ResidentPermitNbr ?? "";
                //var res = await DataServices<string>.PostPayload<ProposalFormTradIdentification>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposalFormTradIdentification>.UpdateDbSet(payload, WebConstants.ProposalFormTradIdentification, "ReferenceNbr", payload.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/identity-info")]
        public IActionResult FindTraditionalBusinessStep3([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep3");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step3DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<ProposalFormTradIdentification>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.ProposalFormTradIdentification);

                var model = _mapper.Map<Step3DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/bank-info")]
        public IActionResult SaveTraditionalBusinessBankInfo([FromBody] ProposedFormTradBankInfo payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessBankInfo");

                payload.UserEmail = userEmail;

                //var res = await DataServices<string>.PostPayload<ProposedFormTradBankInfo>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposedFormTradBankInfo>.UpdateDbSet(payload, WebConstants.ProposedFormTradBankInfo, "ReferenceNbr", payload.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/bank-info")]
        public IActionResult FindTraditionalBusinessStep4([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep4");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step4DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<ProposedFormTradBankInfo>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.ProposedFormTradBankInfo);

                var model = _mapper.Map<Step4DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/mortgage-info")]
        public IActionResult SaveTraditionalBusinessMortgageInfo([FromBody] ProposalFormTradMortgageInfo payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessMortgageInfo");

                payload.UserEmail = userEmail;
                payload.MortgageAddress = payload.MortgageAddress ?? "";
                payload.MortgageName = payload.MortgageName ?? "";
                payload.InterestRate = payload.InterestRate ?? "";

                //var res = await DataServices<string>.PostPayload<ProposalFormTradMortgageInfo>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposalFormTradMortgageInfo>.UpdateDbSet(payload, WebConstants.ProposalFormTradMortgageInfo, "ReferenceNbr", payload.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/mortgage-info")]
        public IActionResult FindTraditionalBusinessStep5([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep5");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step5DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<ProposalFormTradMortgageInfo>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.ProposalFormTradMortgageInfo);

                var model = _mapper.Map<Step5DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/children-education")]
        public IActionResult SaveTraditionalBusinessChildrenEducation([FromBody] ProposedFormTradChildrenEducation payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessChildrenEducation");

                payload.UserEmail = userEmail;

                //var res = await DataServices<string>.PostPayload<ProposedFormTradChildrenEducation>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<ProposedFormTradChildrenEducation>.UpdateDbSet(payload, WebConstants.ProposedFormTradChildrenEducation, "ReferenceNbr", payload.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/children-education")]
        public IActionResult FindTraditionalBusinessStep6([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep6");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step6DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<ProposedFormTradChildrenEducation>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.ProposedFormTradChildrenEducation);

                var model = _mapper.Map<Step6DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/digital-plan")]
        public IActionResult SaveTraditionalBusinessDigitalPlan([FromBody] List<NewDigitalPlanNomineeForm> payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveTraditionalBusinessDigitalPlan");
                //var res = await DataServices<string>.PostPayload<List<NewDigitalPlanNomineeForm>>(payload, url, _httpClientFactory);

                var isSuccessful = false;

                isSuccessful = RoutesController<NewDigitalPlanNomineeForm>.DeleteRangeDbSet("ReferenceNbr", payload[0].ReferenceNbr, WebConstants.NewDigitalPlanNomineeForm);

                if (isSuccessful)
                {
                    foreach (var item in payload)
                    {
                        item.UserEmail = userEmail;
                        item.NomineeName = item.NomineeName ?? "";
                        item.NomineeDob = item.NomineeDob ?? "";
                        item.NomineeRelationship = item.NomineeRelationship ?? "";

                        isSuccessful = RoutesController<NewDigitalPlanNomineeForm>.PostDbSet(item, WebConstants.NewDigitalPlanNomineeForm);
                    }
                }

                var res = isSuccessful ? "Success" : null;

                return StatusCode(200, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/digital-plan")]
        public IActionResult FindTraditionalBusinessStep7([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep7");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<List<Step7DataCaptureFormTraditionalDto>>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<NewDigitalPlanNomineeForm>.GetAll(WebConstants.NewDigitalPlanNomineeForm, u => u.ReferenceNbr == RefNbr);

                var model = _mapper.Map<List<Step7DataCaptureFormTraditionalDto>>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model.ToArray()
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/beneficiaries")]
        public IActionResult AddBeneficiaryToProposalForm([FromBody] NewBeneficiaryForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var ops = new AddBeneficiaryOption
                {
                    BeneficiaryName = payload.BeneficiaryName,
                    Caller = userEmail,
                    ContentTypeCode = payload.ContentTypeCode,
                    Dob = payload.Dob,
                    ProportionPercent = payload.ProportionPercent,
                    ReferenceNumber = payload.ReferenceNumber,
                    Relationship = payload.Relationship
                };

                //var url = _configuration.GetValue<string>("AppSettings:AddBeneficiaryToProposalForm");

                //var newId = await DataServices<string>.PostPayload<AddBeneficiaryOption>(ops, url, _httpClientFactory);

                var isSuccessful = RoutesController<AddBeneficiaryOption>.UpdateDbSet(ops, WebConstants.AddBeneficiaryOption, "ReferenceNumber", payload.ReferenceNumber);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res,
                    //NewBeneficiaryId = Convert.ToDecimal(newId)
                    NewBeneficiaryId = Convert.ToDecimal(1)
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/delete-beneficiary")]
        public IActionResult DeleteBeneficiaryFromProposalForm([FromBody] DeleteBeneficiaryForm payload)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:DeleteBeneficiaryFromProposalForm");

                //var res = await DataServices<string>.PostPayload<DeleteBeneficiaryForm>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<AddBeneficiaryOption>.DeleteDbSet("ReferenceNumber", payload.ReferenceNumber, WebConstants.AddBeneficiaryOption);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/save-draft-beneficiary")]
        public IActionResult SaveDraftBeneficiary([FromBody] SaveDraftBeneficiaryForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SaveDraftBeneficiary");

                payload.UserEmail = userEmail;

                //var res = await DataServices<string>.PostPayload<SaveDraftBeneficiaryForm>(payload, url, _httpClientFactory);

                var isSuccessful = RoutesController<SaveDraftBeneficiaryForm>.UpdateDbSet(payload, WebConstants.SaveDraftBeneficiaryForm, "ReferenceNumber", payload.ReferenceNumber);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/beneficiaries")]
        public IActionResult FindTraditionalBusinessStep8([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep8");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<List<Step8DataCaptureFormTraditionalDto>>.GetPayload(uri, _httpClientFactory);

                var model = new List<Step8DataCaptureFormTraditionalDto>();

                var matched = Repository<AddBeneficiaryOption>.GetAll(WebConstants.AddBeneficiaryOption, u => u.ReferenceNumber == RefNbr);

                //var model = _mapper.Map<List<Step8DataCaptureFormTraditionalDto>>(matched);

                foreach (var item in matched)
                {
                    var newModel = new Step8DataCaptureFormTraditionalDto
                    {
                        BeneficiaryDob = Convert.ToDateTime(item.Dob),
                        BeneficiaryName = item.BeneficiaryName,
                        BeneficiaryProportionPct = item.ProportionPercent,
                        BeneficiaryRelationship = item.Relationship,
                        ContentTypeCode = item.ContentTypeCode,
                        ReferenceNbr = item.ReferenceNumber
                    };

                    model.Add(newModel);
                }

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model.ToArray()
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/sum-assured")]
        public IActionResult SaveSumAssured([FromBody] NewSumAssured payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var opt = new NewDataCaptureSumAssured
                {
                    SumAssured = payload.SumAssured,
                    ContentTypeCode = payload.ContentTypeCode,
                    FirstPremiumPaid = payload.FirstPremiumPaid,
                    FromDate = payload.FromDate,
                    MaturityDate = payload.MaturityDate,
                    PaymentFrequency = payload.PaymentFrequency,
                    PaymentMode = payload.PaymentMode,
                    ReferenceNbr = payload.ReferenceNbr,
                    RegularPremium = payload.RegularPremium,
                    TermYear = payload.TermYear,
                    UserEmail = userEmail
                };

                //var url = _configuration.GetValue<string>("AppSettings:SaveSumAssured");

                //var res = await DataServices<string>.PostPayload<NewDataCaptureSumAssured>(opt, url, _httpClientFactory);

                var isSuccessful = RoutesController<NewDataCaptureSumAssured>.UpdateDbSet(opt, WebConstants.NewDataCaptureSumAssured, "ReferenceNbr", opt.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/sum-assured")]
        public IActionResult FindTraditionalBusinessStep9([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                log.Info($"{RefNbr} - {ContentTypeCode}");

                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep9");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step9DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<NewDataCaptureSumAssured>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.NewDataCaptureSumAssured);

                var model = _mapper.Map<Step9DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/other-insurer")]
        public IActionResult SaveOtherInsurerDetails([FromBody] NewOtherInsurerDetails payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                log.Info($"{DateTime.Now} - {payloadstring}");

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var opt = new AddOtherInsurerOption
                {
                    OtherSumAssured = payload.OtherSumAssured,
                    ContentTypeCode = payload.ContentTypeCode,
                    DeclineReason = payload.DeclineReason ?? "",
                    HasOtherInsurer = payload.HasOtherInsurer,
                    OtherInsurerName = payload.OtherInsurerName ?? "",
                    PriorProposalDecline = payload.PriorProposalDecline,
                    ReferenceNbr = payload.ReferenceNbr,
                    UserEmail = userEmail
                };

                //var url = _configuration.GetValue<string>("AppSettings:SaveOtherInsurerDetails");

                //var res = await DataServices<string>.PostPayload<AddOtherInsurerOption>(opt, url, _httpClientFactory);

                var isSuccessful = RoutesController<AddOtherInsurerOption>.UpdateDbSet(opt, WebConstants.AddOtherInsurerOption, "ReferenceNbr", opt.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/other-insurer")]
        public IActionResult FindTraditionalBusinessStep10([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep10");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step10DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<AddOtherInsurerOption>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.AddOtherInsurerOption);

                var model = _mapper.Map<Step10DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/medical-history")]
        public IActionResult SaveMedicalHistory([FromBody] NewMedicalHistoryForm payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                log.Info($"{DateTime.Now} - {payloadstring}");

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var opt = new AddMedicalHistoryOption
                {
                    ReferenceNbr = payload.ReferenceNbr,
                    HospitalAddress = payload.HospitalAddress,
                    CallerEmail = userEmail,
                    ContentTypeCode = payload.ContentTypeCode,
                    HeightInMeters = payload.HeightInMeters,
                    HospitalName = payload.HospitalName,
                    WeightInKg = payload.WeightInKg
                };

                //var url = _configuration.GetValue<string>("AppSettings:SaveMedicalHistory");

                //var res = await DataServices<string>.PostPayload<AddMedicalHistoryOption>(opt, url, _httpClientFactory);

                var isSuccessful = RoutesController<AddMedicalHistoryOption>.UpdateDbSet(opt, WebConstants.AddMedicalHistoryOption, "ReferenceNbr", opt.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/medical-history")]
        public IActionResult FindTraditionalBusinessStep11([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep11");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step11DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<AddMedicalHistoryOption>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.AddMedicalHistoryOption);

                var model = _mapper.Map<Step11DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/misc-details")]
        public IActionResult SaveMiscellaneousDetails([FromBody] NewMiscellaneousForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var opt = new AddMiscellaneousOption
                {
                    ReferenceNbr = payload.ReferenceNbr,
                    ContentTypeCode = payload.ContentTypeCode,
                    Diabetes = payload.Diabetes,
                    Epilepsy = payload.Epilepsy,
                    ExpectedDeliveryMonth = payload.ExpectedDeliveryMonth ?? "",
                    HeartDisease = payload.HeartDisease,
                    Hypertension = payload.Hypertension,
                    Insanity = payload.Insanity,
                    IsPregnant = payload.IsPregnant,
                    MedicationDetails = payload.MedicationDetails ?? "",
                    OtherIllness = payload.OtherIllness,
                    OtherIllnessDetails = payload.OtherIllnessDetails ?? "",
                    SickOrOnMedication = payload.SickOrOnMedication,
                    Smoked = payload.Smoked,
                    Tuberculosis = payload.Tuberculosis,
                    UserEmail = userEmail
                };

                //var url = _configuration.GetValue<string>("AppSettings:SaveMiscellaneousDetails");

                //var res = await DataServices<string>.PostPayload<AddMiscellaneousOption>(opt, url, _httpClientFactory);

                var isSuccessful = RoutesController<AddMiscellaneousOption>.UpdateDbSet(opt, WebConstants.AddMiscellaneousOption, "ReferenceNbr", opt.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/misc-details")]
        public IActionResult FindTraditionalBusinessStep12([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep12");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step12DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<AddMiscellaneousOption>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.AddMiscellaneousOption);

                var model = _mapper.Map<Step12DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/other-medical-info")]
        public IActionResult SaveOtherMedicalInfo([FromBody] NewOtherMedicalInfoForm payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                log.Info($"{DateTime.Now} - {payloadstring}");

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var opt = new AddOtherMedicalInfo
                {
                    ReferenceNbr = payload.ReferenceNbr,
                    ContentTypeCode = payload.ContentTypeCode,
                    HivAids = payload.HivAids,
                    PastTimeActivities = payload.PastTimeActivities,
                    BloodTransfusion = payload.BloodTransfusion,
                    HepatitisB = payload.HepatitisB,
                    NightSweats = payload.NightSweats,
                    RecurrentDiarrhea = payload.RecurrentDiarrhea,
                    ResideOutsideNigeria = payload.ResideOutsideNigeria,
                    SkinDisorder = payload.SkinDisorder,
                    SwollenGlands = payload.SwollenGlands,
                    WeightLoss = payload.WeightLoss,
                    UserEmail = userEmail
                };

                //var url = _configuration.GetValue<string>("AppSettings:SaveOtherMedicalInfo");

                //var res = await DataServices<string>.PostPayload<AddOtherMedicalInfo>(opt, url, _httpClientFactory);

                var isSuccessful = RoutesController<AddOtherMedicalInfo>.UpdateDbSet(opt, WebConstants.AddOtherMedicalInfo, "ReferenceNbr", opt.ReferenceNbr);

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/other-medical-info")]
        public IActionResult FindTraditionalBusinessStep13([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:FindTraditionalBusinessStep13");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["ContentTypeCode"] = ContentTypeCode,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var model = await DataServices<Step13DataCaptureFormTraditionalDto>.GetPayload(uri, _httpClientFactory);

                var matched = Repository<AddOtherMedicalInfo>.Find(u => u.ReferenceNbr == RefNbr, WebConstants.AddOtherMedicalInfo);

                var model = _mapper.Map<Step13DataCaptureFormTraditionalDto>(matched);

                return StatusCode(200, new
                {
                    FoundRecord = model != null,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("data-capture/traditional/supporting-docs")]
        public IActionResult SaveSupportingDocs([FromQuery] string RefNbr, string DocType, string ContentTypeCode)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                var uploads = webRootPath + WebConstants.ImagePath;

                var data = Request.Form.Files;

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                if (data.Count > 0 && data != null)
                {
                    if (data[0] != null)
                    {
                        //var supportingFile = new SupportingDocPayload()
                        //{
                        //    ReferenceNbr = RefNbr,
                        //    DocType = DocType,
                        //    FileName = data[0].FileName,
                        //    UserEmail = userEmail,
                        //    ContentDispositionHeaderValue = data[0].ContentDisposition,
                        //    ContentType = data[0].ContentType,
                        //    ContentTypeCode = ContentTypeCode,
                        //    Name = data[0].Name,
                        //    Size = data[0].Length,
                        //    LastUpdatedUser = userEmail,
                        //};

                        byte[] convData;

                        var dataByte = data[0];

                        using (var ms = new MemoryStream())
                        {
                            dataByte.CopyTo(ms);
                            //supportingFile.Data = ms.ToArray();
                            convData = ms.ToArray();
                        }

                        //var url = _configuration.GetValue<string>("AppSettings:SaveSupportingDocs");

                        //var res = await DataServices<string>.PostPayload<SupportingDocPayload>(supportingFile, url, _httpClientFactory);

                        var fileExtention = Path.GetExtension(data[0].FileName);

                        string docFileName = ((RefNbr + "_" + DocType).Replace("/", "")) + fileExtention;

                        if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

                        var filename = uploads + docFileName;

                        ControllerHelper.DeleteFile(filename);

                        ControllerHelper.SaveFileToDirectory(convData, filename);

                        var supportingFile = new SupportingDocPayload()
                        {
                            ReferenceNbr = RefNbr,
                            DocType = DocType,
                            FileName = docFileName,
                            UserEmail = userEmail,
                            ContentDispositionHeaderValue = data[0].ContentDisposition,
                            ContentType = data[0].ContentType,
                            ContentTypeCode = ContentTypeCode,
                            Name = data[0].Name,
                            Size = data[0].Length,
                            LastUpdatedUser = userEmail,
                            FileUrl = "",
                            Data = new byte[(new Random().Next(0,1))],
                            LastUpdated = DateTime.Now
                        };

                        //var res = _cpcServices.SaveSupportingDoc(supportingFile);
                        var isSuccessful = RoutesController<SupportingDocPayload>.UpdateDocDbSet(supportingFile, WebConstants.ProposalPackFiles, "ReferenceNbr", RefNbr, DocType);

                        var res = isSuccessful ? "Success" : null;

                        return StatusCode(201, new
                        {
                            OperationOutcome = res
                        });
                    }

                    else
                    {
                        return StatusCode(500, new
                        {
                            ErrorDescription = "File is Empty",
                            ExceptionType = "SaveDataCaptureFormError"
                        });
                    }
                }

                else
                {
                    return StatusCode(500, new
                    {
                        ErrorDescription = "No File Uploaded",
                        ExceptionType = "SaveDataCaptureFormError"
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/supporting-docs")]
        public IActionResult GetSupportingDocs([FromQuery] string refNbr, string contenttype)
        {
            try
            {
                //var url = _configuration.GetValue<string>("AppSettings:GetSupportingDocs");

                //var query = new Dictionary<string, string>()
                //{
                //    ["refNbr"] = refNbr,
                //    ["contenttype"] = contenttype
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var files = await DataServices<List<SupportingDocFile>>.GetPayload(uri, _httpClientFactory);

                var files = Repository<SupportingDocFile>.GetAll(WebConstants.ProposalPackFiles, u => u.ReferenceNbr == refNbr);

                return StatusCode(200, files);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetSupportingDocsError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/supporting-doc")]
        public IActionResult GetFile([FromQuery] string RefNbr, string DocType, string ContentType)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                //var uploads = webRootPath + WebConstants.ImagePath;
                var uploads = @"https://localhost:7178/cpc_files/";

                //var existingFile = _cpcServices.GetFile(RefNbr, DocType, ContentType);

                var existingFile = Repository<SupportingDocFile>.Find(u => u.ReferenceNbr == RefNbr && u.DocType == DocType, WebConstants.ProposalPackFiles);

                //var url = _configuration.GetValue<string>("AppSettings:GetFile");

                //var query = new Dictionary<string, string>()
                //{
                //    ["RefNbr"] = RefNbr,
                //    ["DocType"] = DocType,
                //    ["ContentType"] = ContentType,
                //};

                //var uri = QueryHelpers.AddQueryString(url, query);

                //var existingFile = await DataServices<SupportingDocFile>.GetPayload(uri, _httpClientFactory);

                if (existingFile != null)
                {
                    existingFile.FileUrl = uploads + existingFile.FileName;

                    return StatusCode(200, new
                    {
                        FoundRecord = existingFile != null,
                        Data = existingFile
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        ErrorDescription = "No record found",
                        ExceptionType = "RetrieveDataCaptureFormError"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/submit-content")]
        public IActionResult SubmitProposalPackContent([FromBody] SubmitProposalPackContentForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var url = _configuration.GetValue<string>("AppSettings:SubmitProposalPackContent");
                //var res = await DataServices<string>.PostPayload<SubmitProposalPackContentForm>(payload, url, _httpClientFactory);

                var matched = Repository<CpcProposalPack>.Find(u => u.ReferenceNumber == payload.ReferenceNbr, WebConstants.ProposalPack);

                var isSuccessful = false;

                if (matched != null)
                {
                    matched.PpcStatus = "SAVED";

                    isSuccessful = RoutesController<CpcProposalPack>.UpdateDbSet(matched, WebConstants.ProposalPack, "ReferenceNbr", payload.ReferenceNbr);

                    if (isSuccessful)
                    {
                        var matchedContents = Repository<ProposalPackContentDto>.GetAll(WebConstants.ProposalPackContent, u => u.ProposalPackRefNbr == payload.ReferenceNbr);

                        if (matchedContents != null && matchedContents.Count() > 0)
                        {
                            //isSuccessful = RoutesController<ProposalPackContentDto>.DeleteRangeDbSet("ProposalPackRefNbr", payload.ReferenceNbr, WebConstants.ProposalPackContent);
                            isSuccessful = RoutesController<ProposalPackContentDto>.DeleteDbSet("ProposalPackRefNbr", payload.ReferenceNbr, WebConstants.ProposalPackContent);

                            foreach (var item in matchedContents)
                            {
                                item.ContentStatus = "SAVED";

                                isSuccessful = RoutesController<ProposalPackContentDto>.PostDbSet(item, WebConstants.ProposalPackContent);
                            }
                        }
                    }
                }

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SubmitProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/submit-proposal-pack")]
        public IActionResult SubmitProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                log.Info($"{DateTime.Now} - SubmitProposalPack Action Method - {payloadstring}");

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                //var url = _configuration.GetValue<string>("AppSettings:SubmitProposalPack");

                //var res = await DataServices<string>.PostPayload<SubmitProposalPackForm>(payload, url, _httpClientFactory);

                var matched = Repository<CpcProposalPack>.Find(u => u.ReferenceNumber == payload.ReferenceNbr, WebConstants.ProposalPack);

                var isSuccessful = false;

                if (matched != null)
                {
                    matched.PpcStatus = "SUBMITTED";

                    isSuccessful = RoutesController<CpcProposalPack>.UpdateDbSet(matched, WebConstants.ProposalPack, "ReferenceNbr", payload.ReferenceNbr);

                    if (isSuccessful)
                    {
                        var matchedContents = Repository<ProposalPackContentDto>.GetAll(WebConstants.ProposalPackContent, u => u.ProposalPackRefNbr == payload.ReferenceNbr);

                        if (matchedContents != null && matchedContents.Count() > 0)
                        {
                            //isSuccessful = RoutesController<ProposalPackContentDto>.DeleteRangeDbSet("ProposalPackRefNbr", payload.ReferenceNbr, WebConstants.ProposalPackContent);
                            isSuccessful = RoutesController<ProposalPackContentDto>.DeleteDbSet("ProposalPackRefNbr", payload.ReferenceNbr, WebConstants.ProposalPackContent);

                            foreach (var item in matchedContents)
                            {
                                item.ContentStatus = "SUBMITTED";

                                isSuccessful = RoutesController<ProposalPackContentDto>.PostDbSet(item, WebConstants.ProposalPackContent);
                            }
                        }
                    }
                }

                var res = isSuccessful ? "Success" : null;

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SubmitProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("inbounds")]
        public async Task<IActionResult> PickInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                var url = _configuration.GetValue<string>("AppSettings:PickInboundProposalPack");

                var res = await DataServices<string>.PostPayload<SubmitProposalPackForm>(payload, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "PickInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("accept-pack")]
        public async Task<IActionResult> AcceptInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                var url = _configuration.GetValue<string>("AppSettings:AcceptInboundProposalPack");

                var res = await DataServices<string>.PostPayload<SubmitProposalPackForm>(payload, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AcceptInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("reject-pack")]
        public async Task<IActionResult> RejectInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                var url = _configuration.GetValue<string>("AppSettings:RejectInboundProposalPack");

                var res = await DataServices<string>.PostPayload<SubmitProposalPackForm>(payload, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RejectInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("push-pack")]
        public async Task<IActionResult> PushInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                var url = _configuration.GetValue<string>("AppSettings:PushInboundProposalPack");

                var res = await DataServices<string>.PostPayload<SubmitProposalPackForm>(payload, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "PushInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("approve-pack")]
        public async Task<IActionResult> ApproveProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                payload.UserEmail = userEmail;

                var url = _configuration.GetValue<string>("AppSettings:ApproveProposalPack");

                var res = await DataServices<string>.PostPayload<SubmitProposalPackForm>(payload, url, _httpClientFactory);

                return StatusCode(201, new
                {
                    OperationOutcome = res
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "ApproveProposalPackError"
                });
            }
        }
    }
}




//[HttpGet]
//[Route("data-capture/traditional/supporting-doc")]
//public async Task<IActionResult> GetFile([FromQuery] string RefNbr, string DocType, string ContentType)
//{
//    string webRootPath = _webHostEnvironment.WebRootPath;

//    try
//    {
//        //var uploads = webRootPath + WebConstants.ImagePath;
//        //var uploads = @"https://localhost:7178/cpc_files/";

//        var uploads = @"https://localhost:5003/cpc_files/";

//        var existingFile = _cpcServices.GetFile(RefNbr, DocType, ContentType);

//        //var url = _configuration.GetValue<string>("AppSettings:GetFile");

//        //var query = new Dictionary<string, string>()
//        //{
//        //    ["RefNbr"] = RefNbr,
//        //    ["DocType"] = DocType,
//        //    ["ContentType"] = ContentType,
//        //};

//        //var uri = QueryHelpers.AddQueryString(url, query);

//        //var existingFile = await DataServices<SupportingDocFile>.GetPayload(uri, _httpClientFactory);

//        if (existingFile != null)
//        {
//          existingFile.FileUrl = uploads + existingFile.FileName;

//            return StatusCode(200, new
//            {
//                FoundRecord = existingFile != null,
//                Data = existingFile
//            });
//        }
//        else
//        {
//            return StatusCode(500, new
//            {
//                ErrorDescription = "No record found",
//                ExceptionType = "RetrieveDataCaptureFormError"
//            });
//        }
//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "RetrieveDataCaptureFormError"
//        });
//    }
//}





//var query = new Dictionary<string, string>()
//{
//    ["ReferenceNbr"] = RefNbr,
//    ["DocType"] = DocType,
//    ["UserEmail"] = userEmail,
//};

//var uri = QueryHelpers.AddQueryString(url, query);
//return StatusCode(500, new
//{
//    ErrorDescription = "No File Uploaded",
//    ExceptionType = "SaveDataCaptureFormError"
//});







//[HttpPost]
//[Consumes("multipart/form-data")]
//[Route("data-capture/traditional/supporting-docs")]
//public IActionResult SaveSupportingDocs([FromQuery] string RefNbr, [FromQuery] string DocType)
//{
//    try
//    {
//        var data = Request.Form.Files;

//        string webRootPath = _webHostEnvironment.WebRootPath;
//        var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);
//        var uploads = webRootPath + WebConstants.ImagePath;

//        if (data.Count > 0 && data != null)
//        {
//            if (data[0] != null)
//            {
//                var fileExtention = Path.GetExtension(data[0].FileName);

//                string docFileName = ((RefNbr + "_" + DocType).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var filename = uploads + docFileName;

//                ControllerHelper.DeleteFile(filename);

//                ControllerHelper.SaveFileToDirectory(data[0], filename);

//                var supportingFile = new SupportingDocFile()
//                {
//                    FileName = docFileName,
//                    Name = data[0].Name,
//                    ContentType = data[0].ContentType,
//                    Size = data[0].Length,
//                    ReferenceNbr = RefNbr,
//                    DocType = DocType,
//                    LastUpdatedUser = userEmail,
//                };

//                _cpcServices.SaveSupportingDoc(supportingFile);
//            }

//        }

//        else
//        {
//            var existingFile = _cpcServices.GetFile(RefNbr, DocType);

//            existingFile.DocType = DocType;
//            existingFile.LastUpdatedUser = userEmail;

//            _cpcServices.SaveSupportingDoc(existingFile);
//        }

//        return StatusCode(201, new
//        {
//            OperationOutcome = res
//        });

//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "SaveDataCaptureFormError"
//        });
//    }
//}







//[HttpPost]
//[Consumes("multipart/form-data")]
//[Route("data-capture/traditional/supporting-docs")]
//public IActionResult SaveSupportingDocs([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
//{
//    var data = Request.Form.Files;

//    string webRootPath = _webHostEnvironment.WebRootPath;

//    if (data.Count > 0 && data != null)
//    {
//        try
//        {
//            var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);
//            var uploads = webRootPath + WebConstants.ImagePath;

//            var fileId = data.First(i => i.Name == WebConstants.IdDocTypeName);
//            var filePassport = data.First(i => i.Name == WebConstants.PassportDocTypeName);
//            var fileUtility = data.First(i => i.Name == WebConstants.UtilityDocTypeName);

//            if (fileId != null)
//            {
//                var fileExtention = Path.GetExtension(fileId.FileName);
//                string docFileName = ((RefNbr + ContentTypeCode + WebConstants.IdDocTypeName).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var idDoc = ControllerHelper.HandleFormFile(fileId, uploads, RefNbr, ContentTypeCode, WebConstants.IdDocTypeName, docFileName);
//                _cpcServices.SaveSupportingIdDocs(idDoc, userEmail);
//            }

//            if (filePassport != null)
//            {
//                var fileExtention = Path.GetExtension(filePassport.FileName);
//                string docFileName = ((RefNbr + ContentTypeCode + WebConstants.PassportDocTypeName).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var passportDoc = ControllerHelper.HandleFormFile(filePassport, uploads, RefNbr, ContentTypeCode, WebConstants.PassportDocTypeName, docFileName);
//                _cpcServices.SaveSupportingIdDocs(passportDoc, userEmail);
//            }

//            if (fileUtility != null)
//            {
//                var fileExtention = Path.GetExtension(fileUtility.FileName);
//                string docFileName = ((RefNbr + ContentTypeCode + WebConstants.UtilityDocTypeName).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var utilityDoc = ControllerHelper.HandleFormFile(fileUtility, uploads, RefNbr, ContentTypeCode, WebConstants.UtilityDocTypeName, docFileName);
//                _cpcServices.SaveSupportingIdDocs(utilityDoc, userEmail);
//            }


//            return StatusCode(201, new
//            {
//                OperationOutcome = res
//            });
//        }
//        catch (Exception ex)
//        {
//            log.Error(DateTime.Now.ToString(), ex);

//            return StatusCode(500, new
//            {
//                ErrorDescription = ex.Message,
//                ExceptionType = "SaveDataCaptureFormError"
//            });
//        }
//    }
//    else
//    {
//        return StatusCode(500, new
//        {
//            ErrorDescription = "No File Uploaded",
//            ExceptionType = "SaveDataCaptureFormError"
//        });
//    }
//}











//[HttpGet]
//[Route("data-capture/traditional/supporting-doc")]
//public IActionResult GetFile([FromQuery] string RefNbr, string ContentTypeCode)
//{
//    string webRootPath = _webHostEnvironment.WebRootPath;

//    try
//    {
//        //var uploads = webRootPath + WebConstants.ImagePath;

//        var uploads = @"https://localhost:7178/cpc_files/";

//        var existingIdFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, WebConstants.IdDocTypeName);
//        var existingPassportFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, WebConstants.PassportDocTypeName);
//        var existingUtilityFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, WebConstants.UtilityDocTypeName);


//        if (existingIdFile != null || existingPassportFile != null || existingUtilityFile != null)
//        {
//            existingIdFile.FileUrl = uploads + existingIdFile.FileName;
//            //existingIdFile.FileUrl = @"https://images.freeimages.com/images/large-previews/411/platform-1154314.jpg";
//            existingPassportFile.FileUrl = uploads + existingPassportFile.FileName;
//            //existingPassportFile.FileUrl = @"https://images.freeimages.com/images/large-previews/411/platform-1154314.jpg";
//            existingUtilityFile.FileUrl = uploads + existingUtilityFile.FileName;
//            //existingUtilityFile.FileUrl = @"https://images.freeimages.com/images/large-previews/411/platform-1154314.jpg";

//            var model = new Step14DataCaptureFormTraditionalDto
//            {
//                ContentTypeCode = ContentTypeCode,
//                ReferenceNbr = RefNbr,
//                IdFIle = existingIdFile,
//                PassportFile = existingPassportFile,
//                UtilityBillFile = existingUtilityFile
//            };
//            //"/Users/apple/Desktop/CPC(3)/CPCHub/app-login/wwwroot/cpc_files/comedy-logo-illustration-theme-performances-stand-up-etc-178212650.jpg"
//            return StatusCode(200, new
//            {
//                FoundRecord = model != null,
//                Data = model
//            });
//        }
//        else
//        {
//            return StatusCode(500, new
//            {
//                ErrorDescription = "No record found",
//                ExceptionType = "RetrieveDataCaptureFormError"
//            });
//        }
//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "RetrieveDataCaptureFormError"
//        });
//    }
//}






//var existingIdFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, "id_file");
//var existingPassportFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, "passport_file");
//var existingUtilityFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, "utility_file");


//if (existingIdFile != null || existingPassportFile != null || existingUtilityFile != null)
//{
//    var existingFiles = new List<SupportingDocFile>()
//                        {
//                            existingIdFile,
//                            existingPassportFile,
//                            existingUtilityFile
//                        };

//    foreach (var item in existingFiles)
//    {
//        var oldFile = uploads + item.FileName;

//        if (System.IO.File.Exists(oldFile))
//        {
//            System.IO.File.Delete(oldFile);
//        }
//    }
//}



//model.IdFIle = await ControllerHelper.CreateFormFile(existingIdFile, uploads);
//model.PassportFile = await ControllerHelper.CreateFormFile(existingPassportFile, uploads);
//model.UtilityBillFile = await ControllerHelper.CreateFormFile(existingUtilityFile, uploads);


//ExistingIdFIle = existingIdFile,
//ExistingPassportFile = existingPassportFile,
//ExistingUtilityBillFile = existingUtilityFile



//                    catch (Exception e)
//{
//    Response.Clear();
//    Response.StatusCode = 204;
//    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
//    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
//}





//[HttpPost]
//[Route("data-capture/traditional/supporting-docs")]
//public IActionResult SaveSupportingDocs([FromBody] NewSupportingDocsForm payload)
//{
//    string webRootPath = _webHostEnvironment.WebRootPath;

//    try
//    {
//        string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
//        log.Info($"{DateTime.Now} - {payloadstring}");

//        var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);
//        var uploads = webRootPath + WebConstants.ImagePath;

//        //var model = _cpcServices.FindDataCaptureFormTraditionalStep14(payload.ReferenceNbr, payload.ContentTypeCode);
//        var existingIdFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(payload.ReferenceNbr, payload.ContentTypeCode, "id_card");
//        var existingPassportFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(payload.ReferenceNbr, payload.ContentTypeCode, "passport");
//        var existingUtilityFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(payload.ReferenceNbr, payload.ContentTypeCode, "utility_bill");

//        var existingFiles = new List<SupportingDocFile>()
//                    {
//                        existingIdFile,
//                        existingPassportFile,
//                        existingUtilityFile
//                    };


//        foreach (var item in existingFiles)
//        {
//            var oldFile = uploads + $@"\{item.FileUrl}";

//            if (System.IO.File.Exists(oldFile))
//            {
//                System.IO.File.Delete(oldFile);
//            }
//        }

//        var idDoc = ControllerHelper.HandleFile(payload.IdFIle[0], uploads, payload.ReferenceNbr, payload.ContentTypeCode, "id_card");
//        var passportDoc = ControllerHelper.HandleFile(payload.PassportFile[0], uploads, payload.ReferenceNbr, payload.ContentTypeCode, "passport");
//        var utilityDoc = ControllerHelper.HandleFile(payload.UtilityBillFile[0], uploads, payload.ReferenceNbr, payload.ContentTypeCode, "utility_bill");

//        _cpcServices.SaveSupportingIdDocs(idDoc, userEmail);
//        _cpcServices.SaveSupportingIdDocs(passportDoc, userEmail);
//        _cpcServices.SaveSupportingIdDocs(utilityDoc, userEmail);

//        return StatusCode(201, new
//        {
//            OperationOutcome = res
//        });
//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "SaveDataCaptureFormError"
//        });
//    }

//}
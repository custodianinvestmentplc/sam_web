using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Options;
using SAM.WEB.Models;
using SAM.WEB.Payloads;
using SAM.WEB.Services;
using SAM.WEB.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using log4net;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using SAM.API.Controllers;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using SAM.WEB.Repo;
using SAM.WEB;
using SAM.NUGET;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace SAM.WEB.Controllers
{
    public class CPCController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;

        public CPCController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICPCHubServices cpcHubServices, LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _cpcServices = cpcHubServices;
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
        }

        [HttpGet]
        public async Task<IActionResult> CreateProposalPack()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var newProposalPack = new NewProposalPack();

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    //var url = _configuration.GetValue<string>("AppSettings:GetAllBranches");

                    //var branches = await DataServices<List<CPCBranchDto>>.GetPayload(url, _httpClientFactory);

                    var branches = new List<CPCBranchDto>
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

                    if (branches.Count < 1 || branches == null) TempData["Warning"] = "Fetch Branches \n No record was found";

                    else newProposalPack.Branches = branches;
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = "Fetch Branches \n Unable to fetch Branches from back end";
                }
            }

            return View(newProposalPack);
        }

        [HttpPost]
        public IActionResult SubmitProposalPack(NewProposalPack payload)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                //var packOptions = new CreateProposalPackOptions
                //{
                //    InitiatingAgentCode = payload.AgentCode,
                //    CallerEmail = userEmail,
                //    InitiatingBranchCode = payload.BranchCode,
                //    ProposalTitle = payload.Title,
                //    ProposalPackType = payload.ProposalType,
                //};

                //var url = _configuration.GetValue<string>("AppSettings:CreatePPUrl");

                //var refNbr = await DataServices<string>.PostPayload<CreateProposalPackOptions>(packOptions, url, _httpClientFactory);

                var packOptions = new CpcProposalPack
                {
                    CreateDate = DateTime.Now,
                    CreateUserEmail = userEmail,
                    InitatingAgentCode = payload.AgentCode,
                    InitiatingBranchCode = payload.BranchCode,
                    Title = payload.Title,
                    ProposalPackType = payload.ProposalType,
                    CreateUserName = userEmail,
                    InitiatingAgentName = payload.AgentCode,
                    InitiatingBranchName = payload.BranchCode,
                    PpcStatus = "NEW",
                    ReferenceNumber = $@"CLA/{DateTime.Now}/{new Random().Next(1000, 9999)}"
                };

                var isSuccessful = RoutesController<CpcProposalPack>.PostDbSet(packOptions, WebConstants.ProposalPack);

                if (isSuccessful)
                {
                    var newContent = new ProposalPackContentDto
                    {
                        ProposalPackRefNbr = packOptions.ReferenceNumber,
                        ContentStatus = "NEW",
                        ContentTypeCode = payload.ProposalType,
                        ContentTypeDescription = "Traditional",
                        ContentTypeShortDesc = "Traditional",
                        CreateDate = DateTime.Now,
                        CreateUser = userEmail,
                        //ProposalPackContentRowId = 1
                    };

                    var contentInserted = RoutesController<ProposalPackContentDto>.PostDbSet(newContent, WebConstants.ProposalPackContent);

                    if (contentInserted)
                    {
                        //TempData["RefNbr"] = refNbr;
                        HttpContext.Session.Set("RefNbr", packOptions.ReferenceNumber);
                        HttpContext.Session.Set("SuccessAlert", $"New Proposal Pack created. The Id Number is {packOptions.ReferenceNumber}!");
                        //HttpContext.Session.Set("StatusIndex", "NEW");
                        return RedirectToAction("ProposalPackProperty");
                    }
                    else throw new Exception("Was unable to create proposal pack content");
                }
                else throw new Exception("Was unable to create proposal pack");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Submit Proposal Pack \n Unable to Submit Proposal Pack";

                TempData["ErrorTitle"] = "Unable to Submit Proposal Pack";
                TempData["ExceptionType"] = "Submit Proposal Pack Error";
                TempData["ErrorDescription"] = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("CreateProposalPack");

            }
        }

        [HttpGet]
        public async Task<IActionResult> ProposalPackProperty(string? refnbr)
        {
            if (string.IsNullOrWhiteSpace(refnbr)) refnbr = HttpContext.Session.Get<string>("RefNbr");

            HttpContext.Session.Set("RefNbr", refnbr);

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.Get<string>("SuccessAlert"))) TempData["SuccessAlert"] = $"New Proposal Pack created. The Id Number is {refnbr}!";

            HttpContext.Session.Remove("SuccessAlert");

            if (!string.IsNullOrWhiteSpace(refnbr))
            {
                var endpointName = RouteData.Values["action"].ToString();

                await GetUserViewAsync(endpointName);

                var proposalPackProperty = new CPCProposalPackProperty();

                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["refnbr"] = refnbr,
                    //};

                    try
                    {
                        var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                        //var urlProposalPack = _configuration.GetValue<string>("AppSettings:GetProposalPack");
                        //var uriProposalPack = QueryHelpers.AddQueryString(urlProposalPack, query);

                        //proposalPackProperty.ProposalPack = await DataServices<CpcProposalPack>.GetPayload(uriProposalPack, _httpClientFactory);

                        proposalPackProperty.ProposalPack = Repository<CpcProposalPack>.Find(u => u.ReferenceNumber == refnbr, WebConstants.ProposalPack);

                        if (proposalPackProperty.ProposalPack != null)
                        {
                            //var urlProposalPackContents = _configuration.GetValue<string>("AppSettings:GetProposalPackContents");

                            //var uriProposalPackContents = QueryHelpers.AddQueryString(urlProposalPackContents, query);

                            //proposalPackProperty.Contents = await DataServices<List<ProposalPackContentDto>>.GetPayload(uriProposalPackContents, _httpClientFactory);

                            proposalPackProperty.Contents = Repository<ProposalPackContentDto>.GetAll(WebConstants.ProposalPackContent, u => u.ProposalPackRefNbr == refnbr);

                            if (proposalPackProperty.Contents.Count > 0 && proposalPackProperty.Contents != null)
                            {
                                ViewBag.SavedStatus = proposalPackProperty.Contents.Exists(content => content.ContentStatus.ToLower() == "saved");
                                ViewBag.RejectedStatus = proposalPackProperty.Contents.Exists(content => content.ContentStatus.ToLower() == "rejected");
                                ViewBag.NewStatus = proposalPackProperty.Contents.Exists(content => content.ContentStatus.ToLower() == "new");
                                ViewBag.HasContent = true;

                                if (!Convert.ToBoolean(ViewBag.SavedStatus) && !Convert.ToBoolean(ViewBag.NewStatus) && !Convert.ToBoolean(ViewBag.RejectedStatus))
                                {
                                    //var urlSupportingDocs = _configuration.GetValue<string>("AppSettings:GetSupportingDocs");

                                    //var queryDocs = new Dictionary<string, string>()
                                    //{
                                    //    ["refnbr"] = refnbr,
                                    //    ["contenttype"] = proposalPackProperty.ProposalPack.ProposalPackType,
                                    //};

                                    //var uriSupportingDocs = QueryHelpers.AddQueryString(urlSupportingDocs, queryDocs);

                                    //proposalPackProperty.Docs = await DataServices<List<SupportingDocFile>>.GetPayload(uriSupportingDocs, _httpClientFactory);

                                    proposalPackProperty.Docs = Repository<SupportingDocFile>.GetAll(WebConstants.ProposalPackFiles, u => u.ReferenceNbr == refnbr);

                                    if (proposalPackProperty.Docs.Count <= 0 || proposalPackProperty.Docs == null) TempData["Warning"] = "Fetch Proposal Pack Supporting Documents \n No Supporting Document was found for Proposal Pack";
                                }
                            }

                            else
                            {
                                ViewBag.HasContent = false;

                                TempData["Warning"] = "Fetch Proposal Pack Contents \n No content was found for Proposal Pack";
                            }
                        }

                        else TempData["Warning"] = "Fetch Proposal Pack \n Proposal Pack does not exist in Database";

                    }
                    catch (Exception ex)
                    {
                        log.Error(DateTime.Now.ToString(), ex);

                        TempData["Warning"] = "Fetch Proposal Pack Properties \n Unable to fetch Proposal Pack Properties/Files from Database";

                        //TempData["Error"] = ex.ToString();

                        TempData["ErrorTitle"] = "Unable to Fetch Proposal Pack Property";
                        TempData["ExceptionType"] = "Fetch Proposal Pack Property Error";
                        TempData["ErrorDescription"] = ex.Message;

                        return RedirectToAction("CPCHub", "Modules");
                    }
                }

                return View(proposalPackProperty);
            }

            else
            {
                TempData["Error"] = "Proposal pack reference number not provided to fetch proposal pack Ppoperty.";

                TempData["ErrorTitle"] = "Unable to Fetch Proposal Pack Property";
                TempData["ExceptionType"] = "Fetch Proposal Pack Property Error";
                TempData["ErrorDescription"] = TempData["Error"];

                //log.Error(DateTime.Now.ToString(), TempData["Error"].ToString());

                return RedirectToAction("CPCHub", "Modules");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DataCapture(string refnbr, string contenttypecode)
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var data = new CpcDataCaptureDto
            {
                ContentTypeCode = contenttypecode,
                ReferenceNbr = refnbr,
            };

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    ViewBag.IsDisbled = false;

                    var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    var contents = Repository<ProposalPackContentDto>.GetAll(WebConstants.ProposalPackContent, u => u.ProposalPackRefNbr == refnbr);

                    if (contents.Count > 0 && contents != null)
                    {
                        ViewBag.SavedStatus = contents.Exists(content => content.ContentStatus.ToLower() == "saved");
                        ViewBag.RejectedStatus = contents.Exists(content => content.ContentStatus.ToLower() == "rejected");
                        ViewBag.NewStatus = contents.Exists(content => content.ContentStatus.ToLower() == "new");
                        ViewBag.HasContent = true;

                        if (!Convert.ToBoolean(ViewBag.NewStatus) && !Convert.ToBoolean(ViewBag.RejectedStatus)) ViewBag.IsDisbled = true;
                    }
                    else throw new Exception("Proposal Pack has no content");

                    //var urlProds = _configuration.GetValue<string>("AppSettings:GetAllCpcProducts");

                    //var urlStates = _configuration.GetValue<string>("AppSettings:GetAllStates");

                    //var states = await DataServices<List<StatesInNigeriaDto>>.GetPayload(urlStates, _httpClientFactory);

                    //var prods = await DataServices<List<CpcProductDto>>.GetPayload(urlProds, _httpClientFactory);

                    var states = new List<StatesInNigeriaDto>
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

                    var prods = new List<CpcProductDto>
                    {
                        new CpcProductDto
                    {
                        ProductName = "Prod1",
                        ProductCode = "1",
                        CreateDate = new DateTime(2022,01,06),
                        CreatedBy = useremail,
                    },
                    new CpcProductDto
                    {
                        ProductName = "Prod2",
                        ProductCode = "1",
                        CreateDate = new DateTime(2022,01,06),
                        CreatedBy = useremail,
                    },
                    };

                    if (states == null || states.Count < 1) TempData["Warning"] = "Fetch States \n No state record was found";
                    else
                    {
                        data.States = states;
                    }

                    if (prods == null || prods.Count < 1) TempData["Warning"] = "Fetch Products \n No product record was found";
                    else
                    {
                        data.Products = prods;
                    }

                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = "Fetch Products & States \n Unable to fetch products and states from Database";
                }
            }

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> FileCapture(string refnbr, string contenttype, string activefiletype)
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var file = new CpcFileContextDto
            {
                ActiveFileType = activefiletype,
                ReferenceNbr = refnbr,
                ContentType = contenttype,
            };

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    //var url = _configuration.GetValue<string>("AppSettings:GetAllCpcFiles");

                    //var files = await DataServices<List<CpcFileDto>>.GetPayload(url, _httpClientFactory);

                    var files = new List<CpcFileDto>
                    {
                        new CpcFileDto
                        {
                             FileName = "Means of Identification",
                             FileCode = "id_file",
                             CreatedBy = useremail,
                             CreateDate = new DateTime(2022, 01, 06),
                        },
                        new CpcFileDto
                        {
                             FileName = "Utility Bill",
                             FileCode = "utility_file",
                             CreatedBy = useremail,
                             CreateDate = new DateTime(2022, 01, 06),
                        },
                    };

                    if (files == null || files.Count < 1) TempData["Warning"] = "Fetch File \n No record was found";
                    else
                    {
                        file.Files = files;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = "Fetch Proposal Pack Supporting Documents \n Unable to fetch Proposal Pack Supporting Documents from Database";
                }
            }

            return View(file);
        }

        [HttpGet]
        public async Task<IActionResult> GetDraftProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var daftsProposalPacks = new List<CpcProposalPack>();

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    //var url = _configuration.GetValue<string>("AppSettings:GetDraftProposalPack");
                    //daftsProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                    daftsProposalPacks = Repository<CpcProposalPack>.GetAll(WebConstants.ProposalPack, u => u.PpcStatus == "NEW");

                    daftsProposalPacks = daftsProposalPacks.OrderByDescending(u => u.CreateDate).ToList();

                    if (daftsProposalPacks.Count < 1 || daftsProposalPacks == null) TempData["Warning"] = "Fetch Daft Proposal Packs \n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = "Fetch Daft Proposal Packs \n No record was found";
                }
            }

            return View(daftsProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public async Task<IActionResult> SubmittedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var submittedProposalPacks = new List<CpcProposalPack>();

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    //var url = _configuration.GetValue<string>("AppSettings:GetSubmittedProposalPack");
                    //submittedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                    var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    submittedProposalPacks = Repository<CpcProposalPack>.GetAll(WebConstants.ProposalPack, u => u.PpcStatus == "SUBMITTED");
                    submittedProposalPacks = submittedProposalPacks.OrderByDescending(u => u.CreateDate).ToList();

                    if (submittedProposalPacks.Count < 1 || submittedProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
            }

            return View(submittedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public async Task<IActionResult> InboundProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var inboundProposalPacks = new List<CpcProposalPack>();

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    //var url = _configuration.GetValue<string>("AppSettings:GetInboundProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //inboundProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (inboundProposalPacks.Count < 1 || inboundProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
            }

            return View(inboundProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public async Task<IActionResult> WIPProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var wipProposalPacks = new List<CpcProposalPack>();


            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    //var url = _configuration.GetValue<string>("AppSettings:GetWIPProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //wipProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (wipProposalPacks.Count < 1 || wipProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
            }

            return View(wipProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public async Task<IActionResult> AcceptedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var acceptedProposalPacks = new List<CpcProposalPack>();


            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    //var url = _configuration.GetValue<string>("AppSettings:GetAcceptedProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //acceptedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (acceptedProposalPacks.Count < 1 || acceptedProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
            }

            return View(acceptedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            await GetUserViewAsync(endpointName);

            var approvedProposalPacks = new List<CpcProposalPack>();


            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    //var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                    //var url = _configuration.GetValue<string>("AppSettings:GetApprovedProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //approvedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (approvedProposalPacks.Count < 1 || approvedProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \n No record was found";
                }
            }

            return View(approvedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public Task<IActionResult> UsersProfile()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> RolesSetting()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> Permissions()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> Forms()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> Branches()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> CPCTypes()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> CPCProducts()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> States()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        [HttpGet]
        public Task<IActionResult> SupportingDocuments()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return GetUserViewAsync(endpointName);
        }

        public async Task<IActionResult> GetUserViewAsync(string formName)
        {
            //var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                var useremail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);


                if (!string.IsNullOrEmpty(useremail))
                {
                    //var url = _configuration.GetValue<string>("AppSettings:AuthUrl");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = useremail
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //var user = await DataServices<UserRegisterDto>.GetPayload(uri, _httpClientFactory);

                    var user = new UserRegisterDto
                    {
                        UserDisplayName = "Emmanuel",
                        UserRole = "Agent",
                        BranchName = "BranchName1",
                        AddedByEmail = useremail,
                        BranchCode = "1",
                        CreateDate = new DateTime(2022, 01, 06),
                        JobDescription = "Agent",
                        UserEmail = useremail,
                        TableRowId = 1,
                        UserRoleId = "1"
                    };

                    var _permissions = await ControllerHelper.Authorization(formName, useremail, _configuration, _httpClientFactory);

                    ViewBag.CanViewPermission = _permissions.Exists(permission => permission.ToLower() == "can_view");

                    if (!Convert.ToBoolean(ViewBag.CanViewPermission)) TempData["Warning"] = $"View {formName} Form \n You do not have permission to view {formName} Form";

                    else
                    {
                        ViewBag.CanEditPermission = _permissions.Exists(permission => permission.ToLower() == "can_edit");

                        if (!Convert.ToBoolean(ViewBag.CanEditPermission)) TempData["Warning"] = $"Edit {formName} Form \n You do not have permission to Edit {formName}";
                    }

                    ViewBag.Module = user.UserRole.ToLower();

                    ViewBag.Username = user.UserDisplayName;

                    log.Info($"{DateTime.Now.ToString()} - Logged in the User {useremail}");

                    return Ok();
                }
                else
                {
                    TempData["ErrorTitle"] = "Unable to retreive user email";
                    TempData["ExceptionType"] = "Access Denied";

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                //make specify
                TempData["ErrorTitle"] = "You do not belong to any of the designated permission groups";
                TempData["ExceptionType"] = "Access Denied";
                TempData["ErrorDescription"] = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                //return RedirectToAction("Index", "Home", indexVM);
                return RedirectToAction("Index", "Home");
            }
        }

    }
}



//var count = 5;

//for (int i = 1; i <= count; i++)
//{
//    var cp = new CpcProposalPack
//    {
//        TableRowId = i,
//        Title = $"Test Pack{i}",
//        CreateUserEmail = useremail,
//        CreateUserName = useremail,
//        InitatingAgentCode = "1",
//        InitiatingAgentName = "Test Agent",
//        CreateDate = new DateTime(2022, 01, 06),
//        //InitiatingBranchCode = $"{rnd.Next(1,3)}",
//        InitiatingBranchCode = "1",
//        InitiatingBranchName = "Branch1",
//        PpcStatus = "NEW",
//        ProposalPackType = "001",
//        ReferenceNumber = "testRef"
//    };

//    daftsProposalPacks.Add(cp);
//}
//HttpContext.Session.Set("StatusIndex", "NEW");

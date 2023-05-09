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
using SAM.WEB.Controllers;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using SAM.WEB.Repo;
using SAM.WEB;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.AspNetCore.Authorization;

namespace SAM.WEB.Controllers
{
    [Authorize]
    public class CPCController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LoginConfig _config;

        public CPCController(IConfiguration configuration, IHttpClientFactory httpClientFactory, LoginConfig config)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet]
        //[ServiceFilter(typeof(MyNewCustomActionFilter))]
        public IActionResult CreateProposalPack()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var newProposalPack = new NewProposalPack();

            try
            {
                if (Convert.ToBoolean(ViewBag.CanViewPermission))
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

                    if (branches.Count < 1 || branches == null) TempData["Warning"] = "Fetch Branches \r\n No record was found";

                    else newProposalPack.Branches = branches;
                }
                else throw new Exception("You do not have permission to view the New Proposal Pack form.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                TempData["Warning"] = "Unable to open New Proposal Pack Form";

                TempData["ErrorTitle"] = "New Proposal Pack Form Error \r\n Unable to open New Proposal Pack Form";
                TempData["ExceptionType"] = "New Proposal Pack Form Error";
                TempData["ErrorDescription"] = ex.Message;

                return RedirectToAction("CPCHub", "Modules");
            }

            return View(newProposalPack);
        }

        [HttpPost]
        public IActionResult SubmitProposalPack(NewProposalPack payload)
        {
            try
            {
                var userEmail = HttpContext.Session.Get<string>("userEmail");

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
                TempData["Error"] = "Submit Proposal Pack \r\n Unable to Submit Proposal Pack";

                TempData["ErrorTitle"] = "Unable to Submit Proposal Pack";
                TempData["ExceptionType"] = "Submit Proposal Pack Error";
                TempData["ErrorDescription"] = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return RedirectToAction("CreateProposalPack");

            }
        }

        [HttpGet]
        public IActionResult ProposalPackProperty(string? refnbr)
        {
            if (string.IsNullOrWhiteSpace(refnbr)) refnbr = HttpContext.Session.Get<string>("RefNbr");

            HttpContext.Session.Set("RefNbr", refnbr);

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.Get<string>("SuccessAlert"))) TempData["SuccessAlert"] = $"New Proposal Pack created. The Id Number is {refnbr}!";

            HttpContext.Session.Remove("SuccessAlert");

            if (!string.IsNullOrWhiteSpace(refnbr))
            {
                var endpointName = RouteData.Values["action"].ToString();

                ConfigureView(endpointName);

                var proposalPackProperty = new CPCProposalPackProperty();

                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["refnbr"] = refnbr,
                    //};

                    try
                    {
                        var useremail = HttpContext.Session.Get<string>("userEmail");

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

                                    if (proposalPackProperty.Docs.Count <= 0 || proposalPackProperty.Docs == null) TempData["Warning"] = "Fetch Proposal Pack Supporting Documents \r\n No Supporting Document was found for Proposal Pack";
                                }
                            }

                            else
                            {
                                ViewBag.HasContent = false;

                                TempData["Warning"] = "Fetch Proposal Pack Contents \r\n No content was found for Proposal Pack";
                            }
                        }

                        else throw new Exception("Fetch Proposal Pack \r\n Proposal Pack does not exist in Database");

                    }
                    catch (Exception ex)
                    {
                        log.Error(DateTime.Now.ToString(), ex);

                        TempData["Warning"] = "Fetch Proposal Pack Properties \r\n Unable to fetch Proposal Pack Properties/Files from Database";

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
        public IActionResult DataCapture(string refnbr, string contenttypecode)
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var data = new CpcDataCaptureDto
            {
                ContentTypeCode = contenttypecode,
                ReferenceNbr = refnbr,
            };

            try
            {
                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    ViewBag.IsDisbled = false;

                    var useremail = HttpContext.Session.Get<string>("userEmail");

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

                    if (states == null || states.Count < 1) TempData["Warning"] = "Fetch States \r\n No state record was found";
                    else
                    {
                        data.States = states;
                    }

                    if (prods == null || prods.Count < 1) TempData["Warning"] = "Fetch Products \r\n No product record was found";
                    else
                    {
                        data.Products = prods;
                    }
                }
                else throw new Exception("You do not have permission to view Proposal Pack Content Forms.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                TempData["Warning"] = "Proposal Pack Content Forms Error \r\n Unable to open Proposal Pack Content Forms";

                TempData["ErrorTitle"] = "Unable to open Proposal Pack Content Forms";
                TempData["ExceptionType"] = "Proposal Pack Content Forms Error";
                TempData["ErrorDescription"] = ex.Message;

                return RedirectToAction("CPCHub", "Modules");
            }

            return View(data);
        }

        [HttpGet]
        public IActionResult FileCapture(string refnbr, string contenttype, string activefiletype)
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var file = new CpcFileContextDto
            {
                ActiveFileType = activefiletype,
                ReferenceNbr = refnbr,
                ContentType = contenttype,
            };

            try
            {
                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    var useremail = HttpContext.Session.Get<string>("userEmail");

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

                    if (files == null || files.Count < 1) TempData["Warning"] = "Fetch File \r\n No record was found";
                    else
                    {
                        file.Files = files;
                    }
                }
                else throw new Exception("You do not have permission to view Proposal Pack Supporting Document Form.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                TempData["Warning"] = "Proposal Pack Supporting Document Form Error \r\n Unable to open Proposal Pack Supporting Document";

                TempData["ErrorTitle"] = "Unable to open Proposal Pack Supporting Document";
                TempData["ExceptionType"] = "Proposal Pack Supporting Document Form Error";
                TempData["ErrorDescription"] = ex.Message;

                return RedirectToAction("CPCHub", "Modules");
            }

            return View(file);
        }

        [HttpGet]
        public IActionResult DraftProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var draftsProposalPacks = new List<CpcProposalPack>();

            try
            {
                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    var useremail = HttpContext.Session.Get<string>("userEmail");

                    //var url = _configuration.GetValue<string>("AppSettings:GetDraftProposalPack");
                    //draftsProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                    draftsProposalPacks = Repository<CpcProposalPack>.GetAll(WebConstants.ProposalPack, u => u.PpcStatus == "NEW");

                    draftsProposalPacks = draftsProposalPacks.OrderByDescending(u => u.CreateDate).ToList();

                    if (draftsProposalPacks.Count < 1 || draftsProposalPacks == null) TempData["Warning"] = "Fetch Draft Proposal Packs \r\n No record was found";
                }
                else throw new Exception("You do not have permission to view Draft Proposal Packs Page.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                TempData["Warning"] = "Draft Proposal Packs Page Error \r\n Unable to open Draft Proposal Packs page";

                TempData["ErrorTitle"] = "Unable to open Draft Proposal Packs page";
                TempData["ExceptionType"] = "Draft Proposal Packs Page Error";
                TempData["ErrorDescription"] = ex.Message;

                return RedirectToAction("CPCHub", "Modules");
            }

            return View(draftsProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult SubmittedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var submittedProposalPacks = new List<CpcProposalPack>();


            try
            {
                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    //var url = _configuration.GetValue<string>("AppSettings:GetSubmittedProposalPack");
                    //submittedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                    var useremail = HttpContext.Session.Get<string>("userEmail");

                    submittedProposalPacks = Repository<CpcProposalPack>.GetAll(WebConstants.ProposalPack, u => u.PpcStatus == "SUBMITTED");
                    submittedProposalPacks = submittedProposalPacks.OrderByDescending(u => u.CreateDate).ToList();

                    if (submittedProposalPacks.Count < 1 || submittedProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
                else throw new Exception("You do not have permission to view Submitted Proposal Packs Page.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                TempData["Warning"] = "Submitted Proposal Packs Page Error \r\n Unable to open Submitted Proposal Packs page";

                TempData["ErrorTitle"] = "Unable to open Submitted Proposal Packs page";
                TempData["ExceptionType"] = "Submitted Proposal Packs Page Error";
                TempData["ErrorDescription"] = ex.Message;

                return RedirectToAction("CPCHub", "Modules");
            }

            return View(submittedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult InboundProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var inboundProposalPacks = new List<CpcProposalPack>();

            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var useremail = HttpContext.Session.Get<string>("userEmail");

                    //var url = _configuration.GetValue<string>("AppSettings:GetInboundProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //inboundProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (inboundProposalPacks.Count < 1 || inboundProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
            }

            return View(inboundProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult RejectedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var rejectedProposalPacks = new List<CpcProposalPack>();

            try
            {
                if (Convert.ToBoolean(ViewBag.CanViewPermission))
                {
                    var useremail = HttpContext.Session.Get<string>("userEmail");

                    //var url = _configuration.GetValue<string>("AppSettings:GetRejectedProposalPack");
                    //RejectedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(url, _httpClientFactory);

                    rejectedProposalPacks = Repository<CpcProposalPack>.GetAll(WebConstants.ProposalPack, u => u.PpcStatus == "REJECTED");

                    rejectedProposalPacks = rejectedProposalPacks.OrderByDescending(u => u.CreateDate).ToList();

                    if (rejectedProposalPacks.Count < 1 || rejectedProposalPacks == null) TempData["Warning"] = "Fetch Rejected Proposal Packs \r\n No record was found";
                }
                else throw new Exception("You do not have permission to view Rejected Proposal Packs Page.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                TempData["Warning"] = "Rejected Proposal Packs Page Error \r\n Unable to open Rejected Proposal Packs page";

                TempData["ErrorTitle"] = "Unable to open Rejected Proposal Packs page";
                TempData["ExceptionType"] = "Rejected Proposal Packs Page Error";
                TempData["ErrorDescription"] = ex.Message;

                return RedirectToAction("CPCHub", "Modules");
            }

            return View(rejectedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult WIPProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var wipProposalPacks = new List<CpcProposalPack>();


            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var useremail = HttpContext.Session.Get<string>("userEmail");

                    //var url = _configuration.GetValue<string>("AppSettings:GetWIPProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //wipProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (wipProposalPacks.Count < 1 || wipProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
            }

            return View(wipProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult AcceptedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var acceptedProposalPacks = new List<CpcProposalPack>();


            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    var useremail = HttpContext.Session.Get<string>("userEmail");

                    //var url = _configuration.GetValue<string>("AppSettings:GetAcceptedProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //acceptedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (acceptedProposalPacks.Count < 1 || acceptedProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
            }

            return View(acceptedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult ApprovedProposalPacks()
        {
            var endpointName = RouteData.Values["action"].ToString();

            ConfigureView(endpointName);

            var approvedProposalPacks = new List<CpcProposalPack>();


            if (Convert.ToBoolean(ViewBag.CanViewPermission))
            {
                try
                {
                    //var useremail = HttpContext.Session.Get<string>("userEmail");

                    //var url = _configuration.GetValue<string>("AppSettings:GetApprovedProposalPacks");

                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["useremail"] = userEmail,
                    //};

                    //var uri = QueryHelpers.AddQueryString(url, query);

                    //approvedProposalPacks = await DataServices<List<CpcProposalPack>>.GetPayload(uri, _httpClientFactory);

                    if (approvedProposalPacks.Count < 1 || approvedProposalPacks == null) TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now.ToString(), ex);

                    TempData["Warning"] = $"Fetch {endpointName} \r\n No record was found";
                }
            }

            return View(approvedProposalPacks.AsReadOnly());
        }

        [HttpGet]
        public IActionResult UsersProfile()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult RolesSetting()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult Permissions()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult Forms()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult Branches()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult CPCTypes()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult CPCProducts()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult States()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        [HttpGet]
        public IActionResult SupportingDocuments()
        {
            var endpointName = RouteData.Values["action"].ToString();

            return ConfigureView(endpointName);
        }

        public IActionResult ConfigureView(string formName)
        {
            //var indexVM = new IndexViewModel();

            ViewBag.ShowLayout = true;

            try
            {
                var userEmail = HttpContext.Session.Get<string>("userEmail");

                if (!string.IsNullOrEmpty(userEmail))
                {
                    var user = HttpContext.Session.Get<UserRegisterDto>("UserRegisterDto");

                    if (user != null)
                    {
                        var _permissions = HttpContext.Session.Get<List<PermissionOptions>>("UserPermissions");

                        ViewBag.CanViewPermission = _permissions.Exists(permission => permission.UserEmail.ToLower() == userEmail.ToLower() && permission.Form.ToLower() == formName.ToLower() && permission.Permission.ToLower() == "can_view");

                        if (!Convert.ToBoolean(ViewBag.CanViewPermission)) TempData["Warning"] = $"View {formName} Form \r\n You do not have permission to view {formName} Form";

                        else
                        {
                            ViewBag.CanEditPermission = _permissions.Exists(permission => permission.UserEmail.ToLower() == userEmail.ToLower() && permission.Form.ToLower() == formName.ToLower() && permission.Permission.ToLower() == "can_edit");

                            if (!Convert.ToBoolean(ViewBag.CanEditPermission)) TempData["Warning"] = $"Edit {formName} Form \r\n You do not have permission to Edit {formName}";
                        }

                        ViewBag.Module = user.UserRole.ToLower();

                        ViewBag.Username = user.UserDisplayName;

                        return Ok();
                    }
                    else throw new Exception("No User found.");
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

//    draftsProposalPacks.Add(cp);
//}
//HttpContext.Session.Set("StatusIndex", "NEW");

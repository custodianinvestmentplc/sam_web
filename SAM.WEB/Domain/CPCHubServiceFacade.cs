using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers;
using SAM.WEB.Domain.Options;
using SAM.WEB.Domain.ResponseOptions;
using SAM.WEB.Models;
using SAM.WEB.Payloads;
using SAM.WEB.Services;
using System.Collections.Generic;

namespace SAM.WEB.Domain
{
    public class CPCHubServiceFacade : ICPCHubServices
    {
        private readonly CPCHubManager _cpcManager;

        public CPCHubServiceFacade(string connstring)
        {
            _cpcManager = new CPCHubManager(connstring);
        }

        public List<CPCBranchDto> GetCpcBranches() => _cpcManager.GetAllCpcBranches();

        public List<CpcRoleDto> GetCpcRoles() => _cpcManager.GetAllCpcRoles();

        public List<CpcTemplateDto> GetCpcTemplates(TemplateIdSettingsOptions payload) => _cpcManager.GetAllCpcTemplates(payload);

        public string CreateProposalPack(CreateProposalPackOptions option) => _cpcManager.CreateNewProposalPack(option);

        public string AddNewUser(UserProfileOptions option) => _cpcManager.AddNewUser(option);

        public string AddRoleSettings(AddNewRoleOptions option) => _cpcManager.AddRoleSettings(option);

        public string AddTemplateSettings(AddNewTemplateOptions option) => _cpcManager.AddTemplateSettings(option);

        public void UpdateUserProfile(UserProfileOptions option) => _cpcManager.UpdateUserProfile(option);

        public void EditRoleSettings(AddNewRoleOptions option) => _cpcManager.EditRoleSettings(option);

        public void EditTemplateSettings(AddNewTemplateOptions option) => _cpcManager.EditTemplateSettings(option);

        public CpcProposalPack GetProposalPackByRefNumber(string refNbr) => _cpcManager.GetProposalPackByReferenceNbr(refNbr);

        public UserRegisterDto GetUserProfileByRefNumber(string refNbr) => _cpcManager.GetUserProfileByRefNumber(refNbr);

        public CpcRoleDto GetRoleSettings(string refNbr) => _cpcManager.GetRoleSettings(refNbr);

        public CpcTemplateDto GetTemplateSettings(TemplateIdSettingsOptions payload) => _cpcManager.GetTemplateSettings(payload);

        public List<CpcProposalPack> GetAllDraftProposalPacks() => _cpcManager.FetchAllDraftProposalPacks();

        public List<UserRegisterDto> GetUserProfiles() => _cpcManager.FetchAllUsersProfile();

        public List<CpcProposalPack> GetAllSubmittedProposalPacks() => _cpcManager.FetchAllSubmittedProposalPacks();

        public List<CpcProposalPack> GetAllInboundProposalPacks(string userEmail) => _cpcManager.FetchAllInboundProposalPacks(userEmail);

        public List<CpcProposalPack> GetAllWIPProposalPacks(string userEmail) => _cpcManager.FetchAllWIPProposalPacks(userEmail);

        public List<CpcProposalPack> GetAllAcceptedProposalPacks(string userEmail) => _cpcManager.FetchAllAcceptedProposalPacks(userEmail);

        public List<CpcProposalPack> GetAllApprovedProposalPacks(string userEmail) => _cpcManager.FetchAllApprovedProposalPacks(userEmail);

        public List<ProposalPackContentTypeDto> FetchProposalPackContentTypes() => _cpcManager.FetchProposalPackContentTypes();

        public List<ProposalPackContentDto> GetProposalPackContents(string refNbr) => _cpcManager.GetProposalPackContents(refNbr);

        public bool DeleteProposalPackContent(string refNbr, decimal rowId) => _cpcManager.DeleteProposalPackContent(refNbr, rowId);

        public bool DeleteProposalPackFile(DeleteProposalPackFileRequest payload) => _cpcManager.DeleteProposalPackFile(payload);

        public bool CheckReadOnlyPermission(PermissionOptions permissionOptions) => _cpcManager.CheckReadOnlyPermission(permissionOptions);

        public List<string> GetPermissions(PermissionOptions permissionOptions) => _cpcManager.GetPermissions(permissionOptions);

        public bool DeleteUserProfile(string refNbr, decimal rowId) => _cpcManager.DeleteUserProfile(refNbr, rowId);

        public bool DeleteRoleSettings(decimal refNbr) => _cpcManager.DeleteRoleSettings(refNbr);

        public bool DeleteTemplateSettings(TemplateIdSettings payload) => _cpcManager.DeleteTemplateSettings(payload);

        public AddProposalPackContentResult AddProposalPackContentRecord(AddProposalPackContentRecordOption payload)
        {
            return _cpcManager.AddProposalPackContent(payload);
        }

        public ProposalPackContentDto GetProposalPackContentRecord(string refNbr, decimal recordRowId)
        {
            return _cpcManager.GetProposalPackContentRecord(refNbr, recordRowId);
        }

        public List<CpcProductDto> GetCpcProductList() => _cpcManager.GetAllCpcProducts();

        public List<CpcFileDto> GetCpcFiles() => _cpcManager.GetCpcFiles();

        public List<StatesInNigeriaDto> GetStatesInNigeria() => _cpcManager.GetStatesInNigeria();

        public void SaveProposalFormTradGeneral(ProposalFormTradGeneralOption opt)
        {
            _cpcManager.SaveProposalFormTradGeneral(opt);
        }

        public Step1DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep1(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep1(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradTaxDetails(ProposalFormTradTaxDetails opt)
        {
            _cpcManager.SaveProposalFormTradTaxDetails(opt);
        }

        public Step2DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep2(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep2(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradIdentificationDetails(ProposalFormTradIdentification opt)
        {
            _cpcManager.SaveProposalFormTradIdentificationDetails(opt);
        }

        public Step3DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep3(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep3(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradBankInfo(ProposedFormTradBankInfo opt)
        {
            _cpcManager.SaveProposalFormTradBankInfo(opt);
        }

        public Step4DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep4(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep4(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradMortgageInfo(ProposalFormTradMortgageInfo opt)
        {
            _cpcManager.SaveProposalFormTradMortgageInfo(opt);
        }

        public Step5DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep5(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep5(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradChildrenEducation(ProposedFormTradChildrenEducation opt)
        {
            _cpcManager.SaveProposalFormTradChildrenEducation(opt);
        }

        public Step6DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep6(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep6(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradDigitalPlan(List<NewDigitalPlanNomineeForm> opt, DigitalPlanOperationDetails operation)
        {
            _cpcManager.SaveProposalFormTradDigitalPlan(opt, operation);
        }

        public List<Step7DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep7(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep7(refNbr, contenttypecode);
        }

        public decimal AddBeneficiaryToProposalFormTraditional(AddBeneficiaryOption opt)
        {
            return _cpcManager.AddBeneficiaryToProposalFormTraditional(opt);
        }

        public void DeleteBeneficiaryFromProposalFormTraditional(DeleteBeneficiaryForm payload)
        {
            _cpcManager.DeleteBeneficiaryFromProposalFormTraditional(payload);
        }

        public void SaveDraftBeneficiaryAsActive(SaveDraftBeneficiaryForm payload)
        {
            _cpcManager.SaveDraftBeneficiaryAsActive(payload);
        }

        public List<Step8DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep8(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep8(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradSumAssured(NewDataCaptureSumAssured opt)
        {
            _cpcManager.SaveProposalFormTradSumAssured(opt);
        }

        public Step9DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep9(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep9(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradOtherInsurer(AddOtherInsurerOption data)
        {
            _cpcManager.SaveProposalFormTradOtherInsurer(data);
        }

        public Step10DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep10(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep10(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradMedicalHistory(AddMedicalHistoryOption data)
        {
            _cpcManager.SaveProposalFormTradMedicalHistory(data);
        }

        public Step11DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep11(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep11(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradMisc(AddMiscellaneousOption data)
        {
            _cpcManager.SaveProposalFormTradMisc(data);
        }

        public Step12DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep12(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep12(refNbr, contenttypecode);
        }

        public void SaveProposalFormTradOtherMedicalInfo(AddOtherMedicalInfo data)
        {
            _cpcManager.SaveProposalFormTradOtherMedicalInfo(data);
        }

        public void SaveSupportingDoc(SupportingDocFile data)
        {
            _cpcManager.SaveSupportingDoc(data);
        }

        public Step13DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep13(string refNbr, string contenttypecode)
        {
            return _cpcManager.FindDataCaptureFormTraditionalStep13(refNbr, contenttypecode);
        }

        public SupportingDocFile GetFile(string refNbr, string idType)
        {
            return _cpcManager.GetFile(refNbr, idType);
        }

        public List<SupportingDocFile> GetSupportingDocs(string refNbr)
        {
            return _cpcManager.GetSupportingDocs(refNbr);
        }

        

        public void SubmitProposalPackContent(string refNbr, string contenttypecode)
        {
            _cpcManager.SubmitProposalPackContent(refNbr, contenttypecode);
        }

        public void SubmitProposalPack(SubmitProposalPackForm payload) => _cpcManager.SubmitProposalPack(payload);

        public void PickInboundProposalPack(SubmitProposalPackForm payload) => _cpcManager.PickInboundProposalPack(payload);

        public void AcceptInboundProposalPack(SubmitProposalPackForm payload) => _cpcManager.AcceptInboundProposalPack(payload);

        public void RejectInboundProposalPack(SubmitProposalPackForm payload) => _cpcManager.RejectInboundProposalPack(payload);

        public void PushInboundProposalPack(SubmitProposalPackForm payload) => _cpcManager.PushInboundProposalPack(payload);

        public void ApproveProposalPack(SubmitProposalPackForm payload) => _cpcManager.ApproveProposalPack(payload);
    }
}




//public void SaveSupportingDocs(AddSupportingDocs data, string callerName)
//{
//    _cpcManager.SaveSupportingDocs(data, callerName);
//}

//public Step14DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep14(string refNbr, string contenttypecode)
//{
//    return _cpcManager.FindDataCaptureFormTraditionalStep14(refNbr, contenttypecode);
//}


//public void SaveSupportingPassportDocs(SupportingDocFile data, string callerName)
//{
//    _cpcManager.SaveSupportingPassportDocs(data, callerName);
//}

//public void SaveSupportingUtilityDocs(SupportingDocFile data, string callerName)
//{
//    _cpcManager.SaveSupportingUtilityDocs(data, callerName);
//}


//public SupportingDocFile FindDataCaptureFormTraditionalStep14PassportFile(string refNbr, string contenttypecode)
//{
//    return _cpcManager.FindDataCaptureFormTraditionalStep14PassportFile(refNbr, contenttypecode);
//}

//public SupportingDocFile FindDataCaptureFormTraditionalStep14UtilityFile(string refNbr, string contenttypecode)
//{
//    return _cpcManager.FindDataCaptureFormTraditionalStep14UtilityFile(refNbr, contenttypecode);
//}

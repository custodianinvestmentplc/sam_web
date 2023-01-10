using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Options;
using SAM.WEB.Domain.ResponseOptions;
using SAM.WEB.Models;
using SAM.WEB.Payloads;
using System.Collections.Generic;

namespace SAM.WEB.Services
{
    public interface ICPCHubServices
    {
        List<CPCBranchDto> GetCpcBranches();

        List<CpcRoleDto> GetCpcRoles();

        List<CpcTemplateDto> GetCpcTemplates(TemplateIdSettingsOptions payload);

        string CreateProposalPack(CreateProposalPackOptions option);

        string AddNewUser(UserProfileOptions option);

        string AddRoleSettings(AddNewRoleOptions option);

        string AddTemplateSettings(AddNewTemplateOptions option);

        void UpdateUserProfile(UserProfileOptions option);

        void EditRoleSettings(AddNewRoleOptions option);

        void EditTemplateSettings(AddNewTemplateOptions option);

        CpcProposalPack GetProposalPackByRefNumber(string refNbr);

        UserRegisterDto GetUserProfileByRefNumber(string refNbr);

        CpcRoleDto GetRoleSettings(string refNbr);

        CpcTemplateDto GetTemplateSettings(TemplateIdSettingsOptions payload);
        
        List<CpcProposalPack> GetAllDraftProposalPacks();

        List<UserRegisterDto> GetUserProfiles();

        List<CpcProposalPack> GetAllSubmittedProposalPacks();

        List<CpcProposalPack> GetAllInboundProposalPacks(string userEmail);

        List<CpcProposalPack> GetAllWIPProposalPacks(string userEmail);

        List<CpcProposalPack> GetAllAcceptedProposalPacks(string userEmail);

        List<CpcProposalPack> GetAllApprovedProposalPacks(string userEmail);
        
        List<ProposalPackContentTypeDto> FetchProposalPackContentTypes();
        
        List<ProposalPackContentDto> GetProposalPackContents(string refNbr);
        
        bool DeleteProposalPackContent(string refNbr, decimal rowId);

        bool DeleteProposalPackFile(DeleteProposalPackFileRequest payload);

        bool CheckReadOnlyPermission(PermissionOptions permissionOptions);

        List<string> GetPermissions(PermissionOptions permissionOptions);

        bool DeleteUserProfile(string refNbr, decimal rowId);

        bool DeleteRoleSettings(decimal refNbr);

        bool DeleteTemplateSettings(TemplateIdSettings payload);
        
        AddProposalPackContentResult AddProposalPackContentRecord(AddProposalPackContentRecordOption payload);
        
        ProposalPackContentDto GetProposalPackContentRecord(string refNbr, decimal recordRowId);
        
        List<CpcProductDto> GetCpcProductList();

        List<CpcFileDto> GetCpcFiles();
        
        List<StatesInNigeriaDto> GetStatesInNigeria();
        
        void SaveProposalFormTradGeneral(ProposalFormTradGeneralOption opt);
        
        Step1DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep1(string refNbr, string contenttypecode);
        
        void SaveProposalFormTradTaxDetails(ProposalFormTradTaxDetails opt);
        
        Step2DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep2(string refNbr, string contenttypecode);
        
        void SaveProposalFormTradIdentificationDetails(ProposalFormTradIdentification opt);
        
        Step3DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep3(string refNbr, string contenttypecode);
        
        void SaveProposalFormTradBankInfo(ProposedFormTradBankInfo opt);
        
        Step4DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep4(string refNbr, string contenttypecode);
        
        void SaveProposalFormTradMortgageInfo(ProposalFormTradMortgageInfo opt);
        
        Step5DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep5(string refNbr, string contenttypecode);
        
        void SaveProposalFormTradChildrenEducation(ProposedFormTradChildrenEducation opt);
        
        Step6DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep6(string refNbr, string contenttypecode);
        
        void SaveProposalFormTradDigitalPlan(List<NewDigitalPlanNomineeForm> opt, DigitalPlanOperationDetails operation);
        
        List<Step7DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep7(string refNbr, string contenttypecode);

        decimal AddBeneficiaryToProposalFormTraditional(AddBeneficiaryOption opt);

        void DeleteBeneficiaryFromProposalFormTraditional(DeleteBeneficiaryForm payload);

        void SaveDraftBeneficiaryAsActive(SaveDraftBeneficiaryForm payload);

        List<Step8DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep8(string refNbr, string contenttypecode);

        void SaveProposalFormTradSumAssured(NewDataCaptureSumAssured opt);

        Step9DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep9(string refNbr, string contenttypecode);

        void SaveProposalFormTradOtherInsurer(AddOtherInsurerOption data);

        Step10DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep10(string refNbr, string contenttypecode);

        void SaveProposalFormTradMedicalHistory(AddMedicalHistoryOption data);

        Step11DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep11(string refNbr, string contenttypecode);

        void SaveProposalFormTradMisc(AddMiscellaneousOption data);

        Step12DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep12(string refNbr, string contenttypecode);

        void SaveProposalFormTradOtherMedicalInfo(AddOtherMedicalInfo data);

        void SaveSupportingDoc(SupportingDocFile data);

        Step13DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep13(string refNbr, string contenttypecode);

        SupportingDocFile GetFile(string refNbr, string idType);

        List<SupportingDocFile> GetSupportingDocs(string refNbr);

        void SubmitProposalPackContent(string refNbr, string contenttypecode);

        void SubmitProposalPack(SubmitProposalPackForm payload);

        void PickInboundProposalPack(SubmitProposalPackForm payload);

        void AcceptInboundProposalPack(SubmitProposalPackForm payload);

        void RejectInboundProposalPack(SubmitProposalPackForm payload);

        void PushInboundProposalPack(SubmitProposalPackForm payload);

        void ApproveProposalPack(SubmitProposalPackForm payload);
    }
}

//Step14DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep14(string refNbr, string contenttypecode);

//void SaveSupportingDocs(AddSupportingDocs data, string callerName);

//void SaveSupportingPassportDocs(SupportingDocFile data, string callerName);

//void SaveSupportingUtilityDocs(SupportingDocFile data, string callerName);


//SupportingDocFile FindDataCaptureFormTraditionalStep14PassportFile(string refNbr, string contenttypecode);

//SupportingDocFile FindDataCaptureFormTraditionalStep14UtilityFile(string refNbr, string contenttypecode);

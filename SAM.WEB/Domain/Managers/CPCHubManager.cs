using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.Helpers;
using SAM.WEB.Domain.Options;
using SAM.WEB.Domain.ResponseOptions;
using SAM.WEB.Models;
using SAM.WEB.Payloads;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Managers
{
    public class CPCHubManager
    {
        private readonly string _connectionString;

        public CPCHubManager(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public List<CPCBranchDto> GetAllCpcBranches()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CpcServiceHelper.GetAllCpcBranches(db);

            return lst;
        }

        public List<CpcRoleDto> GetAllCpcRoles()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CpcServiceHelper.GetAllCpcRoles(db);

            return lst;
        }

        public List<CpcTemplateDto> GetAllCpcTemplates(TemplateIdSettingsOptions payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CpcServiceHelper.GetAllCpcTemplates(db, payload);

            return lst;
        }

        public string CreateNewProposalPack(CreateProposalPackOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var proposalRefNbr = CpcServiceHelper.CreateProposalPack(db, option);

            return proposalRefNbr;
        }

        public string AddNewUser(UserProfileOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var UserRefNbr = CpcServiceHelper.AddNewUser(db, option);

            return UserRefNbr;
        }

        public string AddRoleSettings(AddNewRoleOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var RoleRefNbr = CpcServiceHelper.AddRoleSettings(db, option);

            return RoleRefNbr;
        }


        public string AddTemplateSettings(AddNewTemplateOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var RoleRefNbr = CpcServiceHelper.AddTemplateSettings(db, option);

            return RoleRefNbr;
        }


        public void UpdateUserProfile(UserProfileOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.UpdateUserProfile(db, option);
        }

        public void EditRoleSettings(AddNewRoleOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.EditRoleSettings(db, option);
        }

        public void EditTemplateSettings(AddNewTemplateOptions option)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.EditTemplateSettings(db, option);
        }

        public CpcProposalPack GetProposalPackByReferenceNbr(string refNbr)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetProposalPackByRefNumber(db, refNbr);
        }

        public UserRegisterDto GetUserProfileByRefNumber(string refNbr)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetUserProfileByRefNumber(db, refNbr);
        }

        public CpcRoleDto GetRoleSettings(string refNbr)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetRoleSettings(db, refNbr);
        }

        public CpcTemplateDto GetTemplateSettings(TemplateIdSettingsOptions payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetTemplateSettings(db, payload);
        }

        public List<CpcProposalPack> FetchAllDraftProposalPacks()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllDraftProposalPacks(db);
        }

        public List<UserRegisterDto> FetchAllUsersProfile()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllUsersProfile(db);
        }

        public List<CpcProposalPack> FetchAllSubmittedProposalPacks()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllSubmittedProposalPacks(db);
        }

        public List<CpcProposalPack> FetchAllInboundProposalPacks(string userEmail)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllInboundProposalPacks(db, userEmail);
        }

        public List<CpcProposalPack> FetchAllWIPProposalPacks(string userEmail)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllWIPProposalPacks(db, userEmail);
        }

        public List<CpcProposalPack> FetchAllAcceptedProposalPacks(string userEmail)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllAcceptedProposalPacks(db, userEmail);
        }

        public List<CpcProposalPack> FetchAllApprovedProposalPacks(string userEmail)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchAllApprovedProposalPacks(db, userEmail);
        }

        public List<ProposalPackContentTypeDto> FetchProposalPackContentTypes()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FetchProposalPackContentTypes(db);
        }

        public List<ProposalPackContentDto> GetProposalPackContents(string refNbr)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetProposalPackContentList(db, refNbr);
        }

        public bool DeleteProposalPackContent(string refNbr, decimal rowId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.DeleteProposalPackContent(db, refNbr, rowId);
        }

        public bool DeleteProposalPackFile(DeleteProposalPackFileRequest payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.DeleteProposalPackFile(db, payload);
        }
        
        public bool CheckReadOnlyPermission(PermissionOptions permissionOptions)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.CheckReadOnlyPermission(db, permissionOptions);
        }

        public List<string> GetPermissions(PermissionOptions permissionOptions)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetPermissions(db, permissionOptions);
        }

        public bool DeleteUserProfile(string refNbr, decimal rowId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.DeleteUserProfile(db, refNbr, rowId);
        }

        public bool DeleteRoleSettings(decimal refNbr)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.DeleteRoleSettings(db, refNbr);
        }

        public bool DeleteTemplateSettings(TemplateIdSettings payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.DeleteTemplateSettings(db, payload);
        }

        public AddProposalPackContentResult AddProposalPackContent(AddProposalPackContentRecordOption addOption)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var result = CpcServiceHelper.AddProposalPackContentRecord(db, addOption);

            return result;
        }

        public ProposalPackContentDto GetProposalPackContentRecord(string refNbr, decimal rowId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var result = CpcServiceHelper.GetProposalPackContentRecord(db, refNbr, rowId);

            return result;
        }

        public List<CpcProductDto> GetAllCpcProducts()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var prodLst = CpcServiceHelper.GetAllProducts(db);

            return prodLst;
        }

        public List<CpcFileDto> GetCpcFiles()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var fileLst = CpcServiceHelper.GetCpcFiles(db);

            return fileLst;
        }
        
        public List<StatesInNigeriaDto> GetStatesInNigeria()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.GetStatesInNigeria(db);
        }

        public void SaveProposalFormTradGeneral(ProposalFormTradGeneralOption opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradGeneral(db, opt);
        }

        public Step1DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep1(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep1(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradTaxDetails(ProposalFormTradTaxDetails opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradTaxDetails(db, opt);
        }

        public Step2DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep2(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep2(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradIdentificationDetails(ProposalFormTradIdentification opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradIdentificationDetails(db, opt);
        }

        public Step3DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep3(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep3(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradBankInfo(ProposedFormTradBankInfo opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradBankInfo(db, opt);
        }

        public Step4DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep4(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep4(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradMortgageInfo(ProposalFormTradMortgageInfo opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradMortgageInfo(db, opt);
        }

        public Step5DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep5(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep5(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradChildrenEducation(ProposedFormTradChildrenEducation opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradChildrenEducation(db, opt);
        }

        public Step6DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep6(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep6(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradDigitalPlan(List<NewDigitalPlanNomineeForm> opt, DigitalPlanOperationDetails operations)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradDigitalPlan(db, opt, operations.UserEmail, operations.IsApplicable, operations.ReferenceNbr, operations.ContentTypeCode);
        }

        public List<Step7DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep7(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.FindDataCaptureFormTraditionalStep7(db, refNbr, contenttypecode);
        }

        public decimal AddBeneficiaryToProposalFormTraditional(AddBeneficiaryOption opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CpcServiceHelper.AddBeneficiaryToProposalFormTraditional(db, opt);
        }

        public void DeleteBeneficiaryFromProposalFormTraditional(DeleteBeneficiaryForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.DeleteBeneficiaryFromProposalFormTraditional(db, payload);
        }

        public void SaveDraftBeneficiaryAsActive(SaveDraftBeneficiaryForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SaveDraftBeneficiaryAsActive(db, payload);
        }

        public List<Step8DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep8(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.FindDataCaptureFormTraditionalStep8(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradSumAssured(NewDataCaptureSumAssured opt)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            CpcServiceHelper.SaveProposalFormTradSumAssured(db, opt);
        }

        public Step9DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep9(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.FindDataCaptureFormTraditionalStep9(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradOtherInsurer(AddOtherInsurerOption data)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SaveProposalFormTradOtherInsurer(db, data);
        }

        public Step10DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep10(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.FindDataCaptureFormTraditionalStep10(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradMedicalHistory(AddMedicalHistoryOption data)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SaveProposalFormTradMedicalHistory(db, data);
        }

        public Step11DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep11(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.FindDataCaptureFormTraditionalStep11(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradMisc(AddMiscellaneousOption data)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SaveProposalFormTradMisc(db, data);
        }

        public Step12DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep12(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.FindDataCaptureFormTraditionalStep12(db, refNbr, contenttypecode);
        }

        public void SaveProposalFormTradOtherMedicalInfo(AddOtherMedicalInfo data)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SaveProposalFormTradOtherMedicalInfo(db, data);
        }

        public void SaveSupportingDoc(SupportingDocFile data)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SaveSupportingDoc(db, data);
        }

        public Step13DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep13(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.FindDataCaptureFormTraditionalStep13(db, refNbr, contenttypecode);
        }

        public SupportingDocFile GetFile(string refNbr, string idType)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.GetFile(db, refNbr, idType);
        }

        public List<SupportingDocFile> GetSupportingDocs(string refNbr)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return CpcServiceHelper.GetSupportingDocs(db, refNbr);
        }
        
        public void SubmitProposalPackContent(string refNbr, string contenttypecode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SubmitProposalPackContent(db, refNbr, contenttypecode);
        }

        public void SubmitProposalPack(SubmitProposalPackForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.SubmitProposalPack(db, payload);
        }

        public void PickInboundProposalPack(SubmitProposalPackForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.PickInboundProposalPack(db, payload);
        }

        public void AcceptInboundProposalPack(SubmitProposalPackForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.AcceptInboundProposalPack(db, payload);
        }

        public void RejectInboundProposalPack(SubmitProposalPackForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.RejectInboundProposalPack(db, payload);
        }

        public void PushInboundProposalPack(SubmitProposalPackForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.PushInboundProposalPack(db, payload);
        }

        public void ApproveProposalPack(SubmitProposalPackForm payload)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CpcServiceHelper.ApproveProposalPack(db, payload);
        }
    }
}


//public void SaveSupportingDocs(AddSupportingDocs data, string callerName)
//{
//    using var db = DatabaseHelper.OpenDatabase(_connectionString);
//    CpcServiceHelper.SaveSupportingIdDocs(db, data, callerName);
//}

//public Step14DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep14(string refNbr, string contenttypecode)
//{
//    using var db = DatabaseHelper.OpenDatabase(_connectionString);
//    return CpcServiceHelper.FindDataCaptureFormTraditionalStep14(db, refNbr, contenttypecode);
//}


//public void SaveSupportingPassportDocs(SupportingDocFile data, string callerName)
//{
//    using var db = DatabaseHelper.OpenDatabase(_connectionString);
//    CpcServiceHelper.SaveSupportingPassportDocs(db, data, callerName);
//}

//public void SaveSupportingUtilityDocs(SupportingDocFile data, string callerName)
//{
//    using var db = DatabaseHelper.OpenDatabase(_connectionString);
//    CpcServiceHelper.SaveSupportingUtilityDocs(db, data, callerName);
//}


//public SupportingDocFile FindDataCaptureFormTraditionalStep14PassportFile(string refNbr, string contenttypecode)
//{
//    using var db = DatabaseHelper.OpenDatabase(_connectionString);
//    return CpcServiceHelper.FindDataCaptureFormTraditionalStep14PassportFile(db, refNbr, contenttypecode);
//}

//public SupportingDocFile FindDataCaptureFormTraditionalStep14UtilityFile(string refNbr, string contenttypecode)
//{
//    using var db = DatabaseHelper.OpenDatabase(_connectionString);
//    return CpcServiceHelper.FindDataCaptureFormTraditionalStep14UtilityFile(db, refNbr, contenttypecode);
//}
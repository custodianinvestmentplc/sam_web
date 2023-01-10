using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.LocalServices;
using SAM.WEB.Domain.Options;
using SAM.WEB.Domain.ResponseOptions;
using SAM.WEB.Models;
using SAM.WEB.Payloads;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class CpcServiceHelper
    {
        public static List<CPCBranchDto> GetAllCpcBranches(IDbConnection db)
        {
            var sp = "dbo.get_all_cpc_branches";

            var moduleLst = DbServer.LoadData<CPCBranchDto>(db, sp, null);

            return moduleLst;
        }

        public static List<CpcRoleDto> GetAllCpcRoles(IDbConnection db)
        {
            var sp = "dbo.get_all_cpc_roles";

            var moduleLst = DbServer.LoadData<CpcRoleDto>(db, sp, null);

            return moduleLst;
        }

        public static List<CpcTemplateDto> GetAllCpcTemplates(IDbConnection db, TemplateIdSettingsOptions payload)
        {
            var sp = "dbo.get_all_cpc_templates";
            var prm = new DynamicParameters();
            prm.Add("@template_type", payload.TemplateType);
            prm.Add("@user_email", payload.AddedBy);

            var moduleLst = DbServer.LoadData<CpcTemplateDto>(db, sp, prm);

            return moduleLst;
        }

        public static string CreateProposalPack(IDbConnection db, CreateProposalPackOptions option)
        {
            var sp = "dbo.cpc_create_proposal_pack";
            var prm = new DynamicParameters();

            prm.Add("@caller", option.CallerEmail);
            prm.Add("@title", option.ProposalTitle);
            prm.Add("@branch_code", option.InitiatingBranchCode);
            prm.Add("@agent_code", option.InitiatingAgentCode);
            prm.Add("@proposal_type", option.ProposalPackType);
            prm.Add("@ref_nbr", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            DbServer.SaveData(db, sp, prm);

            return prm.Get<string>("@ref_nbr");
        }

        public static string AddNewUser(IDbConnection db, UserProfileOptions option)
        {
            var sp = "dbo.cpc_add_new_user";
            var prm = new DynamicParameters();

            prm.Add("@added_by", option.AddedBy);
            prm.Add("@email", option.UserEmail);
            prm.Add("@display_name", option.DisplayName);
            prm.Add("@role_code", option.RoleCode);
            prm.Add("@branch_code", option.BranchCode);
            //prm.Add("@agent_code", option.AgentCode);
            prm.Add("@agent_position", option.AgentPosition);
            prm.Add("@agent_description", option.AgentDescription);
            prm.Add("@ref_nbr", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            DbServer.SaveData(db, sp, prm);

            return prm.Get<string>("@ref_nbr");
        }

        public static string AddRoleSettings(IDbConnection db, AddNewRoleOptions option)
        {
            var sp = "dbo.cpc_add_role";
            var prm = new DynamicParameters();

            prm.Add("@added_by", option.AddedBy);
            prm.Add("@role", option.Role);

            prm.Add("@new_role_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            DbServer.SaveData(db, sp, prm);

            return prm.Get<string>("@new_role_name");
        }

        public static string AddTemplateSettings(IDbConnection db, AddNewTemplateOptions option)
        {
            var sp = "dbo.cpc_add_template";
            var prm = new DynamicParameters();

            prm.Add("@ref_type_ref_code", option.RefTypeRefCode);
            prm.Add("@added_by", option.AddedBy);
            prm.Add("@template", option.Template);
            prm.Add("@template_type", option.TemplateType);
            prm.Add("@ref_type_refNbr_desc", option.RefTypeRefCodeDesc);
            prm.Add("@class_code", option.ClassCode);
            prm.Add("@class_code_desc", option.ClassCodeDesc);
            prm.Add("@template_desc", option.TemplateDesc);
            prm.Add("@template_short_desc", option.TemplateShortDesc);
            prm.Add("@template_email", option.TemplateEmail);

            prm.Add("@new_template_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            DbServer.SaveData(db, sp, prm);

            return prm.Get<string>("@new_template_name");
        }

        public static void UpdateUserProfile(IDbConnection db, UserProfileOptions option)
        {
            var sp = "dbo.cpc_update_user_profile";
            var prm = new DynamicParameters();

            prm.Add("@added_by", option.AddedBy);
            prm.Add("@email", option.UserEmail);
            prm.Add("@display_name", option.DisplayName);
            prm.Add("@role_code", option.RoleCode);
            prm.Add("@branch_code", option.BranchCode);
            //prm.Add("@agent_code", option.AgentCode);
            prm.Add("@agent_position", option.AgentPosition);
            prm.Add("@agent_description", option.AgentDescription);
            prm.Add("@ref_nbr", option.ReferenceNumber);

            DbServer.SaveData(db, sp, prm);
        }

        public static void EditRoleSettings(IDbConnection db, AddNewRoleOptions option)
        {
            var sp = "dbo.cpc_edit_role";
            var prm = new DynamicParameters();

            prm.Add("@updated_by", option.AddedBy);
            prm.Add("@role", option.Role);
            prm.Add("@ref_nbr", option.ReferenceNumber);

            DbServer.SaveData(db, sp, prm);
        }

        public static void EditTemplateSettings(IDbConnection db, AddNewTemplateOptions option)
        {
            var sp = "dbo.cpc_edit_template";
            var prm = new DynamicParameters();

            prm.Add("@updated_by", option.AddedBy);

            prm.Add("@ref_type_refNbr", option.RefTypeRefCode);
            prm.Add("@ref_type_refNbr_desc", option.RefTypeRefCodeDesc);
            prm.Add("@template", option.Template);
            prm.Add("@template_type", option.TemplateType);
            prm.Add("@ref_nbr", option.ReferenceNumber);
            prm.Add("@class_code", option.ClassCode);
            prm.Add("@class_code_desc", option.ClassCodeDesc);
            prm.Add("@template_desc", option.TemplateDesc);
            prm.Add("@template_short_desc", option.TemplateShortDesc);
            prm.Add("@template_email", option.TemplateEmail);

            DbServer.SaveData(db, sp, prm);
        }

        public static CpcProposalPack GetProposalPackByRefNumber(IDbConnection db, string refnbr)
        {
            var sp = "dbo.cpc_get_proposal_pack_by_ref_nbr";
            var prm = new DynamicParameters();

            prm.Add("@ref_nbr", refnbr);

            var lst = DbServer.LoadData<CpcProposalPack>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            throw new System.Exception($"Cannot find Proposal Pack with Reference Number: {refnbr}");
        }

        public static UserRegisterDto GetUserProfileByRefNumber(IDbConnection db, string refnbr)
        {
            var sp = "dbo.cpc_get_user_profile_by_ref_nbr";
            var prm = new DynamicParameters();

            prm.Add("@ref_nbr", refnbr);

            var lst = DbServer.LoadData<UserRegisterDto>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            throw new System.Exception($"Cannot find User Profile with Reference Number: {refnbr}");
        }

        public static CpcRoleDto GetRoleSettings(IDbConnection db, string refnbr)
        {
            var sp = "dbo.cpc_get_role";
            var prm = new DynamicParameters();

            prm.Add("@ref_nbr", refnbr);

            var lst = DbServer.LoadData<CpcRoleDto>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            throw new System.Exception("Cannot find Role Settings");
        }

        public static CpcTemplateDto GetTemplateSettings(IDbConnection db, TemplateIdSettingsOptions payload)
        {
            var sp = "dbo.cpc_get_template";
            var prm = new DynamicParameters();

            prm.Add("@ref_nbr", payload.ReferenceNbr);
            prm.Add("@template_type", payload.TemplateType);
            prm.Add("@user_email", payload.AddedBy);

            var lst = DbServer.LoadData<CpcTemplateDto>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            throw new System.Exception($"Cannot find record Settings");
        }

        public static List<CpcProposalPack> FetchAllDraftProposalPacks(IDbConnection db)
        {
            var sp = "dbo.cpc_get_draft_proposal_pack_all";

            var model = DbServer.LoadData<CpcProposalPack>(db, sp, null);

            return model;
        }

        public static List<UserRegisterDto> FetchAllUsersProfile(IDbConnection db)
        {
            var sp = "dbo.cpc_get_users_profile_all";

            return DbServer.LoadData<UserRegisterDto>(db, sp, null);
        }

        public static List<CpcProposalPack> FetchAllSubmittedProposalPacks(IDbConnection db)
        {
            var sp = "dbo.cpc_get_submitted_proposal_pack_all";

            return DbServer.LoadData<CpcProposalPack>(db, sp, null);
        }

        public static List<CpcProposalPack> FetchAllInboundProposalPacks(IDbConnection db, string userEmail)
        {
            var sp = "dbo.cpc_get_inbound_proposal_packs";
            var prm = new DynamicParameters();

            prm.Add("@user_email", userEmail);

            return DbServer.LoadData<CpcProposalPack>(db, sp, prm);
        }

        public static List<CpcProposalPack> FetchAllWIPProposalPacks(IDbConnection db, string userEmail)
        {
            var sp = "dbo.cpc_get_wip_proposal_packs";
            var prm = new DynamicParameters();

            prm.Add("@user_email", userEmail);

            return DbServer.LoadData<CpcProposalPack>(db, sp, prm);
        }

        public static List<CpcProposalPack> FetchAllAcceptedProposalPacks(IDbConnection db, string userEmail)
        {
            var sp = "dbo.cpc_get_accepted_proposal_packs";
            var prm = new DynamicParameters();

            prm.Add("@user_email", userEmail);

            return DbServer.LoadData<CpcProposalPack>(db, sp, null);
        }

        public static List<CpcProposalPack> FetchAllApprovedProposalPacks(IDbConnection db, string userEmail)
        {
            var sp = "dbo.cpc_get_approved_proposal_packs";
            var prm = new DynamicParameters();

            prm.Add("@user_email", userEmail);

            return DbServer.LoadData<CpcProposalPack>(db, sp, null);
        }

        public static List<ProposalPackContentTypeDto> FetchProposalPackContentTypes(IDbConnection db)
        {
            var sp = "dbo.cpc_fetch_proposal_pack_content_types";

            return DbServer.LoadData<ProposalPackContentTypeDto>(db, sp, null);
        }

        public static List<ProposalPackContentDto> GetProposalPackContentList(IDbConnection db, string refNbr)
        {
            var sp = "dbo.cpc_get_proposal_pack_contents";
            var prm = new DynamicParameters();

            prm.Add("@proposal_ref_nbr", refNbr);

            return DbServer.LoadData<ProposalPackContentDto>(db, sp, prm);
        }

        public static bool DeleteProposalPackContent(IDbConnection db, string refNbr, decimal rowId)
        {
            var sp = "dbo.cpc_delete_proposal_pack_content";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@row_id", rowId);
            param.Add("@is_deleted", dbType: DbType.Int32, direction: ParameterDirection.Output, size: Int32.MaxValue);

            DbServer.SaveData(db, sp, param);

            if (param.Get<int>("@is_deleted") == 1)
            {
                return true;
            }

            return false;
        }

        public static bool DeleteProposalPackFile(IDbConnection db, DeleteProposalPackFileRequest payload)
        {
            var sp = "dbo.DeleteProposalPackFile";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ProposalPackReferenceNbr);
            param.Add("@doc_type", payload.proposalPackDocType);
            param.Add("@is_deleted", dbType: DbType.Int32, direction: ParameterDirection.Output, size: Int32.MaxValue);

            DbServer.SaveData(db, sp, param);

            if (param.Get<int>("@is_deleted") == 1)
            {
                return true;
            }

            return false;
        }
        
        public static bool CheckReadOnlyPermission(IDbConnection db, PermissionOptions permissionOptions)
        {
            var sp = "dbo.check_readOnly_permission";
            var param = new DynamicParameters();

            param.Add("@permission", permissionOptions.Permission);
            param.Add("@user_email", permissionOptions.UserEmail);
            param.Add("@form", permissionOptions.Form);
            param.Add("@is_readOnly", dbType: DbType.Int32, direction: ParameterDirection.Output, size: Int32.MaxValue);

            DbServer.SaveData(db, sp, param);

            var isROR = param.Get<int>("@is_readOnly");

            if (param.Get<int>("@is_readOnly") == 1)
            {
                return true;
            }

            return false;
        }

        public static List<string> GetPermissions(IDbConnection db, PermissionOptions permissionOptions)
        {
            var sp = "dbo.get_permissions";
            var param = new DynamicParameters();

            param.Add("@user_email", permissionOptions.UserEmail);
            param.Add("@form", permissionOptions.Form);
            //param.Add("@tbl_permissions", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            var isROR = DbServer.LoadData<string>(db, sp, param);

            return isROR;
        }

        public static bool DeleteUserProfile(IDbConnection db, string refNbr, decimal rowId)
        {
            var sp = "dbo.cpc_delete_user_profile";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@row_id", rowId);
            param.Add("@is_deleted", dbType: DbType.Int32, direction: ParameterDirection.Output, size: Int32.MaxValue);

            DbServer.SaveData(db, sp, param);

            if (param.Get<int>("@is_deleted") == 1)
            {
                return true;
            }

            return false;
        }

        public static bool DeleteRoleSettings(IDbConnection db, decimal refNbr)
        {
            var sp = "dbo.cpc_delete_role";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@is_deleted", dbType: DbType.Int32, direction: ParameterDirection.Output, size: Int32.MaxValue);

            DbServer.SaveData(db, sp, param);

            if (param.Get<int>("@is_deleted") == 1)
            {
                return true;
            }

            return false;
        }

        public static bool DeleteTemplateSettings(IDbConnection db, TemplateIdSettings payload)
        {
            var sp = "dbo.cpc_delete_template";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@template_type", payload.TemplateType);
            param.Add("@user_email", payload.UserEmail);
            param.Add("@is_deleted", dbType: DbType.Int32, direction: ParameterDirection.Output, size: Int32.MaxValue);

            DbServer.SaveData(db, sp, param);

            if (param.Get<int>("@is_deleted") == 1)
            {
                return true;
            }

            return false;
        }

        public static ProposalPackContentDto GetProposalPackContentRecord(IDbConnection db, string refNbr, decimal rowId)
        {
            var sp = "dbo.cpc_get_proposal_pack_content_record";
            var param = new DynamicParameters();

            param.Add("@proposal_ref_nbr", refNbr);
            param.Add("@row_id", rowId);

            var result = DbServer.LoadData<ProposalPackContentDto>(db, sp, param);

            if (result != null && result.Count == 1)
            {
                return result[0];
            }

            return null;
        }

        public static AddProposalPackContentResult AddProposalPackContentRecord(IDbConnection db, AddProposalPackContentRecordOption option)
        {
            var sp = "dbo.cpc_add_proposal_pack_content";
            var param = new DynamicParameters();
            var opsResult = new AddProposalPackContentResult();

            param.Add("@ref_nbr", option.ReferenceNumber);
            param.Add("@content_type_code", option.ContentTypeCode);
            param.Add("@caller", option.CallerEmail);
            param.Add("@new_row_id", dbType: DbType.Decimal, direction: ParameterDirection.Output, size: 25);
            param.Add("@msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 150);

            DbServer.SaveData(db, sp, param);

            opsResult.NewRecordId = param.Get<decimal>("@new_row_id");
            opsResult.Message = param.Get<string>("@msg");

            return opsResult;
        }

        public static List<CpcProductDto> GetAllProducts(IDbConnection db)
        {
            var sp = "dbo.cpc_get_all_products";

            return DbServer.LoadData<CpcProductDto>(db, sp, null);
        }

        public static List<CpcFileDto> GetCpcFiles(IDbConnection db)
        {
            var sp = "dbo.cpc_get_all_files";

            return DbServer.LoadData<CpcFileDto>(db, sp, null);
        }
        
        public static List<StatesInNigeriaDto> GetStatesInNigeria(IDbConnection db)
        {
            var sp = "dbo.cpc_get_states";

            return DbServer.LoadData<StatesInNigeriaDto>(db, sp, null);
        }

        public static void SaveProposalFormTradGeneral(IDbConnection db, ProposalFormTradGeneralOption opt)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_GENERAL";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("SALUTATION_TITLE_CODE", typeof(string));
            dt.Columns.Add("SALUTATION_OTHERS", typeof(string));
            dt.Columns.Add("PRODUCT_CODE", typeof(string));
            dt.Columns.Add("FIRST_NAME", typeof(string));
            dt.Columns.Add("MIDDLE_NAME", typeof(string));
            dt.Columns.Add("SURNAME", typeof(string));
            dt.Columns.Add("DATE_OF_BIRTH", typeof(string));
            dt.Columns.Add("GENDER", typeof(string));
            dt.Columns.Add("GENDER_OTHERS", typeof(string));
            dt.Columns.Add("STATE_OF_ORIGIN_CODE", typeof(string));
            dt.Columns.Add("NATIONALITY_CODE", typeof(string));
            dt.Columns.Add("COUNTRY_OF_ORIGIN", typeof(string));
            dt.Columns.Add("TOWN_OR_CITY_OF_BIRTH", typeof(string));
            dt.Columns.Add("COUNTRY_OF_BIRTH", typeof(string));
            dt.Columns.Add("RESIDENTIAL_ADDRESS", typeof(string));
            dt.Columns.Add("ADDRESS_TOWN", typeof(string));
            dt.Columns.Add("ADDRESS_CITY", typeof(string));
            dt.Columns.Add("ADDRESS_STATE", typeof(string));
            dt.Columns.Add("ADDRESS_COUNTRY", typeof(string));

            dt.Rows.Add(
                opt.ReferenceNbr,
                opt.ContentTypeCode,
                opt.Title,
                opt.OtherTitle,
                opt.ProductCode,
                opt.Firstname,
                opt.Middlename,
                opt.Surname,
                opt.Dob,
                opt.Gender,
                opt.GenderOthers,
                opt.StateOfOrigin,
                opt.Nationality,
                opt.CountryOfAddress,
                opt.TownOrCityOfBirth,
                opt.CountryOfBirth,
                opt.Address,
                opt.TownOfAddress,
                opt.CityOfAddress,
                opt.StateOfAddress,
                opt.CountryOfAddress
            );

            param.Add("@caller", opt.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_GENERAL"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step1DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep1(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_1";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step1DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradTaxDetails(IDbConnection db, ProposalFormTradTaxDetails data)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_TAX_INFO";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("TIN", typeof(string));
            dt.Columns.Add("EMPLOYER_NAME", typeof(string));
            dt.Columns.Add("EMPLOYER_ADDRESS", typeof(string));
            dt.Columns.Add("EMPLOYER_EMAIL", typeof(string));
            dt.Columns.Add("EMPLOYER_PHONE_NBR", typeof(string));
            dt.Columns.Add("CUST_RELIGION", typeof(string));
            dt.Columns.Add("CUST_OCCUPATION", typeof(string));
            dt.Columns.Add("CUST_SOURCE_OF_FUNDS", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                data.Tin,
                data.Employer,
                data.EmployerAddress,
                data.EmployerEmail,
                data.EmployerTelephone,
                data.Religion,
                data.CustomerOccupation,
                data.CustomerSourceOfFunds
            );

            param.Add("@caller", data.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_TAX_INFO"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step2DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep2(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_2";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step2DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradIdentificationDetails(IDbConnection db, ProposalFormTradIdentification data)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_IDENTIFICATION";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("MEANS_OF_IDENTIFICATION", typeof(string));
            dt.Columns.Add("MEANS_OF_IDENTIFICATION_OTHERS", typeof(string));
            dt.Columns.Add("IDENTIFICATION_NBR", typeof(string));
            dt.Columns.Add("COUNTRY_OF_ISSUE", typeof(string));
            dt.Columns.Add("COUNTRY_OF_ISSUE_OTHERS", typeof(string));
            dt.Columns.Add("ISSUING_AUTHORITY", typeof(string));
            dt.Columns.Add("DATE_ISSUED", typeof(string));
            dt.Columns.Add("DATE_EXPIRED", typeof(string));
            dt.Columns.Add("RESIDENTIAL_PERMIT_NBR", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                data.MeansOfIdentification,
                data.MeansOfidentificationOthers,
                data.IdentifiationNbr,
                data.IdCountryOfIssue,
                data.IdCountryOfIssueOthers,
                data.IdIssuingAuthrity,
                data.IdIssueDate,
                data.IdExpiryDate,
                data.ResidentPermitNbr
            );

            param.Add("@caller", data.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_IDENTITY"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step3DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep3(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_3";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step3DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradBankInfo(IDbConnection db, ProposedFormTradBankInfo data)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_BANK_INFO";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("BANK_NAME", typeof(string));
            dt.Columns.Add("ACCOUNT_NUMBER", typeof(string));
            dt.Columns.Add("ACCOUNT_NAME", typeof(string));
            dt.Columns.Add("BVN", typeof(string));
            dt.Columns.Add("BANK_PRODUCT_NAME", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                data.BankName,
                data.AccountNumber,
                data.AccountName,
                data.Bvn,
                data.BankProductName
            );

            param.Add("@caller", data.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_BANK_INFO"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step4DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep4(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_4";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step4DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradMortgageInfo(IDbConnection db, ProposalFormTradMortgageInfo data)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_MORTGAGE_INFO";
            var param = new DynamicParameters();

            param.Add("@caller", data.UserEmail);
            param.Add("@ref_nbr", data.ReferenceNbr);
            param.Add("@content_code", data.ContentTypeCode);
            param.Add("@mortgage_name", data.MortgageName);
            param.Add("@mortgage_address", data.MortgageAddress);
            param.Add("@interst_rate", data.InterestRate);
            param.Add("@action_code", data.IsApplicable ? 1 : 2);

            DbServer.SaveData(db, sp, param);
        }

        public static Step5DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep5(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_5";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step5DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradChildrenEducation(IDbConnection db, ProposedFormTradChildrenEducation data)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_CHILDREN_EDUCATION";
            var param = new DynamicParameters();

            param.Add("@caller", data.UserEmail);
            param.Add("@ref_nbr", data.ReferenceNbr);
            param.Add("@content_code", data.ContentTypeCode);
            param.Add("@school_fees_amt", data.SchoolFeesAmount);
            param.Add("@benefit_pct", data.BenefitPercent);
            param.Add("@action_code", data.IsApplicable ? 1 : 2);

            DbServer.SaveData(db, sp, param);
        }

        public static Step6DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep6(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_6";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step6DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradDigitalPlan(IDbConnection db, List<NewDigitalPlanNomineeForm> data, string callerName, bool isApplicable, string refNbr, string contenttype)
        {
            var sp = "dbo.CPC_ADD_UPDATE_PROPOSAL_FORM_TRAD_DIGITAL_PLAN";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("NOMINEE_NAME", typeof(string));
            dt.Columns.Add("NOMINEE_DOB", typeof(string));
            dt.Columns.Add("RELATIONSHIP", typeof(string));
            dt.Columns.Add("NOMINEE_SUM_ASSURED", typeof(decimal));

            foreach (var item in data)
            {
                dt.Rows.Add(item.ReferenceNbr, item.ContentTypeCode, item.NomineeName, item.NomineeDob, item.NomineeRelationship, item.SumAssured);
            }

            param.Add("@caller", callerName);
            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttype);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_DIGITAL_PLAN"));
            param.Add("@action_code", isApplicable ? 1 : 2);

            DbServer.SaveData(db, sp, param);
        }

        public static List<Step7DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep7(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_7";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            return DbServer.LoadData<Step7DataCaptureFormTraditionalDto>(db, sp, param);
        }

        public static decimal AddBeneficiaryToProposalFormTraditional(IDbConnection db, AddBeneficiaryOption opt)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_BENEFICIARY";
            var param = new DynamicParameters();

            param.Add("@caller", opt.Caller);
            param.Add("@ref_nbr", opt.ReferenceNumber);
            param.Add("@content_type_code", opt.ContentTypeCode);
            param.Add("@beneficiary_name", opt.BeneficiaryName);
            param.Add("@dob", opt.Dob);
            param.Add("@relation", opt.Relationship);
            param.Add("@proportion_pct", opt.ProportionPercent);
            param.Add("@new_beneficiary_id", direction: ParameterDirection.Output, dbType: DbType.Decimal);

            DbServer.SaveData(db, sp, param);

            var newId = param.Get<decimal>("@new_beneficiary_id");

            return newId;
        }

        public static void DeleteBeneficiaryFromProposalFormTraditional(IDbConnection db, DeleteBeneficiaryForm payload)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_DELETE_BENEFICIARY";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNumber);
            param.Add("@content_type_code", payload.ContentTypeCode);
            param.Add("@beneficiary_row_id", payload.RecordRowId);

            DbServer.SaveData(db, sp, param);
        }

        public static void SaveDraftBeneficiaryAsActive(IDbConnection db, SaveDraftBeneficiaryForm payload)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_SAVE_DRAFT_BENEFICIARY";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNumber);
            param.Add("@content_type_code", payload.ContentTypeCode);
            param.Add("@caller", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }

        public static List<Step8DataCaptureFormTraditionalDto> FindDataCaptureFormTraditionalStep8(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_8";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            return DbServer.LoadData<Step8DataCaptureFormTraditionalDto>(db, sp, param);
        }

        public static void SaveProposalFormTradSumAssured(IDbConnection db, NewDataCaptureSumAssured data)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_SUM_ASSURED";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("SUM_ASSURED", typeof(decimal));
            dt.Columns.Add("TERM_YEAR", typeof(int));
            dt.Columns.Add("FIRST_PREMIUM_PAID", typeof(decimal));
            dt.Columns.Add("REGULAR_PREMIUM", typeof(decimal));
            dt.Columns.Add("PAYMENT_FREQUENCY", typeof(string));
            dt.Columns.Add("FROM_DATE", typeof(string));
            dt.Columns.Add("MATURIRY_DATE", typeof(string));
            dt.Columns.Add("PAYMENT_MODE", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                data.SumAssured,
                data.TermYear,
                data.FirstPremiumPaid,
                data.RegularPremium,
                data.PaymentFrequency,
                data.FromDate,
                data.MaturityDate,
                data.PaymentMode
            );

            param.Add("@caller", data.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_SUM_ASSURED"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step9DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep9(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_9";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step9DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradOtherInsurer(IDbConnection db, AddOtherInsurerOption data)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_OTHER_INSURER";
            var param = new DynamicParameters();

            param.Add("@caller", data.UserEmail);
            param.Add("@ref_nbr", data.ReferenceNbr);
            param.Add("@content_type", data.ContentTypeCode);
            param.Add("@has_other_insurer", data.HasOtherInsurer);
            param.Add("@other_insurer_name", data.OtherInsurerName);
            param.Add("@other_sum_assured", data.OtherSumAssured);
            param.Add("@prior_decline", data.PriorProposalDecline);
            param.Add("@decline_reason", data.DeclineReason);

            DbServer.SaveData(db, sp, param);
        }

        public static Step10DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep10(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_10";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step10DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradMedicalHistory(IDbConnection db, AddMedicalHistoryOption data)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_MEDICAL_HISTORY";
            var param = new DynamicParameters();

            param.Add("@caller", data.CallerEmail);
            param.Add("@ref_nbr", data.ReferenceNbr);
            param.Add("@content_type", data.ContentTypeCode);
            param.Add("@hospital_name", data.HospitalName);
            param.Add("@hospital_address", data.HospitalAddress);
            param.Add("@height", data.HeightInMeters);
            param.Add("@weight", data.WeightInKg);

            DbServer.SaveData(db, sp, param);
        }

        public static Step11DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep11(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_11";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step11DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradMisc(IDbConnection db, AddMiscellaneousOption data)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_MISC";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("SICK_OR_MEDICATION", typeof(string));
            dt.Columns.Add("MEDICATION_DETAILS", typeof(string));
            dt.Columns.Add("IS_PREGNANT", typeof(string));
            dt.Columns.Add("EXPECTED_DELIVERY_MONTH", typeof(string));
            dt.Columns.Add("SMOKED", typeof(string));
            dt.Columns.Add("TUBERCULOSIS", typeof(string));
            dt.Columns.Add("EPILEPSY", typeof(string));
            dt.Columns.Add("HEART_DISEASE", typeof(string));
            dt.Columns.Add("INSANITY", typeof(string));
            dt.Columns.Add("DIABETES", typeof(string));
            dt.Columns.Add("HYPERTENSION", typeof(string));
            dt.Columns.Add("OTHER_ILLNESS", typeof(string));
            dt.Columns.Add("OTHER_ILLNESS_DETAILS", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                data.SickOrOnMedication,
                data.MedicationDetails,
                data.IsPregnant,
                data.ExpectedDeliveryMonth,
                data.Smoked,
                data.Tuberculosis,
                data.Epilepsy,
                data.HeartDisease,
                data.Insanity,
                data.Diabetes,
                data.Hypertension,
                data.OtherIllness,
                data.OtherIllnessDetails
            );

            param.Add("@caller", data.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_MISC"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step12DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep12(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_12";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step12DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

        public static void SaveProposalFormTradOtherMedicalInfo(IDbConnection db, AddOtherMedicalInfo data)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_OTHER_MEDICAL_INFO";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("SKIN_DISORDER", typeof(string));
            dt.Columns.Add("NIGHT_SWEATS", typeof(string));
            dt.Columns.Add("WEIGHT_LOSS", typeof(string));
            dt.Columns.Add("SWOLLEN_GLANDS", typeof(string));
            dt.Columns.Add("RECURRENT_DIARRHEA", typeof(string));
            dt.Columns.Add("HEPATITIS_B", typeof(string));
            dt.Columns.Add("HIV_AIDS", typeof(string));
            dt.Columns.Add("BLOOD_TRANSFUSION", typeof(string));
            dt.Columns.Add("PAST_TIME_ACTIVITIES", typeof(string));
            dt.Columns.Add("RESIDE_OUTSIDE_NIGERIA", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                data.SkinDisorder,
                data.NightSweats,
                data.WeightLoss,
                data.SwollenGlands,
                data.RecurrentDiarrhea,
                data.HepatitisB,
                data.HivAids,
                data.BloodTransfusion,
                data.PastTimeActivities,
                data.ResideOutsideNigeria
            );

            param.Add("@caller", data.UserEmail);
            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_OTHER_MEDICAL_INFO"));

            DbServer.SaveData(db, sp, param);
        }

        public static void SaveSupportingDoc(IDbConnection db, SupportingDocFile data)
        {
            var sp = "dbo.CPC_PROPOSAL_FORM_ADD_SUPPORTING_DOCS";
            var param = new DynamicParameters();

            var dt = new DataTable();

            dt.Columns.Add("CPC_REF_NBR", typeof(string));
            dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));
            dt.Columns.Add("PATH", typeof(string));
            //dt.Columns.Add("NAME", typeof(string));
            //dt.Columns.Add("FILE_NAME", typeof(string));
            //dt.Columns.Add("FILE_URL", typeof(string));
            //dt.Columns.Add("CONTENT_DISPOSITION_HEADER_VALUE", typeof(string));
            //dt.Columns.Add("CONTENT_TYPE", typeof(string));
            //dt.Columns.Add("SIZE", typeof(Int32));
            dt.Columns.Add("SHT_DESC", typeof(string));
            dt.Columns.Add("UPDATED_BY", typeof(string));

            dt.Rows.Add(
                data.ReferenceNbr,
                data.ContentTypeCode,
                //data.Name,
                data.FileName,
                //data.FileUrl,
                //data.ContentDispositionHeaderValue,
                //data.ContentType,
                //data.Size,
                data.DocType,
                data.LastUpdatedUser
            );

            param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_SUPPORTING_DOC"));

            DbServer.SaveData(db, sp, param);
        }

        public static Step13DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep13(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_13";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type_code", contenttypecode);

            var lst = DbServer.LoadData<Step13DataCaptureFormTraditionalDto>(db, sp, param);

            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

        public static SupportingDocFile GetFile(IDbConnection db, string refNbr, string idType)
        {
            var sp = "dbo.CPC_GET_FILE";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@doc_type", idType);

            var lst = DbServer.LoadData<SupportingDocFile>(db, sp, param);

            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }
        
        public static List<SupportingDocFile> GetSupportingDocs(IDbConnection db, string refNbr)
        {
            var sp = "dbo.CPC_SUPPORTING_FILES";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);

            var lst = DbServer.LoadData<SupportingDocFile>(db, sp, param);

            return lst;
        }

        public static void SubmitProposalPackContent(IDbConnection db, string refNbr, string contenttypecode)
        {
            var sp = "dbo.CPC_SUBMIT_PROPOSAL_PACK_CONTENT";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", refNbr);
            param.Add("@content_type", contenttypecode);

            DbServer.SaveData(db, sp, param);
        }

        public static void SubmitProposalPack(IDbConnection db, SubmitProposalPackForm payload)
        {
            var sp = "dbo.CPC_SUBMIT_PROPOSAL_PACK";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@caller", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }

        public static void PickInboundProposalPack(IDbConnection db, SubmitProposalPackForm payload)
        {
            var sp = "dbo.CPC_INBOUND_PROPOSAL_PACK";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@picker", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }

        public static void AcceptInboundProposalPack(IDbConnection db, SubmitProposalPackForm payload)
        {
            var sp = "dbo.CPC_ACCEPT_PROPOSAL_PACK";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@picker", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }

        public static void RejectInboundProposalPack(IDbConnection db, SubmitProposalPackForm payload)
        {
            var sp = "dbo.CPC_REJECT_PROPOSAL_PACK";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@picker", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }

        public static void PushInboundProposalPack(IDbConnection db, SubmitProposalPackForm payload)
        {
            var sp = "dbo.CPC_PUSH_PROPOSAL_PACK";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@picker", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }

        public static void ApproveProposalPack(IDbConnection db, SubmitProposalPackForm payload)
        {
            var sp = "dbo.CPC_APPROVE_PROPOSAL_PACK";
            var param = new DynamicParameters();

            param.Add("@ref_nbr", payload.ReferenceNbr);
            param.Add("@approver", payload.UserEmail);

            DbServer.SaveData(db, sp, param);
        }
    }
}




//public static SupportingDocFile FindDataCaptureFormTraditionalStep14IdFile(IDbConnection db, string refNbr, string contenttypecode, string idType)
//{
//    var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_14_ID_FILE";
//    var param = new DynamicParameters();

//    param.Add("@ref_nbr", refNbr);
//    param.Add("@content_type_code", contenttypecode);
//    param.Add("@doc_type", idType);

//    var lst = DbServer.LoadData<SupportingDocFile>(db, sp, param);

//    if (lst != null && lst.Count > 0)
//    {
//        return lst[0];
//    }

//    return null;
//}



//public static void SaveSupportingDocs(IDbConnection db, AddSupportingDocs data, string callerName)
//{
//    var sp = "dbo.CPC_PROPOSAL_FORM_ADD_SUPPORTING_DOCS";
//    var param = new DynamicParameters();

//    var dt = new DataTable();

//    dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
//    dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));

//    dt.Rows.Add(
//        data.ReferenceNbr,
//        data.ContentTypeCode
//    );

//    param.Add("@caller", callerName);
//    param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_SUPPORTING_DOCS"));

//    DbServer.SaveData(db, sp, param);
//}





//public static Step14DataCaptureFormTraditionalDto FindDataCaptureFormTraditionalStep14(IDbConnection db, string refNbr, string contenttypecode)
//{
//    var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_14";
//    var param = new DynamicParameters();

//    param.Add("@ref_nbr", refNbr);
//    param.Add("@content_type_code", contenttypecode);

//    var lst = DbServer.LoadData<Step14DataCaptureFormTraditionalDto>(db, sp, param);

//    if (lst != null && lst.Count > 0)
//    {
//        return lst[0];
//    }

//    return null;
//}


//public static SupportingDocFile FindDataCaptureFormTraditionalStep14PassportFile(IDbConnection db, string refNbr, string contenttypecode)
//{
//    var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_14_PASSPORT_FILE";
//    var param = new DynamicParameters();

//    param.Add("@ref_nbr", refNbr);
//    param.Add("@content_type_code", contenttypecode);

//    var lst = DbServer.LoadData<SupportingDocFile>(db, sp, param);

//    if (lst != null && lst.Count > 0)
//    {
//        return lst[0];
//    }

//    return null;
//}

//public static SupportingDocFile FindDataCaptureFormTraditionalStep14UtilityFile(IDbConnection db, string refNbr, string contenttypecode)
//{
//    var sp = "dbo.CPC_DATA_CAPTURE_FORM_TRAD_STEP_14_UTILITY_FILE";
//    var param = new DynamicParameters();

//    param.Add("@ref_nbr", refNbr);
//    param.Add("@content_type_code", contenttypecode);

//    var lst = DbServer.LoadData<SupportingDocFile>(db, sp, param);

//    if (lst != null && lst.Count > 0)
//    {
//        return lst[0];
//    }

//    return null;
//}



//public static void SaveSupportingPassportDocs(IDbConnection db, SupportingDocFile data, string callerName)
//{
//    var sp = "dbo.CPC_PROPOSAL_FORM_ADD_PASSPORT_DOCS";
//    var param = new DynamicParameters();

//    var dt = new DataTable();

//    dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
//    dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));

//    dt.Rows.Add(
//        data.FileName,
//        data.ContentDispositionHeaderValue,
//        data.ContentType,
//        data.FileUrl,
//        data.Name,
//        data.Size
//    );

//    param.Add("@caller", callerName);
//    param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_PASSPORT_DOCS"));

//    DbServer.SaveData(db, sp, param);
//}

//public static void SaveSupportingUtilityDocs(IDbConnection db, SupportingDocFile data, string callerName)
//{
//    var sp = "dbo.CPC_PROPOSAL_FORM_ADD_UTILITY_BILL_DOCS";
//    var param = new DynamicParameters();

//    var dt = new DataTable();

//    dt.Columns.Add("CPF_CPP_REF_NBR", typeof(string));
//    dt.Columns.Add("CPF_PPCT_TYPE_CODE", typeof(string));

//    dt.Rows.Add(
//        data.FileName,
//        data.ContentDispositionHeaderValue,
//        data.ContentType,
//        data.FileUrl,
//        data.Name,
//        data.Size
//    );

//    param.Add("@caller", callerName);
//    param.Add("@data", dt.AsTableValuedParameter("dbo.UDT_CPC_FORM_TRAD_UTILITY_DOCS"));

//    DbServer.SaveData(db, sp, param);
//}

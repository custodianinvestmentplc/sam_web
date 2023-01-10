using AutoMapper;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Options;
using SAM.WEB.Payloads;

namespace SAM.WEB
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProposalFormTradGeneralOption, Step1DataCaptureFormTraditionalDto>();
            CreateMap<ProposalFormTradTaxDetails, Step2DataCaptureFormTraditionalDto>();
            CreateMap<ProposalFormTradIdentification, Step3DataCaptureFormTraditionalDto>();
            CreateMap<ProposedFormTradBankInfo, Step4DataCaptureFormTraditionalDto>();
            CreateMap<ProposalFormTradMortgageInfo, Step5DataCaptureFormTraditionalDto>();
            CreateMap<ProposedFormTradChildrenEducation, Step6DataCaptureFormTraditionalDto>();
            CreateMap<NewDigitalPlanNomineeForm, Step7DataCaptureFormTraditionalDto>();
            //CreateMap<ProposalFormTradGeneralOption, Step8DataCaptureFormTraditionalDto>();
            CreateMap<NewDataCaptureSumAssured, Step9DataCaptureFormTraditionalDto>();
            CreateMap<AddOtherInsurerOption, Step10DataCaptureFormTraditionalDto>();
            CreateMap<AddMedicalHistoryOption, Step11DataCaptureFormTraditionalDto>();
            CreateMap<AddMiscellaneousOption, Step12DataCaptureFormTraditionalDto>();
            CreateMap<AddOtherMedicalInfo, Step13DataCaptureFormTraditionalDto>();
        }
    }
}
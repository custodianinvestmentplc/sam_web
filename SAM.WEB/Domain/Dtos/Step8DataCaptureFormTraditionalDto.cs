using System;

namespace SAM.WEB.Domain.Dtos
{
    public class Step8DataCaptureFormTraditionalDto
    {
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public string BeneficiaryName { get; set; }
		public DateTime BeneficiaryDob { get; set; }
		public string BeneficiaryRelationship { get; set; }
		public decimal BeneficiaryProportionPct { get; set; }
		public decimal RecordRowId { get; set; }
		public string RecordStatus { get; set; }
	}
}

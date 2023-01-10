using System;

namespace SAM.WEB.Domain.Dtos
{
    public class Step9DataCaptureFormTraditionalDto
    {
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public decimal SumAssured { get; set; }
		public int TermYear { get; set; }
		public decimal FirstPremiumPaid { get; set; }
		public decimal RegularPremium { get; set; }
		public string PaymentFrequency { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime MaturityDate { get; set; }
		public string PaymentMode { get; set; }
	}
}

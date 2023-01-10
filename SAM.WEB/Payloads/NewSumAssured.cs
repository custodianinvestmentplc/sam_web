﻿namespace SAM.WEB.Payloads
{
    public class NewSumAssured
    {
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public decimal SumAssured { get; set; }
		public int TermYear { get; set; }
		public decimal FirstPremiumPaid { get; set; }
		public decimal RegularPremium { get; set; }
		public string PaymentFrequency { get; set; }
		public string FromDate { get; set; }
		public string MaturityDate { get; set; }
		public string PaymentMode { get; set; }
	}
}

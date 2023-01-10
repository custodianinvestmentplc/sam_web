namespace SAM.WEB.Payloads
{
    public class ProposalFormTradMortgageInfo
    {
		public bool IsApplicable { get; set; }
		public string MortgageName { get; set; }
		public string MortgageAddress { get; set; }
		public string InterestRate { get; set; }
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public string UserEmail { get; set; }
	}
}

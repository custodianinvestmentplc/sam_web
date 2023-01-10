namespace SAM.WEB.Payloads
{
    public class ProposalFormTradIdentification
    {
		public string MeansOfIdentification { get; set; }
		public string MeansOfidentificationOthers { get; set; }
		public string IdentifiationNbr { get; set; }
		public string IdCountryOfIssue { get; set; }
		public string IdCountryOfIssueOthers { get; set; }
		public string IdIssuingAuthrity { get; set; }
		public string IdIssueDate { get; set; }
		public string IdExpiryDate { get; set; }
		public string ResidentPermitNbr { get; set; }
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
        public string UserEmail { get; set; }

    }
}

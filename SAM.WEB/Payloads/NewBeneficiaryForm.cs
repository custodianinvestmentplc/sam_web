namespace SAM.WEB.Payloads
{
    public class NewBeneficiaryForm
    {
		public string ReferenceNumber { get; set; }
		public string ContentTypeCode { get; set; }
		public string BeneficiaryName { get; set; }
		public string Dob { get; set; }
		public string Relationship { get; set; }
		public decimal ProportionPercent { get; set; }
	}
}

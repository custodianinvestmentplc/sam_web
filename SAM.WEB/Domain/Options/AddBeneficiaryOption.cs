namespace SAM.WEB.Domain.Options
{
    public class AddBeneficiaryOption
    {
		public string Caller { get; set; }
		public string ReferenceNumber { get; set; }
		public string ContentTypeCode { get; set; }
		public string BeneficiaryName { get; set; }
		public string Dob { get; set; }
		public string Relationship { get; set; }
		public decimal ProportionPercent { get; set; }
	}
}

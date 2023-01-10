namespace SAM.WEB.Payloads
{
    public class NewDigitalPlanNomineeForm
    {
        public bool IsApplicable { get; set; }
		public string NomineeName { get; set; }
		public string NomineeDob { get; set; }
		public string NomineeRelationship { get; set; }
		public decimal SumAssured { get; set; }
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public string UserEmail { get; set; }
	}

	public class DigitalPlanOperationDetails
    {
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public bool IsApplicable { get; set; }
        public string UserEmail { get; set; }
    }
}

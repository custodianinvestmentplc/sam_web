namespace SAM.WEB.Domain.Options
{
    public class AddOtherInsurerOption
    {
		public string ReferenceNbr { get; set; }
		public string UserEmail { get; set; }
        public string ContentTypeCode { get; set; }
        public string HasOtherInsurer { get; set; }
        public string OtherInsurerName { get; set; }
        public decimal OtherSumAssured { get; set; }
        public string PriorProposalDecline { get; set; }
        public string DeclineReason { get; set; }
    }
}

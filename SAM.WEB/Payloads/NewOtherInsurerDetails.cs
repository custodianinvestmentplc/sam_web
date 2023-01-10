namespace SAM.WEB.Payloads
{
    public class NewOtherInsurerDetails
    {
        public string ReferenceNbr { get; set; }
        public string ContentTypeCode { get; set; }
        public string HasOtherInsurer { get; set; }
        public string OtherInsurerName { get; set; }
        public decimal OtherSumAssured { get; set; }
        public string PriorProposalDecline { get; set; }
        public string DeclineReason { get; set; }
    }
}

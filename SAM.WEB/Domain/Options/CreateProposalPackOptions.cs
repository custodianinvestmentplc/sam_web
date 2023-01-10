namespace SAM.WEB.Domain.Options
{
    public class CreateProposalPackOptions
    {
        public string CallerEmail { get; set; }
        public string ProposalTitle { get; set; }
        public string InitiatingBranchCode { get; set; }
        public string InitiatingAgentCode { get; set; }
        public string ProposalPackType { get; set; }
    }
}

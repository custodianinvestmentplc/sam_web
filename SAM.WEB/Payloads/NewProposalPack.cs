using System.Collections.Generic;
using SAM.WEB.Domain.Dtos;

namespace SAM.WEB.Payloads
{
    public class NewProposalPack
    {
        public string Title { get; set; }
        public string UserEmail { get; set; }
        public string BranchCode { get; set; }
        public string AgentCode { get; set; }
        public string ProposalType { get; set; }
        public IReadOnlyCollection<CPCBranchDto> Branches { get; set; }
        //public List<CPCBranchDto> BranchesInput { get; set; }
    }
}

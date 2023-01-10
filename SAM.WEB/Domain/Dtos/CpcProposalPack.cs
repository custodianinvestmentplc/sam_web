using System;

namespace SAM.WEB.Domain.Dtos
{
    public class CpcProposalPack
    {
		public int TableRowId { get; set; }
		public string ReferenceNumber { get; set; }
		public string Title { get; set; }
		public string InitiatingBranchCode { get; set; }
		public string InitatingAgentCode { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreateUserEmail { get; set; }
		public string InitiatingBranchName { get; set; }
		public string InitiatingAgentName { get; set; }
		public string CreateUserName { get; set; }
		public string PpcStatus { get; set; }
        public string ProposalPackType { get; set; }
    }
}

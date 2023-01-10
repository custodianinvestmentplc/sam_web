using System.Data;

namespace SAM.WEB.Payloads
{
    public class DeleteProposalPackFileRequest
    {
        public string ProposalPackReferenceNbr { get; set; }
        public string proposalPackDocName { get; set; }
        public string proposalPackDocType { get; set; }
    }
}
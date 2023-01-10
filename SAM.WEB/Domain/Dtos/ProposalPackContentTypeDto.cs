using System;

namespace SAM.WEB.Domain.Dtos
{
    public class ProposalPackContentTypeDto
    {
        public string ProposalTypeCode { get; set; }
        public string ProposalTypeShortDesc { get; set; }
        public string ProposalTypeDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
}

using System;

namespace SAM.WEB.Domain.Dtos
{
    public class ProposalPackContentDto
    {
        public decimal ProposalPackContentRowId { get; set; }
        public string ProposalPackRefNbr { get; set; }
        public string ContentTypeCode { get; set; }
        public string ContentTypeShortDesc { get; set; }
        public string ContentTypeDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string ContentStatus { get; set; }
    }
}

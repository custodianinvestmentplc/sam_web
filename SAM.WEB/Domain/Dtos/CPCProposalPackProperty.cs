using System.Collections.Generic;
using System.Xml.Linq;
using SAM.WEB.Models;

namespace SAM.WEB.Domain.Dtos
{
    public class CPCProposalPackProperty
    {
        public CpcProposalPack ProposalPack { get; set; }
        public List<SupportingDocFile> Docs { get; set; }
        public List<ProposalPackContentDto> Contents { get; set; }
    }
}
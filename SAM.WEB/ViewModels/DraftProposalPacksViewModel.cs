using System;
using System.Collections.Generic;
using SAM.WEB.Domain.Dtos;
using SAM.WEB.Models;

namespace SAM.WEB.ViewModels
{
    public class DraftProposalPacksViewModel
    {
        public List<CpcProposalPack> CpcProposalPackModel { get; set; } = new List<CpcProposalPack>();
        public string UserEmail { get; set; }
        public string ErrorDescription { get; set; } = null;
        public string ExceptionType { get; set; } = null;
    }
}


using SAM.WEB.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Dtos
{
    public class Step14DataCaptureFormTraditionalDto
    {
        public string ReferenceNbr { get; set; }
        public string ContentTypeCode { get; set; }
        //public IFormFile IdFIle { get; set; }
        //public IFormFile PassportFile { get; set; }
        //public IFormFile UtilityBillFile { get; set; }
        public SupportingDocFile IdFIle { get; set; }
        public SupportingDocFile PassportFile { get; set; }
        public SupportingDocFile UtilityBillFile { get; set; }
    }
}



//public string ReferenceNbr { get; set; }
//public string ContentTypeCode { get; set; }
//public string IdFIleUrl { get; set; }
//public string PassportFileUrl { get; set; }
//public string UtilityBillFileUrl { get; set; }
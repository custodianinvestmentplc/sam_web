using SAM.WEB.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SAM.WEB.Payloads
{
    public class NewSupportingDocsForm
    {
        public string ReferenceNbr { get; set; }
        public string ContentTypeCode { get; set; }
        public IFormFile FormData { get; set; }
        //public List<IFormFile> IdFIle { get; set; }
        //public List<IFormFile> PassportFile { get; set; }
        //public List<IFormFile> UtilityBillFile { get; set; }
    }
}




//public class NewSupportingDocsForm
//{
//    public string ReferenceNbr { get; set; }
//    public string ContentTypeCode { get; set; }
//    public SupportingDocFile IdFIle { get; set; }
//    public SupportingDocFile PassportFile { get; set; }
//    public SupportingDocFile UtilityBillFile { get; set; }
//    //public IList<IFormFile> UploadFiles { get; set; }
//}
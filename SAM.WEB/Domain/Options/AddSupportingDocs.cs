using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Options
{
    public class AddSupportingDocs
    {
		public string ReferenceNbr { get; set; }
        public string ContentTypeCode { get; set; }
        public string IdFIleUrl { get; set; }
        public string PassportFileUrl { get; set; }
        public string UtilityBillFileUrl { get; set; }
        //public IList<IFormFile> UploadFiles { get; set; }
    }
}

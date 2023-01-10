using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using System.Xml.Linq;

namespace SAM.WEB.Models
{
    public class SupportingDocFile
    {
        public string ReferenceNbr { get; set; }
        public string ContentTypeCode { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string ContentDispositionHeaderValue { get; set; }
        public string ContentType { get; set; }
        public string DocType { get; set; }
        public long Size { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedUser { get; set; }
        //public IList<IFormFile> UploadFiles { get; set; }
    }
}
using System;

namespace SAM.WEB.Domain.Dtos
{
    public class CpcTemplateDto
    {
        public int RowId { get; set; }
        public string Template { get; set; }
        public string TemplateType { get; set; }
        public string TemplateEmail { get; set; }
        public string TemplateShortDesc { get; set; }
        public string TemplateDesc { get; set; }
        public string RefTypeRefCode { get; set; }
        public string RefTypeRefCodeDesc { get; set; }
        public string ClassCode { get; set; }
        public string ClassCodeDesc { get; set; }
        public DateTime CreateDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Dtos
{
    public class CpcFileContextDto
    {
        public List<CpcFileDto> Files { get; set; }
        public string ActiveFileType { get; set; }
        public string ReferenceNbr { get; set; }
        public string ContentType { get; set; }
    }
}

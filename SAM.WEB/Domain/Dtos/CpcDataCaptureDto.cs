using System;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Dtos
{
    public class CpcDataCaptureDto
    {
        public List<CpcProductDto> Products { get; set; }
        public List<StatesInNigeriaDto> States { get; set; }
        public string ContentTypeCode { get; set; }
        public string ReferenceNbr { get; set; }
    }
}
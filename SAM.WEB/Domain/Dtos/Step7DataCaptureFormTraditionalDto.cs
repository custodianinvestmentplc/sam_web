using System;

namespace SAM.WEB.Domain.Dtos
{
    public class Step7DataCaptureFormTraditionalDto
    {
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public string NomineeName { get; set; }
		public DateTime NomineeDob { get; set; }
		public string NomineeRelationship { get; set; }
		public decimal SumAssured { get; set; }

	}
}

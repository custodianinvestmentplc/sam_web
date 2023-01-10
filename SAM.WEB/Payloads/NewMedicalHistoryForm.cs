namespace SAM.WEB.Payloads
{
    public class NewMedicalHistoryForm
    {
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
		public string HospitalName { get; set; }
		public string HospitalAddress { get; set; }
		public decimal HeightInMeters { get; set; }
		public decimal WeightInKg { get; set; }
	}
}

using System;

namespace SAM.WEB.Domain.Dtos
{
    public class Step1DataCaptureFormTraditionalDto
    {
		public string Title { get; set; }
		public string OtherTitle { get; set; }
		public string Surname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }
		public DateTime Dob { get; set; }
		public string Gender { get; set; }
		public string GenderOthers { get; set; }
		public string StateOfOrigin { get; set; }
		public string Nationality { get; set; }
		public string NationalityOthers { get; set; }
		public string TownOrCityOfBirth { get; set; }
		public string CountryOfBirth { get; set; }
		public string Address { get; set; }
		public string TownOfAddress { get; set; }
		public string CityOfAddress { get; set; }
		public string StateOfAddress { get; set; }
		public string CountryOfAddress { get; set; }
		public string ProductCode { get; set; }
		public string ReferenceNbr { get; set; }
		public string ContentTypeCode { get; set; }
	}
}

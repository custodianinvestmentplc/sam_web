namespace SAM.WEB.Domain.Dtos.Cts
{
    public class OpenTicketByTechnicianDto
    {
		public int TechnicianId { get; set; }
		public string TechnicianName { get; set; }
		public int AllocatedCount { get; set; }
		public int ReopenCount { get; set; }
		public int ReturnCount { get; set; }
		public int TotalCount { get; set; }
	}
}
           
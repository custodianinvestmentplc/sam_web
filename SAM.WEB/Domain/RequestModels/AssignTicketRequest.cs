namespace SAM.WEB.Domain.RequestModels
{
    public class AssignTicketRequest
    {
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryItemId { get; set; }
        public int SeverityId { get; set; }
        public string TicketNumber { get; set; }
        public int CompanyId { get; set; }
        public string UserEmail { get; set; }
        public int TicketRequestId { get; set; }
        public int TechnicianId { get; set; }
        public int UserId { get; set; }
    }
}

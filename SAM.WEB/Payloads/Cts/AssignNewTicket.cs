namespace SAM.WEB.Payloads.Cts
{
    public class AssignNewTicket
    {
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryItemId { get; set; }
        public int SeverityId { get; set; }
        public string TicketNumber { get; set; }
        public int CompanyId { get; set; }
        public int TechnicianId { get; set; }
        public string UserEmail { get; set; }

    }
}

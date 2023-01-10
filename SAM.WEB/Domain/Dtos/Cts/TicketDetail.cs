using System;

namespace SAM.WEB.Domain.Dtos.Cts
{
    public class TicketDetail
    {
		public int RequestId { get; set; }
        public string Title { get; set; }
        public string TicketContent { get; set; }
        public string SenderName { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TicketNumber { get; set; }
        public string TicketSource { get; set; }
        public string SenderEmail { get; set; }
        public string TicketStage { get; set; }
        public string Technician { get; set; }
    }
}

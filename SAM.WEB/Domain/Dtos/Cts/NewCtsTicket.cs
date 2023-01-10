using System;

namespace SAM.WEB.Domain.Dtos.Cts
{
    public class NewCtsTicket
    {
		public int RequestId { get; set; }
        public string TicketNumber { get; set; }
        public string TitleOfTicket { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DbRecordCreateDate { get; set; }
    }
}

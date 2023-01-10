using System;

namespace SAM.WEB.Domain.Dtos.Cts
{
    public class ActivityLog
    {
        public int ActivityRowId { get; set; }
        public int RequestId { get; set; }
        public string TicketNumber { get; set; }
        public DateTime ActivityCreateDate { get; set; }
        public int ActivityActorId { get; set; }
        public string ActivityActorName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}

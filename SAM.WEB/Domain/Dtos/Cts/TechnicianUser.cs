using System;

namespace SAM.WEB.Domain.Dtos.Cts
{
    public class TechnicianUser
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

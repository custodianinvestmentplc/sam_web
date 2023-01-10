using System;

namespace SAM.WEB.Domain.Dtos
{
    public class CpcRoleDto
    {
        public int RowId { get; set; }
        public string UserRole { get; set; }
        public DateTime CreateDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
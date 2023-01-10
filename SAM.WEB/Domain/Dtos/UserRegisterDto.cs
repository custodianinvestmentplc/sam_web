using System;

namespace SAM.WEB.Domain.Dtos
{
    public class UserRegisterDto
    {
        //public int UserRegisterRowId { get; set; }
        public int TableRowId { get; set; }
        public string UserEmail { get; set;}
        public string UserDisplayName { get; set;}
        public string UserRoleId { get; set;}
        public string UserRole { get; set;}
        //public string UserGroup { get; set;}
        //public string AddedByName { get; set; }
        //public string ReferenceNumber { get; set; }
        public string BranchCode { get; set; }
        //public string AgentCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string AddedByEmail { get; set; }
        public string BranchName { get; set; }
        //public string AgentName { get; set; }
        public string JobDescription { get; set; }
        //public string AgentShortDescription { get; set; }

    }
}

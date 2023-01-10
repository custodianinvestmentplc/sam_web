using System.Collections.Generic;

namespace SAM.WEB.Domain.Dtos
{
    public class CPCBranchAndRolesDto
    {
        public CPCBranchDto[] Branches { get; set; }
        public CpcRoleDto[] Roles { get; set; }
    }
}
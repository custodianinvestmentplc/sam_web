using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;

namespace SAM.WEB.Resources
{
    public class AgentResource
    {
        public AgentDogTagDto DogTag { get; set; }
        public List<OrcHierarchyNodeDto> OrcHierarchy { get; set; } = new List<OrcHierarchyNodeDto>();
    }
}

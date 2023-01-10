using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;

namespace SAM.WEB.Resources
{
    public class OrcScenarioDetailResource
    {
        public string OrcScenarioId { get; set; }
        public List<OrcScenarioProductDto> OrcScenarioProducts { get; set; } = new List<OrcScenarioProductDto>();
        public List<OrcScenarioRateItemDto> OrcScenarioRateItems { get; set; } = new List<OrcScenarioRateItemDto>();
    }
}

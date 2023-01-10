using SAM.WEB.Domain.Dtos;
using SAM.WEB.Resources;
using System.Collections.Generic;

namespace SAM.WEB.Services
{
    public interface IAgentServices
    {
        List<SalesTeamMemberDto> GetAllAgents();
        AgentResource GetAgent(string agentcode);
        List<OrcScenarioDto> GetOrcScenarios();
        OrcScenarioDetailResource GetOrcScenarioDetails(string scenarioId);
        List<SalesTeamMemberDto> FindAgentByName(string searchKey, string Ref);
    }
}

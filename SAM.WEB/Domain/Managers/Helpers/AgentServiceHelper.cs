using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.LocalServices;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class AgentServiceHelper
    {
        public static List<SalesTeamMemberDto> FetchAllAgents(IDbConnection db)
        {
            var sp = "dbo.GetAllSalesTeam";

            var lst = DbServer.LoadData<SalesTeamMemberDto>(db, sp, null);

            return lst;
        }

        public static List<OrcHierarchyNodeDto> GetOrcHierarchy(IDbConnection db, string agentcode, string agentOrcScenario)
        {
            var sp = "dbo.GetAgentOrcHierarchyNodeList";
            var prm = new DynamicParameters();

            prm.Add("@agent_business_code", agentcode);
            prm.Add("@scenario_id", agentOrcScenario);

            var lst = DbServer.LoadData<OrcHierarchyNodeDto>(db, sp, prm);

            return lst;
        }

        public static AgentDogTagDto GetAgentDogTag(IDbConnection db, string agentcode)
        {
            var sp = "dbo.GetAgentDogTag";
            var prm = new DynamicParameters();
            var errMessage = string.Empty;

            prm.Add("@agn_sht_desc", agentcode);

            var lst = DbServer.LoadData<AgentDogTagDto>(db, sp, prm)
                .SingleElseException(rec =>
                {
                    if (rec > 1)
                    {
                        errMessage = $"Too many records found. Expecting 1 record but found { rec } records";
                    }
                    else
                    {
                        errMessage = $"No record found for Agent with Id { agentcode }";
                    }

                    throw new InvalidOperationException(errMessage);
                });

            return lst;
        }

        public static List<OrcScenarioDto> GetAllOrcScenarios(IDbConnection db)
        {
            var sp = "dbo.GetOrcScenarios";
            var lst = DbServer.LoadData<OrcScenarioDto>(db, sp, null);

            return lst;
        }

        public static List<SalesTeamMemberDto> FindAgentByName(IDbConnection db, string searchkey, string Ref)
        {
            var sp = "dbo.cpc_find_agent_by_name";
            var prm = new DynamicParameters();

            prm.Add("@search_key", searchkey);
            prm.Add("@ref", Ref);

            var lst = DbServer.LoadData<SalesTeamMemberDto>(db, sp, prm);

            return lst;
        }
    }
}

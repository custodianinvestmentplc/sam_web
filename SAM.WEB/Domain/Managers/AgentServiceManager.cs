using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.Helpers;
using SAM.WEB.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SAM.WEB.Domain.Managers
{
    public class AgentServiceManager
    {
        private readonly string _connectionString;

        public AgentServiceManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SalesTeamMemberDto> GetAllAgents()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return AgentServiceHelper.FetchAllAgents(db);
        }

        public AgentResource GetAgent(string agentcode)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var dogTag = AgentServiceHelper.GetAgentDogTag(db, agentcode);
            var orcHierarchy = AgentServiceHelper.GetOrcHierarchy(db, agentcode, dogTag.AgentOrcScenarioId);

            return new AgentResource
            {
                DogTag = dogTag,
                OrcHierarchy = orcHierarchy
            };
        }

        public List<OrcScenarioDto> GetAllOrcScenarios()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var data = AgentServiceHelper.GetAllOrcScenarios(db);

            return data;
        }

        public OrcScenarioDetailResource GetOrcScenarioDetails(string scenarioId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var prodlst = CommissionServiceHelper.GetOrcScenarioProducts(db, scenarioId);
            var prodRateLst = CommissionServiceHelper.GetOrcScenarioRates(db, scenarioId);

            var model = new OrcScenarioDetailResource
            {
                OrcScenarioId = scenarioId
            };

            if (prodlst != null)
            {
                foreach (var prod in prodlst)
                {
                    model.OrcScenarioProducts.Add(new OrcScenarioProductDto
                    {
                        ProductType = prod.ProductType,
                        ScenarioId = scenarioId
                    });

                    if (prodRateLst != null)
                    {
                        var prodRates = prodRateLst.Where(x => x.ProductType.Equals(prod.ProductType)).ToList();

                        if (prodRates != null && prodRates.Count > 0)
                        {
                            foreach (var rate in prodRates)
                            {
                                model.OrcScenarioRateItems.Add(new OrcScenarioRateItemDto
                                {
                                    OrcPosition = rate.OrcPosition,
                                    OrcRate = rate.OrcRate,
                                    ProductType = rate.ProductType,
                                    ScenarioId = rate.ScenarioId
                                });
                            }
                        }
                    }
                }
            }

            return model;
        }

        public List<SalesTeamMemberDto> FindAgentByName(string searchKey, string Ref)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return AgentServiceHelper.FindAgentByName(db, searchKey, Ref);
        }
    }
}

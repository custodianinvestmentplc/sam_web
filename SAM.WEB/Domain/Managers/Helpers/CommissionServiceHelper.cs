using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.LocalServices;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class CommissionServiceHelper
    {
        public static List<OrcScenarioProductDto> GetOrcScenarioProducts(IDbConnection db, string scenarioId)
        {
            var sp = "dbo.GetScenarioIdProducts";
            var param = new DynamicParameters();

            param.Add("@scenario_id", scenarioId);

            var lst = DbServer.LoadData<OrcScenarioProductDto>(db, sp, param);

            return lst;
        }

        public static List<OrcScenarioRateItemDto> GetOrcScenarioRates(IDbConnection db, string scenarioId)
        {
            var sp = "dbo.GetScenarioRateItemById";
            var param = new DynamicParameters();

            param.Add("@scenario_id", scenarioId);

            var lst = DbServer.LoadData<OrcScenarioRateItemDto>(db, sp, param);

            return lst;
        }
    }
}

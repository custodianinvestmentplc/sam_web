using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.LocalServices;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class ReportHelper
    {
        public static List<ReportModuleDto> GetReportModuleByUserEmail(IDbConnection db, string useremail)
        {
            var sp = "rpt.GetUserReportModules";
            var param = new DynamicParameters();

            param.Add("@user_email", useremail);

            var moduleLst = DbServer.LoadData<ReportModuleDto>(db, sp, param);

            return moduleLst;
        }

        public static List<ReportItemDto> GetReportItemByModuleId(IDbConnection db, int moduleId, string userEmail)
        {
            var sp = "rpt.GetReportByModuleId";
            var param = new DynamicParameters();

            param.Add("@module_id", moduleId);
            param.Add("user_email", userEmail);

            var reportLst = DbServer.LoadData<ReportItemDto>(db, sp, param);

            return reportLst;
        }
    }
}

using SAM.WEB.Resources;
using System.Collections.Generic;

namespace SAM.WEB.Services
{
    public interface IReportService
    {
        List<ReportModuleResource> GetUserReportModules(string userName);
        List<ReportItemResource> GetReportByModuleId(int moduleId, string userEmail);
    }
}

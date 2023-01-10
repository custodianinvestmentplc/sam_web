using SAM.WEB.Domain.Managers;
using SAM.WEB.Resources;
using SAM.WEB.Services;
using System.Collections.Generic;

namespace SAM.WEB.Domain
{
    public class ReportServiceFacade : IReportService
    {
        private readonly ReportServiceManager _reportManager;

        public ReportServiceFacade(string connstring)
        {
            _reportManager = new ReportServiceManager(connstring);
        }

        public List<ReportItemResource> GetReportByModuleId(int moduleId, string userEmail)
        {
            return _reportManager.GetReportByModuleId(moduleId, userEmail);
        }

        public List<ReportModuleResource> GetUserReportModules(string userName)
        {
            return _reportManager.GetUserReportModules(userName);
        }
    }
}

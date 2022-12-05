using SAM.NUGET.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SAM.WEB.Components
{
    public class UserModulesViewComponent : ViewComponent
    {
        private readonly IReportService _service;

        public UserModulesViewComponent(IReportService reportService)
        {
            _service = reportService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var model = await Task.Run(() => _service.GetUserReportModules(userName));

            return View("ListOfUserModules", model);
        }
    }
}

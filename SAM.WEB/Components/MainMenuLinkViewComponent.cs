using SAM.NUGET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SAM.WEB.Components
{
    public class MainMenuLinkViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var model = await Task.Run(() => new LayoutOptionViewModel());

            model.UserEmail = userName;

            return View("LayoutOptions", model);
        }
    }
}

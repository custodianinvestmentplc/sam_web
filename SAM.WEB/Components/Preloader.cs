using Microsoft.AspNetCore.Mvc;

namespace SAM.WEB.Components
{
    public class Preloader : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

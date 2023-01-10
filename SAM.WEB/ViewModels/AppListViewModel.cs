using System.Collections.Generic;

namespace SAM.WEB.ViewModels
{
    public class AppListViewModel
    {
        public AppListViewModel()
        {
            UserModules = new List<AppModule>();
        }

        public string UserName { get; set; }
        public string UserEmail { get; set; } 
        public List<AppModule> UserModules { get; set; }
    }

    public class AppModule
    {
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public int UserAppRowId { get; set; }
        public string ControllerName { get; set; }
        public string ControllerAction { get; set; }
    }
}

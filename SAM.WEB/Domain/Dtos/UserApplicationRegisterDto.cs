namespace SAM.WEB.Domain.Dtos
{
    public class UserApplicationRegisterDto
    {
        public int UserApplicationRegisterId { get; set; }
        public int UserRegisterRowId { get; set; }
        public int ApplicationRegisterRowId { get; set; }
        public string UserEmail { get; set; }
        public string UserDisplayName { get; set; }
        public string ApplicationTitle { get; set; }
        public string ApplicationSubtitle { get; set; }
        public string ControllerName { get; set; }
        public string ControllerAction { get; set; }
    }
}

using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;

namespace SAM.WEB.Services
{
    public interface IUserServices
    {
        List<UserApplicationRegisterDto> GetUserApplicationModules(string userEmail);
        UserRegisterDto GetUserByEmail(string email);
    }
}

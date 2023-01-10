using SAM.WEB.Models;

namespace SAM.WEB.Services
{
    public interface IAuthProvider
    {
        string AcquireAdToken(string authcode);
        AzureAdUserInfo GetLoggedInUser(string accessToken);
    }
}

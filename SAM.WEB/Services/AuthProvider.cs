using SAM.NUGET.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using SAM.NUGET.Services;

namespace SAM.WEB.Services
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IHttpClientFactory _factory;
        private readonly LoginConfig _config;

        public AuthProvider(IHttpClientFactory clientFactory, LoginConfig config)
        {
            _factory = clientFactory;
            _config = config;
        }

        public string AcquireAdToken(string authcode)
        {
            string[] scopes = new string[]
            {
                "https://graph.microsoft.com/User.Read"
            };

            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(_config.ClientId)
                .WithTenantId(_config.TenantId)
                .WithClientSecret(_config.ClientSecrete)
                .WithRedirectUri(_config.CallbackPath)
                .Build();

            var accessTokenRequest = confidentialClient.AcquireTokenByAuthorizationCode(scopes, authcode);
            var accessToken = accessTokenRequest.ExecuteAsync().Result.AccessToken;

            return accessToken;
        }

        public AzureAdUserInfo GetLoggedInUser(string accessToken)
        {
            var graphUri = "https://graph.microsoft.com/v1.0/me";
            var httpclient = _factory.CreateClient();

            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var resp = httpclient.GetAsync(graphUri).Result;
            resp.EnsureSuccessStatusCode();

            var content = resp.Content.ReadAsStringAsync().Result;
            var adUser = JsonConvert.DeserializeObject<AzureAdUserInfo>(content);

            return adUser;
        }
    }
}

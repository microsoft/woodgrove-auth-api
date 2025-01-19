using System.Text;
using System.Text.Json;

namespace woodgroveapi.Models
{

    public class AzureAppServiceClaimsHeader
    {
        public string auth_typ { get; set; }
        public List<AzureAppServiceClaim> claims { get; set; }
        public string name_typ { get; set; }
        public string role_typ { get; set; }

        public static bool Authorize(HttpRequest req)
        {
            // For all language frameworks, App Service makes the claims in the incoming token 
            // available to your code by injecting them into the request headers.
            // For more information, https://learn.microsoft.com/azure/app-service/configure-authentication-user-identities
            if (req.Headers.TryGetValue("x-ms-client-principal", out var xMsClientPrincipal))
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(xMsClientPrincipal[0]!));
                AzureAppServiceClaimsHeader header = JsonSerializer.Deserialize<AzureAppServiceClaimsHeader>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

                AzureAppServiceClaim azp = header.claims.Find(x => x.typ == "azp")!;

                // Validate that the 'azp' claim contains the 99045fe1-7639-4a75-9d4a-577b6ca3810f value. 
                // This value ensures that the Microsoft Entra is the one who calls the API. 
                // For more information, https://learn.microsoft.com/azure/active-directory/develop/custom-extension-overview#protect-your-rest-api
                return (azp != null) && 
                        azp.val == "99045fe1-7639-4a75-9d4a-577b6ca3810f";
            }

            return false;
        }
    }

    public class AzureAppServiceClaim
    {
        public string typ { get; set; }
        public string val { get; set; }
    }
}

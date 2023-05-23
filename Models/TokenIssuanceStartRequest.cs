using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class TokenIssuanceStartRequest
    {
        [JsonPropertyName("data")]
        public TokenIssuanceStart_Data data { get; set; }
        public TokenIssuanceStartRequest()
        {
            data = new TokenIssuanceStart_Data();
        }
    }

    public class TokenIssuanceStart_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string tenantId { get; set; }
        public string authenticationEventListenerId { get; set; }
        public string customAuthenticationExtensionId { get; set; }
        public AuthenticationContext authenticationContext { get; set; }

    }
}
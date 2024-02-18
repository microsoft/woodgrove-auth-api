using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class TokenIssuanceStartRequest
    {
        [JsonPropertyName("data")]
        public TokenIssuanceStartRequest_Data data { get; set; }
        public TokenIssuanceStartRequest()
        {
            data = new TokenIssuanceStartRequest_Data();
        }
    }

    public class TokenIssuanceStartRequest_Data: AllRequestData
    {

    }
}
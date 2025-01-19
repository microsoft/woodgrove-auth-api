using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class TokenIssuanceStartResponse
    {
        [JsonPropertyName("data")]
        public TokenIssuanceStartResponse_Data data { get; set; }
        public TokenIssuanceStartResponse()
        {
            data = new TokenIssuanceStartResponse_Data();
            data.odatatype = "microsoft.graph.onTokenIssuanceStartResponseData";

            this.data.actions = new List<TokenIssuanceStartResponse_Action>();
            this.data.actions.Add(new TokenIssuanceStartResponse_Action());
        }
    }

    public class TokenIssuanceStartResponse_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public List<TokenIssuanceStartResponse_Action> actions { get; set; }
    }

    public class TokenIssuanceStartResponse_Action
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TokenIssuanceStartResponse_Claims claims { get; set; }

        public TokenIssuanceStartResponse_Action()
        {
            odatatype = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
            claims = new TokenIssuanceStartResponse_Claims();
        }
    }

    public class TokenIssuanceStartResponse_Claims
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CorrelationId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LoyaltyNumber { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LoyaltySince { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LoyaltyTier { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ApiVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("act_as")]
        public string ActAs { get; set; }

        public List<string> CustomRoles { get; set; }
        public TokenIssuanceStartResponse_Claims()
        {
            CustomRoles = new List<string>();
        }
    }
}